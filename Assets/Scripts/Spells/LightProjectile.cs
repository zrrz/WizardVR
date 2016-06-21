using UnityEngine;
using System.Collections;

public class LightProjectile : SpellHitEffect {

  public float lifeTime;

  void OnCollisionEnter(Collision col)
  {
    GetComponent<Rigidbody>().isKinematic = true;
    GetComponent<SphereCollider>().enabled = false;
    Collider[] overlapCols = Physics.OverlapSphere(col.contacts[0].point, GetComponent<SphereCollider>().radius * scalingValue);
    foreach (Collider overlapCol in overlapCols)
    {
      overlapCol.SendMessage("TakeDamage", 1f, SendMessageOptions.DontRequireReceiver);
    }
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
