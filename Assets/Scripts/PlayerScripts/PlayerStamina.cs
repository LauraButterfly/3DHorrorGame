using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//stamina bar
//melissa code


public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;
    public float useRate = 20f;
    public float regenRate = 15f;
    public float jump = 20f;
    public float regenWait = 1.5f;


    private bool hasStamina = true;


    public Slider staminaSlider;
    [SerializeField] private PlayerMovement player;


    private Coroutine regenCoroutine;


    void Start(){
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
    }


    void Update(){
        if(player.getSprinting() && hasStamina){
            Sprint();
        } else {
            if(currentStamina < maxStamina && regenCoroutine == null){
                regenCoroutine = StartCoroutine(RegenStamina());
            }
        }


        if(currentStamina > 0){
            hasStamina = true;
        }
    }


    //adjust the slider when sprinting
    private void Sprint(){
        //stop regen
        if(regenCoroutine != null){
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }


        currentStamina -= useRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaSlider.value = currentStamina;


        if(currentStamina <= 0){
            hasStamina = false;
        }
    }


    //jumping instant uses stamina
    public void Jump(){
        if(regenCoroutine != null){
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }


        currentStamina -= jump;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaSlider.value = currentStamina;


        if(currentStamina <= 0){
            hasStamina = false;
        }
    }


    public bool getStamina(){
        return hasStamina;
    }


    //coroutine for stamina regeneration
    private IEnumerator RegenStamina(){
        yield return new WaitForSeconds(regenWait);


        while(currentStamina < maxStamina){
            currentStamina += regenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            staminaSlider.value = currentStamina;


            yield return null;
        }
        regenCoroutine = null;
    }


}

