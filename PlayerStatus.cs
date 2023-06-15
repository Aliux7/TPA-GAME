using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public Slider health;
    public GameObject Stats, Lose;
    LoadCharacter instance = LoadCharacter.getInstance();
    //float healthSliderValue = HealthSlider.value;
    //float staminaSliderValue = StaminaSlider.value;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        health.value = instance.getCurrHealth();
        if(health.value <= 0)
        {
            Invoke("OpenLoseScreen", 1f);
        }
        //Debug.Log("ini player status " + instance.getCurrHealth());
        //health.value = instance.getCurrHealth();
        
    }
    
    void OpenLoseScreen()
    {
        Stats.SetActive(false);
        Lose.SetActive(true);
    }
}

