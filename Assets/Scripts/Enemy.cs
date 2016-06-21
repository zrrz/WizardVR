using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

  Player player;
  public float turnSpeed = 0.5f;
  public float moveSpeed = 2f;

  void Start () {
    player = FindObjectOfType<Player>();
	}
	
	void Update () {
	  if(player != null)
    {
      Vector3 lookDir = player.transform.position - transform.position;
      Debug.DrawRay(transform.position, lookDir);
      Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized, transform.up);
      transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
      transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
	}

  void OnCollisionEnter(Collision col)
  {
    if(col.gameObject.GetComponent<Player>() != null)
    {
      //TODO Do damage
      //TODO Play sound
      Destroy(gameObject);
    }
    else if(col.gameObject.GetComponent<SpellHitEffect>() != null)
    {
      Destroy(gameObject, 2f);
    }
  }
}
