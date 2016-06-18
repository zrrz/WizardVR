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
    controller.SwitchToWand();
    wandTransform.gameObject.SetActive(false);
    //controller.controllerState = WVRController.ControllerState.WandHeld;
    //wandTransform.localPosition = Vector3.zero;
    //wandTransform.SetParent(controller.wandAttachPoint, true);
    //Destroy(gameObject);
  }
}
