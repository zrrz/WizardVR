using UnityEngine;
using System.Collections;

public class WandInteractable : WVRInteractable {

	void Start () {
	
	}
	
	void Update () {
	
	}

  public Transform wandTransform;

  public override void Interact(WVRController controller)
  {
    base.Interact(controller);
    AttachWandToController(controller);
  }

  void AttachWandToController(WVRController controller)
  {
    controller.HideViveController(true);
    controller.controllerState = WVRController.ControllerState.WandHeld;
    wandTransform.localPosition = Vector3.zero;
    wandTransform.SetParent(transform, false);
    Destroy(gameObject);
  }
}
