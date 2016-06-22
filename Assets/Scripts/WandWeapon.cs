using UnityEngine;
using System.Collections;

public class WandWeapon : MonoBehaviour {

  //TODO change these to ScriptableObjects
  //public GameObject idleParticle;
  //public GameObject fireParticle;
  //public GameObject lightParticle;

  //public GameObject fireProjectile;
  //public GameObject lightProjectile;

  public SpellData[] spellData;

  //public enum SpellType
  //{
  //  None,
  //  Fire,
  //  Light
  //}

  [SerializeField]
  Transform spellcastPoint;
  //public float spellFizzleTime = 4f;
  //public float maxSpellSize = 25f;
  //public float scalingSpeed = 1.1f;
  //public bool canFizzle = true;

  //Active OO data
  //SpellType currentSpell;
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
          ParticleUtilities.ScaleParticleSystem(currentParticleEffect, 1 + (scalingSpeed * Time.deltaTime));
          totalSpellScaling *= (1 + (scalingSpeed * Time.deltaTime));
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
    //currentSpell = (SpellType)(((int)currentSpell + 1) % System.Enum.GetNames(typeof(SpellType)).Length);
    //if (currentSpell == SpellType.None)
    //{
      //NextSpellType(); //Only want to toggle between active spells
    //}
    //else {
    //if (currentParticleEffect)
    //{
    //  Destroy(currentParticleEffect.gameObject);
    //}
    ResetSpell();
    //}
  }

  public void StartSpellCast()
  {
    //if (currentSpell == SpellType.None)
    //  return;

    castingSpell = true;
    spellcastTime = 0f;
    totalSpellScaling = 1f;
  }

  public int RandomSpell()
  {
    currentSpell = Random.Range(0, spellData.Length);
    //do
    //{
    //  currentSpell = (SpellType)Random.Range(0, System.Enum.GetNames(typeof(SpellType)).Length);
    //  //currentSpell = (SpellType)(((int)currentSpell + 1) % System.Enum.GetNames(typeof(SpellType)).Length);
    //} while (currentSpell == SpellType.None);
  
    //if (currentParticleEffect)
    //{
    //  Destroy(currentParticleEffect.gameObject);
    //}
    ResetSpell();

    return currentSpell;
  }

  public void ReleaseSpell(Vector3 velocity)
  {
    if (!castingSpell) //Redundant but safe. TODO cleanup
      return;

    GameObject spell = (GameObject)Instantiate(spellData[currentSpell].projectileObject, spellcastPoint.position, spellcastPoint.rotation);
    //switch (currentSpell)
    //{
    //  case SpellType.Fire:
    //    spell = (GameObject)Instantiate(fireProjectile, spellcastPoint.position, spellcastPoint.rotation);
        
    //    break;
    //  case SpellType.Light:
    //    spell = (GameObject)Instantiate(lightProjectile, spellcastPoint.position, spellcastPoint.rotation);
    //    break;
    //  default:

    //    break;
    //}
    if(spell != null)
    {
      ParticleUtilities.ScaleParticleSystem(spell, totalSpellScaling);
      spell.GetComponent<Projectile>().Activate(velocity, spellcastPoint, transform.up, totalSpellScaling);
      castingSpell = false;
      //Destroy(spell, 4f);
    }
    ResetSpell();
    //BackToIdle();
  }

  void ResetSpell()
  {
    if (currentParticleEffect != null)
      Destroy(currentParticleEffect);

    GameObject spellParticle = spellData[currentSpell].castingObject;
    //switch (currentSpell)
    //{
    //  case SpellType.Fire:
    //    spellParticle = fireParticle;
    //    break;
    //  case SpellType.Light:
    //    spellParticle = lightParticle;
    //    break;
    //  default:
    //    spellParticle = null;
    //    break;
    //}
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
    //switch (currentSpell)
    //{
    //  case SpellType.Fire:
    //    //TODO make spell "pop" and fade out
    //    break;
    //  case SpellType.Light:
        
    //    break;
    //  default:
        
    //    break;
    //}
    Destroy(currentParticleEffect);
    castingSpell = false;
    ResetSpell();
  }
}
