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
      Collider[] overlapCols = Physics.OverlapSphere(point, GetComponent<SphereCollider>().radius * scalingValue);
      foreach(Collider overlapCol in overlapCols)
      {
        overlapCol.SendMessage("TakeDamage", 1f, SendMessageOptions.DontRequireReceiver);
      }
      GameObject explosion = (GameObject)Instantiate(explosionObject, point, Quaternion.LookRotation(dir.normalized));
      ParticleUtilities.ScaleParticleSystem(explosion, scalingValue);
    }
    else
    {
      Debug.LogError("explosionObject not set", this);
    }
  }
}
