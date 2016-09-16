using UnityEngine;
using System.Collections;

public class WandWeapon : MonoBehaviour {
  public SpellData[] spellData;

  [SerializeField]
  Transform spellcastPoint;

  //Active OO data
  int currentSpell;
  GameObject currentParticleEffect;
  float spellcastTime = 0f;
  bool castingSpell = false;
  float totalSpellScaling = 1f;

  void Start () {
    currentSpell = 0;
    ResetSpell();
    castingSpell = false;
    spellcastTime = 0f;
  }

  void Update () {
	if(castingSpell)
    {
      spellcastTime += Time.deltaTime;
      if (spellData[currentSpell].canFizzle && spellcastTime > spellData[currentSpell].spellFizzleTime)
      {
        FizzleSpell();
      }
      else {
        if (totalSpellScaling < spellData[currentSpell].maxSpellSize)
        {
		  float scalingSpeed = spellData[currentSpell].scalingSpeed;
		  totalSpellScaling *= (1 + (scalingSpeed * Time.deltaTime));
		  if (currentParticleEffect.GetComponent<SpellCharge> () != null) {
			currentParticleEffect.GetComponent<SpellCharge> ().scalingSpeed = scalingSpeed;
			currentParticleEffect.GetComponent<SpellCharge> ().UpdateCharge ();
			currentParticleEffect.GetComponent<SpellCharge> ().totalSpellScaling = totalSpellScaling;
		  }
//          ParticleUtilities.ScaleParticleSystem(currentParticleEffect, 1 + (scalingSpeed * Time.deltaTime), false);
        }
      }
    }
  }

  public void NextSpellType()
  {
    //Debug.Log("NextSpellType");
    currentSpell++;
    if (currentSpell > spellData.Length)
      currentSpell = 0;
    ResetSpell();
  }

  public void StartSpellCast()
  {
    castingSpell = true;
    spellcastTime = 0f;
    totalSpellScaling = 1f;
	if (currentParticleEffect.GetComponent<SpellCharge> () != null) {
	  currentParticleEffect.GetComponent<SpellCharge> ().StartCharge ();
	}
  }

  public int RandomSpell()
  {
    currentSpell = Random.Range(0, spellData.Length);
    ResetSpell();
    return currentSpell;
  }

  public void ReleaseSpell(Vector3 velocity)
  {
    if (!castingSpell) //Redundant but safe. TODO cleanup
      return;

	if (currentParticleEffect.GetComponent<SpellCharge> () != null) {
	  currentParticleEffect.GetComponent<SpellCharge> ().ReleaseCharge ();
	}

    GameObject spell = (GameObject)Instantiate(spellData[currentSpell].projectileObject, spellcastPoint.position, spellcastPoint.rotation);
    if(spell != null)
    {
      ParticleUtilities.ScaleParticleSystem(spell, totalSpellScaling);
      spell.GetComponent<Projectile>().Activate(velocity, spellcastPoint, transform.up, totalSpellScaling);
//      spell.GetComponent<Projectile>().collisionObject = spellData[currentSpell].impactObject;
      castingSpell = false;
    }

	float scalingSpeed = spellData[currentSpell].scalingSpeed;
	if (currentParticleEffect.GetComponent<SpellCharge> () != null) {
//		currentParticleEffect.GetComponent<SpellCharge> ().scalingSpeed = scalingSpeed;
//		currentParticleEffect.GetComponent<SpellCharge> ().UpdateCharge ();
			ParticleUtilities.ScaleParticleSystem(currentParticleEffect.transform.FindChild ("ChargeParticle").gameObject, 1f/totalSpellScaling);
			totalSpellScaling = 1f;
			currentParticleEffect.GetComponent<SpellCharge> ().totalSpellScaling = totalSpellScaling;
	}

//    ResetSpell();
  }

  void ResetSpell()
  {
    if (currentParticleEffect != null)
      Destroy(currentParticleEffect);

    GameObject spellParticle = spellData[currentSpell].castingObject;
    if (spellParticle != null)
    {
      currentParticleEffect = (GameObject)Instantiate(spellParticle, spellcastPoint.position, spellcastPoint.rotation);
      currentParticleEffect.transform.SetParent(spellcastPoint);
    }
  }

  public float ActiveSpellStrength()
  {
    if (!castingSpell)
      return 0f;
    return totalSpellScaling / spellData[currentSpell].maxSpellSize;
  }

  public void FizzleSpell()
  {
    Destroy(currentParticleEffect);
    castingSpell = false;
	if (currentParticleEffect.GetComponent<SpellCharge> () != null) {
	  currentParticleEffect.GetComponent<SpellCharge> ().FizzleCharge ();
	}
    ResetSpell();
  }
}
