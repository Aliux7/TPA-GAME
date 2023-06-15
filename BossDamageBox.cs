using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDamageBox : MonoBehaviour
{
    // Start is called before the first frame update
    LoadCharacter instance = LoadCharacter.getInstance();
    public Slider health;
    private int ctr = 0;
    public BoxCollider box;
    private void Update()
    {
        if (!BossScript.isAttack)
        {
            ctr = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (BossScript.isAttack)
        {
            ctr++;

            if (other.gameObject.tag == "Player" && ctr == 1)
            {
                instance.setCurrHealth(instance.getCurrHealth() - 20);
                health.value = instance.getCurrHealth();
                box.enabled = false;
                Debug.Log(instance.getCurrHealth());
            }
        }
    }
}
