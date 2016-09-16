using UnityEngine;
using System.Collections;

public class SimpleProp : MonoBehaviour, IDamageable {

	new Rigidbody rigidbody;

	void Awake() {
		rigidbody = GetComponent<Rigidbody> ();
		if (rigidbody == null) {
			Debug.LogError ("Should have a rigidbody", this);
		}
	}

	void Start () {
	
	}
	
	void Update () {
	
	}

	public void Damage(float damage) {

	}
}
