using UnityEngine;
using System.Collections;

public abstract class SpellCharge : MonoBehaviour {
	public float totalSpellScaling;
	public float scalingSpeed;

	public abstract void StartCharge ();
	public abstract void UpdateCharge ();
	public abstract void ReleaseCharge ();
	public abstract void FizzleCharge ();
}
