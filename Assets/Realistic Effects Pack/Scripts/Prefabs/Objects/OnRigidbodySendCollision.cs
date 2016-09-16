using UnityEngine;
using System.Collections;

public class OnRigidbodySendCollision : MonoBehaviour {

  private EffectSettings effectSettings;

  private void GetEffectSettingsComponent(Transform tr)
  {
		//if(effectSettings
    var parent = tr.parent;
    if (parent != null)
    {
      effectSettings = parent.GetComponentInChildren<EffectSettings>();
      if (effectSettings == null)
        GetEffectSettingsComponent(parent.transform);
    }
  }
  void Start()
  {
    GetEffectSettingsComponent(transform);
  }

  void OnCollisionEnter(Collision collision)
  {
//		CollisionInfo col = new CollisionInfo ();
//		col.Hit = new RaycastHit ();
////		col.Hit.collider
    effectSettings.OnCollisionHandler(new CollisionInfo());
  }
}
