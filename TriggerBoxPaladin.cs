using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerBoxPaladin : MonoBehaviour
{

    public Slider enemyHP;
    private int ctr = 0;

    // Update is called once per frame
    void Update()
    {
        if (!PaladinAnimationController.isAttack)
        {
            ctr = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (PaladinAnimationController.isAttack)
        {
            ctr++;

            if (other.gameObject.tag == "Enemy" && ctr == 1)
            {
                enemyHP.value = enemyHP.value - 30;
                Debug.Log(enemyHP.value);
            }
        }
    }
}
