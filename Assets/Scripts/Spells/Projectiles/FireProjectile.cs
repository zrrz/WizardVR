using UnityEngine;
using System.Collections;
using System;

public class FireProjectile : Projectile {

  public override void Activate(Vector3 velocity, Transform startPosition, Vector3 direction, float scalingValue)
  {
    this.scalingValue = scalingValue;
    this.startPosition = startPosition;

    GetComponent<Rigidbody>().velocity = velocity * 2f;
    GetComponent<SphereCollider>().radius *= scalingValue;
	transform.FindChild ("Ball").localScale *= scalingValue;
  }

	void Update() {
		transform.LookAt (transform.position + GetComponent<Rigidbody> ().velocity.normalized);
	}

  void OnCollisionEnter(Collision col)
  {
    Destroy(gameObject);
    if (collisionObject != null)
    {
      Vector3 point = col.contacts[0].point;
      Vector3 dir = transform.position - point;
      ApplyOverlapSphereDamage(col.contacts[0].point, GetComponent<SphereCollider>().radius * scalingValue, 1f);
      GameObject explosion = (GameObject)Instantiate(collisionObject, point, Quaternion.LookRotation(dir.normalized));
      ParticleUtilities.ScaleParticleSystem(explosion, scalingValue);
	  Destroy (explosion, 7f);
    }
    else
    {
      Debug.LogError("explosionObject not set", this);
    }
  }
}
