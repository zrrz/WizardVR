using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IDamageable{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  //The required method of the IDamageable interface
  public void Damage(float damageTaken)
  {
    //Do something fun
  }
}
