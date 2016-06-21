using UnityEngine;
using System.Collections;

public class FireProjectile : SpellHitEffect {

  public GameObject explosionObject;

  void OnCollisionEnter(Collision col)
  {
    Destroy(gameObject);
    if (explosionObject != null)
    {
      Vector3 point = col.contacts[0].point;
      Vector3 dir = transform.position - point;
      GameObject explosion = (GameObject)Instantiate(explosionObject, point, Quaternion.LookRotation(dir.normalized));
      ParticleUtilities.ScaleParticleSystem(explosion, scalingValue);
      Destroy(gameObject);
    }
    else
    {
      Debug.LogError("explosionObject not set", this);
    }
  }
}
