using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

  public float spawnRadius;
  public GameObject enemyPrefab;
  public float waveFrequency;
  public float maxY;
  public int minWaveSpawns = 1, maxWaveSpawns = 4;
  float timer = 0f;

  [HideInInspector]
  public bool spawnsStarted = false;

  void Start () {
	  
	}
	
	void Update () {
    if (!spawnsStarted)
      return;

    timer += Time.deltaTime;
    if(timer > waveFrequency)
    {
      SpawnEnemy();
      timer = 0f;
    }
	}

  void SpawnEnemy()
  {
    for (int i = 0, n = Random.Range(minWaveSpawns, maxWaveSpawns); i < n; i++)
    {
      Vector3 spawnPos = Random.onUnitSphere * spawnRadius;
      spawnPos.y = Mathf.Clamp(spawnPos.y, -maxY, maxY);
      Instantiate(enemyPrefab, transform.position + spawnPos, Quaternion.LookRotation((transform.position - spawnPos).normalized));
    }
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.DrawWireSphere(transform.position, spawnRadius);
    Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadius, maxY, spawnRadius));
  }
}
