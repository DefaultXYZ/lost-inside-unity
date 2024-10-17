using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private CameraLook cameraLook;

    private InputAction interactAction;

    private void Awake()
    {
        interactAction = InputSystem.actions.FindAction(InputActionNames.Interact);
    }

    private void OnEnable()
    {
        interactAction.performed += InteractActionPerformed;
    }

    private void OnDisable()
    {
        interactAction.performed -= InteractActionPerformed;
    }

    private void InteractActionPerformed(InputAction.CallbackContext obj)
    {
        if (cameraLook.CurrentObservedObject != null &&
            cameraLook.CurrentObservedObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.OnInteract();
        }
    }
}