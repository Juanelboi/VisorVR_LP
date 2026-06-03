using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TeleportationActivator : MonoBehaviour
{
    public XRRayInteractor teleportInteractor;
    public InputActionProperty teleportActivationAction;
    public XRRayInteractor rayInteractor;
    void Start()
    {
        teleportInteractor.gameObject.SetActive(false);

        teleportActivationAction.action.performed += OnTeleportActivation;
        rayInteractor.uiHoverEntered.AddListener(x => DisableTeleport());
    }

    private void OnTeleportActivation(InputAction.CallbackContext context)
    {
        if(rayInteractor && rayInteractor.IsOverUIGameObject())
        {
            return;
        }
        teleportInteractor.gameObject.SetActive(true);
    }

    public void DisableTeleport()
    {
        teleportInteractor.gameObject.SetActive(false);
    }

    void Update()
    {
        if(teleportActivationAction.action.WasReleasedThisFrame())
        {
            teleportInteractor.gameObject.SetActive(false);
        }
    }
}
