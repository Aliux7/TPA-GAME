using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireScript : MonoBehaviour
{
    private LoadCharacter instance = LoadCharacter.getInstance();
    private bool playerDmg, enemyDmg;
    Collider c;

    private void Start()
    {
        c = null;
        playerDmg = false;
        enemyDmg = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !enemyDmg)
        {
            c = other;
            enemyDmg = true;
            //StartCoroutine(Damage(other));
            Invoke("EnemyDmg", 0.2f);
        }
        else if (other.gameObject.CompareTag("Boss") && !enemyDmg)
        {
            c = other;
            enemyDmg = true;
            Invoke("BossDmg", 0.2f);
        }
        else if (other.gameObject.CompareTag("Player") && !playerDmg)
        {
            playerDmg = true;
            Invoke("PlayerDmg", 0.2f);
            //StartCoroutine(PlayerDamage(other));
        }
    }

    void BossDmg()
    {
        if (enemyDmg && c != null)
        {
            c.gameObject.GetComponent<BossScript>().health.value -= 10;
            enemyDmg = false;
            c = null;
        }
    }

    void EnemyDmg()
    {
        if (enemyDmg && c != null)
        {
            c.gameObject.GetComponent<EnemyController>().health.value -= 10;
            enemyDmg = false;
            c = null;
        }
    }

    void PlayerDmg()
    {
        if (playerDmg)
        {
            instance.setCurrHealth(instance.getCurrHealth() - 5);
            playerDmg = false;
        }
    }

    //IEnumerator PlayerDamage(Collider other)
    //{
    //    while (playerDmg)
    //    {
    //        yield return new WaitForSeconds(0.2f);
    //        instance.SetCurrHp(instance.GetCurrHp() - 5);
    //    }
    //    StopCoroutine(PlayerDamage(other));
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyDmg = false;
            c = null;
            //StopCoroutine(Damage(other));
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            playerDmg = false;
            //StopCoroutine(PlayerDamage(other));
        }
    }

    //private IEnumerator Damage(Collider other)
    //{
    //    while (enemyDmg)
    //    {
    //        Debug.Log("woi");
    //        yield return new WaitForSeconds(0.2f);
    //        other.gameObject.GetComponent<Enemy>().CurrHp -= 10;
    //        Debug.Log("woioasdjas");
    //        StopCoroutine(Damage(other));
    //    }

    //}
}
