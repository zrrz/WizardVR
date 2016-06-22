using UnityEngine;
using System.Collections;

public class WandInteractable : MonoBehaviour, IInteractable {

	void Start () {
	
	}
	
	void Update () {
	
	}

  public Transform wandTransform;

  public void Interact(WVRController controller)
  {
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
