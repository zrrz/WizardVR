using UnityEngine;
using System.Collections;

public class EnemyWizard : MonoBehaviour {

  Animator animator;
  public Transform player;

  float health = 5f;

  public enum State
  {
    Idle,
    Attacking,
    Moving,
    Blocking
  }

  public State state = State.Idle;

  public WandWeapon wandWeapon;
  public float attackCooldown;

	void Start () {
    animator = GetComponentInChildren<Animator>();
    animator.SetBool("Recover", true);
    ResetAttackCooldown();
  }

  void Update() {
    switch (state)
    {
      case State.Idle:
        attackCooldown -= Time.deltaTime;
        if (attackCooldown < 0f)
        {
          ResetAttackCooldown();
          Attack();
        }
        break;
      case State.Attacking:
        break;
      default:
        break;
    }
  }

  void OnCollisionEnter(Collision col)
  {
    //if (col.gameObject.GetComponent<Player>() != null)
    //{
    //  //TODO Do damage
    //  //TODO Play sound
    //}
    //else
    //if (col.gameObject.GetComponent<SpellHitEffect>() != null)
    //{
    //  TakeDamage(1f); //TODO take variable damage from SpellHitEffect
    //}
  }

  void ResetAttackCooldown()
  {
    attackCooldown = Random.Range(2f, 4.5f);
  }

  public void Attack()
  {
    int spellNum = wandWeapon.RandomSpell();
    animator.SetTrigger("Attack" + spellNum);
    wandWeapon.StartSpellCast();
    state = State.Attacking;
  }

  public void AttackFinished()
  {
    Debug.Log("AttackFinished");
    wandWeapon.ReleaseSpell(((player.transform.position - wandWeapon.transform.position).normalized * 30f) + Vector3.up * 0f);
    state = State.Idle;
  }

  void TakeDamage(float damage)
  {
    animator.SetTrigger("Hit");
    health -= damage;
    if (health < 0f)
    {
      Faint();
    }
  }

  void Faint()
  {
    animator.SetBool("Recover", false);
    GetComponent<CharacterController>().enabled = false;
  }
}
