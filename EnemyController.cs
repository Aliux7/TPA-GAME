using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    public float walkRadius;
    public Animator animator;
    public BoxCollider dmgBox;
    public GameObject paladin, wizard;

    private GameObject active;

    private LoadCharacter instance = LoadCharacter.getInstance();
    public enum EnemyState { Idle, Chase, Attacking, Die };
    private EnemyState currentState = EnemyState.Idle;
    public static bool isAttack = false;
    public Slider health;
    public GameObject food;

    private System.Random rand = new System.Random();

    private string str = null;

    private void Start()
    {
        isAttack = false;
        currentState = EnemyState.Idle;
        dmgBox.enabled = false;

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
        var distance = Vector3.Distance(transform.position, active.transform.position);
        dmgBox.enabled = true;
        if (health.value == 0)
        {
            currentState = EnemyState.Die;
            food.SetActive(true);

        }
        switch (currentState)
        {
            case EnemyState.Die:
                animator.SetBool("isDeath", true);
                break;
            case EnemyState.Idle:
                animator.SetBool("isWalking", false);
                if (str != null)
                    animator.SetBool(str, false);
                if(health.value <= 0)
                {
                    currentState = EnemyState.Die;
                }
                break;
            case EnemyState.Chase:
                animator.SetBool("isWalking", true);
                agent.SetDestination(active.transform.position);
                distance = Vector3.Distance(transform.position, active.transform.position);
                if (distance <= 1.8f && animator.GetBool("isDeath") == false)
                {
                    animator.SetBool("isWalking", false);
                    //Debug.Log(distance);

                    currentState = EnemyState.Attacking;
                    //Debug.Log("tot"+ currentState);
                }
                break;
            case EnemyState.Attacking:
                dmgBox.enabled = true;
                if (!isAttack)
                {
                    isAttack = true;
                    var x = rand.Next(3) + 1;
                    str = "Attack" + x;
                    animator.SetBool(str, true);
                    Invoke("AttackFalse", 1f);
                }
                break;
        }
        if(distance <= 1.8f && animator.GetBool("isDeath") == true)
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
        if (other.gameObject.tag == "Player" && instance.getCurrHealth() > 0)
        {
            currentState = EnemyState.Chase;
            agent.isStopped = false;
        }
        if(instance.getCurrHealth() <= 0)
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

}