using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerBoxEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    LoadCharacter instance = LoadCharacter.getInstance();
    public Slider health;
    private int ctr = 0;


    private void Update()
    {
        if (!EnemyController.isAttack)
        {
            ctr = 0;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (EnemyController.isAttack)
        {
            ctr++;

            if (other.gameObject.tag == "Player" && ctr == 1)
            {
                
                instance.setCurrHealth(instance.getCurrHealth() - 10);
                health.value = instance.getCurrHealth();
                Debug.Log(instance.getCurrHealth());
            }
        }
    }

}