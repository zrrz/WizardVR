using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WVRController : MonoBehaviour {

  private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
  private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
  private Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

  SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index);} }
  SteamVR_TrackedObject trackedObj;

  public Transform wandAttachPoint;

  WandWeapon wandWeapon;

  public enum ControllerState
  {
    NoWand,
    WandHeld
  }

  public ControllerState controllerState = ControllerState.NoWand;

  List<IInteractable> touchedInteractables = new List<IInteractable>();

  //List<Vector3> controllerPositions;
  Vector3 velocity;

  void Start () {
	UnityEngine.VR.VRSettings.renderScale = 1.5f;
    trackedObj = GetComponent<SteamVR_TrackedObject>();

    var deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
    if (deviceIndex != -1 && SteamVR_Controller.Input(deviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
      SteamVR_Controller.Input(deviceIndex).TriggerHapticPulse(1000);

    //controllerPositions = new List<Vector3>();
    wandWeapon = GetComponentInChildren<WandWeapon>(true); //TODO not have WandWeapon on controller at start

		SwitchToWand ();
  }

  void Update () {
    if (controller == null)
    {
      Debug.Log("Controller not initialized");
      return;
    }

    CalculateControllerVelocity();

    switch (controllerState)
    {
      case ControllerState.WandHeld:
        if (wandWeapon == null)
        {
          Debug.LogError("wandWeapon is null");
          return;
        }
        if (controller.GetPressDown(triggerButton))
        {
          wandWeapon.StartSpellCast();
        }
        else if(controller.GetPress(triggerButton))
        {
          controller.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, wandWeapon.ActiveSpellStrength()));
        }
        else if (controller.GetPressUp(triggerButton))
        {
          wandWeapon.ReleaseSpell(velocity);
        }
        if (controller.GetPressDown(touchPad))
        {
          wandWeapon.NextSpellType();
        }
        break;
      case ControllerState.NoWand:
        if (controller.GetPressDown(triggerButton) && touchedInteractables.Count > 0)
        {
          foreach (IInteractable interactable in touchedInteractables)
          {
            interactable.Interact(this);
            //Debug.Log("interacting");
          }
        }
        break;
      
      default:
        break;
    } 
	}

  void OnTriggerEnter(Collider col)
  {
    IInteractable interactable = col.GetComponent<IInteractable>();
    if (interactable != null)
    {
      touchedInteractables.Add(interactable);
    }
    Debug.Log("OnTriggerEnter");
  }

  void OnTriggerExit(Collider col)
  {
    IInteractable interactable = col.GetComponent<IInteractable>();
    if (interactable != null)
    {
      touchedInteractables.Remove(interactable);
    }
  }

  public void SwitchToWand()
  {
    HideViveController(true);
    wandAttachPoint.gameObject.SetActive(true);
    controllerState = ControllerState.WandHeld;
    //FindObjectOfType<EnemySpawner>().spawnsStarted = true;
	if(FindObjectOfType<EnemyWizard>() != null)
      FindObjectOfType<EnemyWizard>().state = EnemyWizard.State.Moving;
//    GameObject.Find("Pedestal").GetComponent<Animation>().Play("Pedestal_Sink");
  }

  public void HideViveController(bool hide)
  {
    transform.GetChild(0).gameObject.SetActive(false);
  }

  void CalculateControllerVelocity()
  {
    velocity = controller.velocity;// new Vector3(controller.GetPose().vVelocity.v0, controller.GetPose().vVelocity.v1, controller.GetPose().vVelocity.v2);
  }
}
