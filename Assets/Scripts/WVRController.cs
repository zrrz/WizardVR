using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WVRController : MonoBehaviour {

  private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
  private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

  SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index);} }
  SteamVR_TrackedObject trackedObj;

  public Transform wandAttachPoint;

  public enum ControllerState
  {
    NoWand,
    WandHeld
  }

  public ControllerState controllerState = ControllerState.NoWand;

  List<WVRInteractable> touchedInteractables = new List<WVRInteractable>();

  void Start () {
    trackedObj = GetComponent<SteamVR_TrackedObject>();

    var deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
    if (deviceIndex != -1 && SteamVR_Controller.Input(deviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
      SteamVR_Controller.Input(deviceIndex).TriggerHapticPulse(1000);
  }

  void Update () {
    if (controller == null)
    {
      Debug.Log("Controller not initialized");
      return;
    }

    if (controller.GetPressDown(triggerButton) && touchedInteractables.Count > 0)
    {
      foreach (WVRInteractable interactable in touchedInteractables)
      {
        interactable.Interact(this);
        Debug.Log("interacting");
      }
    }

    switch (controllerState)
    {
      case ControllerState.NoWand:
        
        break;
      case ControllerState.WandHeld:

        break;
      default:
        break;
    } 
	}

  void OnTriggerEnter(Collider col)
  {
    WVRInteractable interactable = col.GetComponent<WVRInteractable>();
    if (interactable != null)
    {
      touchedInteractables.Add(interactable);
    }
    Debug.Log("OnTriggerEnter");
  }

  void OnTriggerExit(Collider col)
  {
    WVRInteractable interactable = col.GetComponent<WVRInteractable>();
    if (interactable != null)
    {
      touchedInteractables.Remove(interactable);
    }
  }

  public void SwitchToWand()
  {
    HideViveController(true);
    wandAttachPoint.gameObject.SetActive(true);
  }

  public void HideViveController(bool hide)
  {
    transform.GetChild(0).gameObject.SetActive(false);
  }
}
