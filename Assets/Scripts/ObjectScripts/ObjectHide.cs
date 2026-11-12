using UnityEngine;
using Unity.Cinemachine;
//Objects that player can interact and hide with
//Melissa coded

public class ObjectHide : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private CinemachineCamera hideCam;

    //to make the player not visible when in first person view
    private Renderer[] playerRender;

    private void Awake(){
        if(player == null){
            player = FindAnyObjectByType<PlayerMovement>();
        }

        playerRender = player.GetComponentsInChildren<Renderer>();
    }

    //when player presses e
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
        //print("is hiding");
        player.setHiding(true);
        SetVisible(false);
        hideCam.Priority = 10;
    }

    private void PlayerOpen(){
        //print("no longer hiding");
        player.setHiding(false);
        SetVisible(true);
        hideCam.Priority = 0;
    }

    //set the visibility of each child of player, so it still exists just not visible
    private void SetVisible(bool status){
        foreach (Renderer r in playerRender){
            r.enabled = status;
        }
    }
}
