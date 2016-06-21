using UnityEngine;
using System.Collections;

public class WandWeapon : MonoBehaviour {

  //TODO change these to ScriptableObjects
  public GameObject idleParticle;
  public GameObject fireParticle;
  public GameObject lightParticle;

  public GameObject fireProjectile;
  public GameObject lightProjectile;

  public enum SpellType
  {
    None,
    Fire,
    Light
  }

  [SerializeField]
  Transform spellcastPoint;
  public float spellFizzleTime = 4f;
  public float maxSpellSize = 25f;
  public float scalingSpeed = 1.1f;
  public bool canFizzle = true;

  //Active OO data
  SpellType currentSpell;
  GameObject currentParticleEffect;
  float spellcastTime = 0f;
  bool castingSpell = false;
  float totalScalingValue = 1f;

  void Start () {
    currentSpell = SpellType.Fire;
    ResetSpell();
    castingSpell = false;
    spellcastTime = 0f;
    //currentParticleEffect = (GameObject)Instantiate(idleParticle, spellcastPoint.position, spellcastPoint.rotation);
  }
	
	void Update () {
	  if(castingSpell)
    {
      spellcastTime += Time.deltaTime;
      if (canFizzle && spellcastTime > spellFizzleTime)
      {
        if(currentSpell != SpellType.Light)
          FizzleSpell();
      }
      else {
        if (totalScalingValue < maxSpellSize)
        {
          ParticleUtilities.ScaleParticleSystem(currentParticleEffect, 1 + (scalingSpeed * Time.deltaTime));
          totalScalingValue *= (1 + (scalingSpeed * Time.deltaTime));
        }

        //switch (currentSpell)
        //{
        //  case SpellType.Fire:
        //    break;
        //  case SpellType.Light:
        //    break;
        //  default:
        //    break;
        //}
      }
    }
	}

  public void NextSpellType()
  {
    Debug.Log("NextSpellType");
    currentSpell = (SpellType)(((int)currentSpell + 1) % System.Enum.GetNames(typeof(SpellType)).Length);
    if (currentSpell == SpellType.None)
    {
      NextSpellType(); //Only want to toggle between active spells
    }
    else {
      if (currentParticleEffect)
      {
        Destroy(currentParticleEffect.gameObject);
      }
      ResetSpell();
    }
  }

  public void StartSpellCast()
  {
    if (currentSpell == SpellType.None)
      return;

    castingSpell = true;
    spellcastTime = 0f;
    totalScalingValue = 1f;
  }

  public int RandomSpell()
  {
    do
    {
      currentSpell = (SpellType)Random.Range(0, System.Enum.GetNames(typeof(SpellType)).Length);
      //currentSpell = (SpellType)(((int)currentSpell + 1) % System.Enum.GetNames(typeof(SpellType)).Length);
    } while (currentSpell == SpellType.None);
  
    if (currentParticleEffect)
    {
      Destroy(currentParticleEffect.gameObject);
    }
    ResetSpell();

    return (int)currentSpell;
  }

  public void ReleaseSpell(Vector3 velocity)
  {
    if (currentSpell == SpellType.None || !castingSpell)
      return;

    GameObject spell = null;
    switch (currentSpell)
    {
      case SpellType.Fire:
        spell = (GameObject)Instantiate(fireProjectile, spellcastPoint.position, spellcastPoint.rotation);
        
        break;
      case SpellType.Light:
        spell = (GameObject)Instantiate(lightProjectile, spellcastPoint.position, spellcastPoint.rotation);
        break;
      default:

        break;
    }
    if(spell != null)
    {
      ParticleUtilities.ScaleParticleSystem(spell, totalScalingValue);
      spell.GetComponent<Rigidbody>().velocity = velocity * 1.5f;
      //spell.GetComponent<Rigidbody>().AddForce(velocity * 100f);
      spell.GetComponent<SpellHitEffect>().scalingValue = totalScalingValue;
      spell.GetComponent<SphereCollider>().radius *= totalScalingValue;
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

    GameObject spellParticle;
    switch (currentSpell)
    {
      case SpellType.Fire:
        spellParticle = fireParticle;
        break;
      case SpellType.Light:
        spellParticle = lightParticle;
        break;
      default:
        spellParticle = null;
        break;
    }
    if (spellParticle != null)
    {
      currentParticleEffect = (GameObject)Instantiate(spellParticle, spellcastPoint.position, spellcastPoint.rotation);
      currentParticleEffect.transform.SetParent(spellcastPoint);
    }
  }

  //void BackToIdle()
  //{
  //  Destroy(currentParticleEffect);
  //  currentSpell = SpellType.None;
  //  currentParticleEffect = (GameObject)Instantiate(idleParticle, spellcastPoint.position, spellcastPoint.rotation);
  //  currentParticleEffect.transform.SetParent(spellcastPoint);
  //  castingSpell = false;
  //}

  public void FizzleSpell()
  {
    switch (currentSpell)
    {
      case SpellType.Fire:
        //TODO make spell "pop" and fade out
        break;
      case SpellType.Light:
        
        break;
      default:
        
        break;
    }
    Destroy(currentParticleEffect);
    castingSpell = false;
    ResetSpell();
    //BackToIdle();
  }
}
