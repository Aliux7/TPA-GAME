using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    public float walkRadius;
    public Animator animator;
    public BoxCollider leftDmgBox,rightDmgBox,hipDmgBox;
    public GameObject paladin, wizard;
    public GameObject winScreen;

    private GameObject active;

    private LoadCharacter instance = LoadCharacter.getInstance();
    public enum EnemyState { Idle, Chase, Attacking, Die };
    private EnemyState currentState = EnemyState.Idle;
    public static bool isAttack = false;
    public Slider health;
    public GameObject food;

    private System.Random rand = new System.Random();
    public GameObject fireSkill, particle;

    private string str = null;

    private void Start()
    {
        print("TESTINGGGGGGGGGGGGGGGG");
        isAttack = false;
        currentState = EnemyState.Idle;
        leftDmgBox.enabled = false;
        rightDmgBox.enabled = false;
        hipDmgBox.enabled = false;

        //paladin.SetActive(false);
        //wizard.SetActive(false);
        if (instance.getcharNum() == 0)
        {
            active = paladin;
        }
        else
        {
            active = wizard;
        }
    }

    private void Update()
    {
        //currentState = EnemyState.Attacking;
        //leftDmgBox.enabled = false;
        //rightDmgBox.enabled = false;
        //hipDmgBox.enabled = false;
        print(currentState);
        var distance = Vector3.Distance(transform.position, active.transform.position);
        if (health.value <= 0)
        {
            currentState = EnemyState.Die;
            winScreen.SetActive(true);
            //food.SetActive(true);

        }
        switch (currentState)
        {
            case EnemyState.Die:
                animator.SetBool("isWalking", false);
                animator.SetBool("isDeath", true);
                break;
            case EnemyState.Idle:
                animator.SetBool("isWalking", false);
                if (str != null)
                    animator.SetBool(str, false);
                if (health.value <= 0)
                {
                    currentState = EnemyState.Die;
                }
                break;
            case EnemyState.Chase:
                animator.SetBool("isWalking", true);
                agent.SetDestination(active.transform.position);
                distance = Vector3.Distance(transform.position, active.transform.position);
                if (distance <= 2.8f && animator.GetBool("isDeath") == false)
                {
                    animator.SetBool("isWalking", false);
                    Debug.Log("INI DISTANCE" + distance);

                    currentState = EnemyState.Attacking;
                    //Debug.Log("tot"+ currentState);
                }
                break;
            case EnemyState.Attacking:
                if (!isAttack)
                {
                    isAttack = true;
                    var x = rand.Next(4) + 1;
                    //var x = 3;
                    str = "Attack" + x;
                    if(x == 1)
                    {
                        leftDmgBox.enabled = false;
                        rightDmgBox.enabled = false;
                        hipDmgBox.enabled = true;
                    }else if(x == 2)
                    {
                        leftDmgBox.enabled = false;
                        rightDmgBox.enabled = true;
                        hipDmgBox.enabled = false;
                    }else if(x == 3)
                    {
                        leftDmgBox.enabled = false;
                        rightDmgBox.enabled = false;
                        hipDmgBox.enabled = false;
                        //fireSkill.SetActive(false);
                        //flameSound.Play();
                        //UseSpell2();
                        StartCoroutine(Cast());
                    }
                    else if(x == 4)
                    {
                        leftDmgBox.enabled = true;
                        rightDmgBox.enabled = false;
                        hipDmgBox.enabled = false;
                    }
                    animator.SetBool(str, true);
                    Invoke("AttackFalse", 1f);
                }
                break;
        }
        if (distance <= 2.8f && animator.GetBool("isDeath") == true)
        {
            print("TEST");
            if (Input.GetKeyDown(KeyCode.C))
            {
                Destroy(food);
            }
        }
    }

    private void AttackFalse()
    {
        animator.SetBool(str, false);
        isAttack = false;
    }

    private void OnTriggerStay(Collider other)
    {
        var distance = Vector3.Distance(transform.position, active.transform.position);
        if (other.gameObject.tag == "Player" && instance.getCurrHealth() > 0 && distance > 2.8f)
        {
            currentState = EnemyState.Chase;
            agent.isStopped = false;
        }
        if (instance.getCurrHealth() <= 0)
        {
            currentState = EnemyState.Idle;
            agent.isStopped = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentState = EnemyState.Idle;
            agent.isStopped = true;
        }
    }

    IEnumerator Cast()
    {
        yield return new WaitForSeconds(2.5f);
        fireSkill.SetActive(true);
        particle.SetActive(true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("Attack3", false);
        yield return new WaitForSeconds(0.3f);
        fireSkill.SetActive(false);
        particle.SetActive(false);
        //lastFireTime = Time.time;
        //yield return new WaitForSeconds(5f);
        //skill2.SetActive(true);
        //skill2Cooldown.SetActive(false);
    }
}
