using UnityEngine;

public class ObjectHide : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerMovement player;

    public void Interact(){
        if(!CanInteract()) return;
        if(!player.getHiding()){
            PlayerHide();
        } else {
            PlayerOpen();
        }
    }

    public bool CanInteract(){
        return true;
    }

    private void PlayerHide(){
        print("is hiding");
        player.setHiding(true);
    }

    private void PlayerOpen(){
        print("no longer hiding");
        player.setHiding(false);
    }
}
