using UnityEngine;
using System.Collections;

public class EnemyWizardAnimMessager : MonoBehaviour {

  EnemyWizard enemyWizard;

	void Awake () {
    enemyWizard = GetComponentInParent<EnemyWizard>();
	}


  public void AttackFinished()
  {
    enemyWizard.AttackFinished();
  }

  public void ReleaseAttack()
  {
    enemyWizard.ReleaseAttack();
  }
}
