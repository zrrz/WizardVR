using UnityEngine;
using System.Collections;

public class SpellData : ScriptableObject {
  public GameObject castingObject;
  public GameObject projectileObject;
  public GameObject impactObject;

  public bool canFizzle = false;
  public float spellFizzleTime = 5f;

  public float maxSpellSize = 10f;
  public float scalingSpeed = 1.1f;

  //public virtual void CleanupCasting()
  //{
  //  Destroy(castingObject);
  //}

  //public virtual void CleanupProjectile()
  //{
  //  Destroy(projectileObject);
  //}

  //public virtual void CleanupImpact()
  //{
  //  Destroy(impactObject);
  //}


}
