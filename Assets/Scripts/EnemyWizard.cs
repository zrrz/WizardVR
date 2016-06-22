using UnityEngine;
using System.Collections;

public class EnemyWizard : MonoBehaviour, IDamageable {

  Animator animator;
  public Transform player;

  float health = 5f;

  public enum State
  {
    Idle,
    Attacking,
    Moving,
    Blocking,
    Fainted
  }

  public State state = State.Idle;

  public WandWeapon wandWeapon;
  public float attackCooldown;
  public float forwardAttackStrength = 8f;
  public float upAttackStrength = 1.1f;
  public float moveSpeed = 2f;

  public Vector3 moveTarget = Vector3.zero;

  public Transform moveZone;

  CharacterController characterController;

  void Start () {
    animator = GetComponentInChildren<Animator>();
    characterController = GetComponentInChildren<CharacterController>();
    animator.SetBool("Recover", true);
    ResetAttackCooldown();
  }

  void Update() {
    animator.SetFloat("MoveX", 0f);
    animator.SetFloat("MoveY", 0f);

    switch (state)
    {
      case State.Idle:
        break;
      case State.Moving:
        if (moveTarget == Vector3.zero || Vector3.Distance(characterController.transform.position, moveTarget) < 0.1f)
          GetNewMoveTarget();
        Debug.DrawRay(moveTarget, Vector3.up);
        Vector3 dir = (characterController.transform.position - moveTarget).normalized;
        //characterController.SimpleMove(dir * moveSpeed);
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.z);
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

  void GetNewMoveTarget()
  {
    moveTarget = Vector3.Lerp(moveZone.GetComponent<BoxCollider>().bounds.min, moveZone.GetComponent<BoxCollider>().bounds.max, Random.Range(0f, 1f));
    moveTarget.y = transform.position.y;
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

  public void ReleaseAttack()
  {
    Debug.Log("AttackFinished");
    Vector3 playerPos = player.transform.position + Vector3.up; //Assuming 2 meters tall
    wandWeapon.ReleaseSpell(((playerPos - wandWeapon.transform.position).normalized * forwardAttackStrength) + Vector3.up * upAttackStrength);
    //state = State.Moving;
  }

  public void AttackFinished()
  {
    Debug.Log("AttackFinished");
    //wandWeapon.ReleaseSpell(((player.transform.position - wandWeapon.transform.position).normalized * 30f) + Vector3.up * 0f);
    state = State.Moving;
  }

  public void Damage(float damage)
  {
    state = State.Moving;
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
    state = State.Fainted;
  }
}
