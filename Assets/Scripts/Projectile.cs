using UnityEngine;
using System.Collections;

public abstract class Projectile: MonoBehaviour
{
  protected Vector3 velocity;
  protected Transform startPosition;
  protected Vector3 direction;
  protected float scalingValue;
  public GameObject collisionObject;

  public abstract void Activate(Vector3 velocity, Transform startPosition, Vector3 direction, float scalingValue);

  protected void ApplyOverlapSphereDamage(Vector3 center, float radius, float damage)
  {
    Collider[] overlapCols = Physics.OverlapSphere(center, radius);
    foreach (Collider overlapCol in overlapCols)
    {
      IDamageable damageable = overlapCol.GetComponent<IDamageable>();
      if (damageable != null)
        damageable.Damage(damage);
    }
  }
}
