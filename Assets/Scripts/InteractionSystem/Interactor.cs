using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;

    [SerializeField] private InteractionPromptUI _interactionPromptUI;

    [Header("VR Input (OpenXR / XR Interaction Toolkit)")]
    [Tooltip("Acción del controlador para interactuar (trigger o grip). " +
             "Asígnala desde un Input Action Reference, p. ej. XRI RightHand Interaction/Activate.")]
    [SerializeField] private InputActionProperty _interactAction;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numfound;

    private IInteractable _interactable;

    private void OnEnable()
    {
        if (_interactAction.action != null) _interactAction.action.Enable();
    }

    private void OnDisable()
    {
        if (_interactAction.action != null) _interactAction.action.Disable();
    }

    private void Update()
    {
        _numfound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numfound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if (_interactable != null)
            {
                if (!_interactionPromptUI.IsActive) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);

                if (InteractPressedThisFrame()) _interactable.Interact(this);
            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
            if (_interactionPromptUI.IsActive) _interactionPromptUI.Close();
        }
    }

    private bool InteractPressedThisFrame()
    {
        return _interactAction.action != null && _interactAction.action.WasPressedThisFrame();
    }

    private void OnDrawGizmos()
    {
        if (_interactionPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }

    public void RefreshInputAction()
    {
        if (_interactAction.action != null) _interactAction.action.Enable();
    }
}