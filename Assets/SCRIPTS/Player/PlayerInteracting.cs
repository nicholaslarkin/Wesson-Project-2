using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteracting : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    private Collider currentInteractable;

    // Called when the player presses the interact button
    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && currentInteractable != null)
        {
            Interact();
        }
    }

    // Handle the interaction
    void Interact()
    {
        Debug.Log("Interacted with: " + currentInteractable.gameObject.name);
        // Add interaction logic here (e.g., call a method on the interactable object)
    }

    // Detect when entering a trigger with the specified LayerMask
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
        {
            currentInteractable = other;
            Debug.Log("Can interact with: " + other.gameObject.name);
        }
    }

    // Clear the interactable when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (currentInteractable == other)
        {
            Debug.Log("Exited interaction zone: " + other.gameObject.name);
            currentInteractable = null;
        }
    }
}
