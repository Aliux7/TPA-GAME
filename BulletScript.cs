using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletScript : MonoBehaviour
{
    private bool collided = false;
    public GameObject impact;
    public Slider health;
    private int ctr = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (!collided && collision.gameObject.tag != "Player" && collision.gameObject.tag != "Bullet")
        {
            collided = true;

            var obj = Instantiate(impact, collision.contacts[0].point, Quaternion.identity) as GameObject;

            if (WizardAnimationController.isAttack)
            {
                ctr++;

            
                if (collision.gameObject.tag == "Enemy" && ctr == 1)
                {
                    health.value = health.value - 34;
                }

                if (collision.gameObject.tag == "Boss" && ctr == 1)
                {
                    collision.gameObject.GetComponent<BossScript>().health.value -= 10;
                }
            }

            Destroy(gameObject);
            Destroy(obj, 2f);
        }
    }
}
