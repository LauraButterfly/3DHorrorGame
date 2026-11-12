using UnityEngine;
using UnityEngine.InputSystem;


public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableRange = null;
    public GameObject interactionIcon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionIcon.SetActive(false);
    }


    //when player presses e
    public void OnInteract(InputAction.CallbackContext context){
        if(context.performed){
            interactableRange?.Interact();
        }
    }


    //when player enters the radius of an interactable object
    private void OnTriggerEnter(Collider obj){
        if(obj.TryGetComponent(out IInteractable interactable) && interactable.CanInteract()){
            interactableRange = interactable;
            interactionIcon.SetActive(true);
        }
    }


    //when player leaves radius of interactable object
    private void OnTriggerExit(Collider obj){
        if(obj.TryGetComponent(out IInteractable interactable) && interactable == interactableRange){
            interactableRange = null;
            interactionIcon.SetActive(false);
        }
    }
}



