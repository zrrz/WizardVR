using UnityEngine;
using System.Collections;
using System;

public class LightProjectile : Projectile {

  public float lifeTime;

  public override void Activate(Vector3 velocity, Transform startPosition, Vector3 direction, float scalingValue)
  {
    this.scalingValue = scalingValue;
    this.startPosition = startPosition;

    GetComponent<Rigidbody>().velocity = velocity * 2f;
    GetComponent<SphereCollider>().radius *= scalingValue;
  }

  void OnCollisionEnter(Collision col)
  {
    GetComponent<Rigidbody>().isKinematic = true;
    GetComponent<SphereCollider>().enabled = false;
    ApplyOverlapSphereDamage(col.contacts[0].point, GetComponent<SphereCollider>().radius * scalingValue, 1f);
    transform.SetParent(col.transform);
    StartCoroutine(FadeOut());
  }

  IEnumerator FadeOut()
  {
    for(float t = 0f; t < lifeTime; t += Time.deltaTime)
    {
      yield return null;
      ParticleUtilities.ScaleParticleSystem(gameObject, 1 - (0.3f * Time.deltaTime)); //TODO fix this math so it times properly
    }
    Destroy(gameObject);
  }
}
