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

  //Active OO data
  SpellType currentSpell;
  GameObject currentParticleEffect;
  float spellcastTime = 0f;


  void Start () {
    currentSpell = SpellType.None;
    currentParticleEffect = (GameObject)Instantiate(idleParticle, spellcastPoint.position, spellcastPoint.rotation);
  }
	
	void Update () {
	  if(currentSpell != SpellType.None) //Currently casting
    {
      spellcastTime += Time.deltaTime;
      if (spellcastTime > spellFizzleTime)
      {
        FizzleSpell();
      }
      else {
        currentParticleEffect.GetComponent<ParticleSystem>().startSize += 1 * Time.deltaTime;
        currentParticleEffect.GetComponent<ParticleSystem>().startSize += 1 * Time.deltaTime;

        switch (currentSpell)
        {
          case SpellType.Fire:
            break;
          case SpellType.Light:
            break;
          default:
            break;
        }
      }
    }
	}

  public void NextSpellType()
  {
    currentSpell = (SpellType)(((int)currentSpell + 1) % System.Enum.GetNames(typeof(SpellType)).Length);
  }

  public void StartSpellCast()
  {
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
      currentParticleEffect = (GameObject)Instantiate(spellParticle, spellcastPoint.position, spellcastPoint.rotation);
  }

  void BackToIdle()
  {
    Destroy(currentParticleEffect);
    currentSpell = SpellType.None;
    currentParticleEffect = (GameObject)Instantiate(idleParticle, spellcastPoint.position, spellcastPoint.rotation);
  }

  public void FizzleSpell()
  {
    switch (currentSpell)
    {
      case SpellType.Fire:
        
        break;
      case SpellType.Light:
        
        break;
      default:
        
        break;
    }
  }
}
