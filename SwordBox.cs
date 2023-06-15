using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordBox : MonoBehaviour
{
    public Slider health;
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
                health.value = health.value - 34;
            }
        }
    }
}
