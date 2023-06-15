using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NpcMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    public float idleTime = 2f;
    public float walkRadius;
    public Animator animator;
    public GameObject paladin, wizard;

    private GameObject select;

    private LoadCharacter instance = LoadCharacter.getInstance();
    private enum NpcState { Idle, Moving, Interact };
    private static NpcState currentState = NpcState.Idle;

    int letterPerSeconds = 20;
    public TextMeshProUGUI dialog;
    string[] text = { "Oh hi there ! Must be a new heroes here right?",
        "Please show me your basic skills 10 times!",
        "Great job ! I can see your basic skills. Now, show me your advance skills!",
        "Your skills are really beautiful!, Now meet all of my friends here and talk to them!",
        "Awesome, now Go to the maze and kill all the monster! Good luck my friends!"};

    private bool isWalking = false;
    private float random;


    void Start()
    {

        if (instance.getcharNum() == 0)
        {
            select = paladin;
        }
        else
        {
            select = wizard;
        }
        //agent.speed = speed;
        //agent.SetDestination(RandomMeshLocation());
        //SetDestination();
        animator.SetBool("isWalking", false);
        random = Random.Range(20, 61);
    }

    // Update is called once per frame
    public GameObject interactiveText;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactiveText.SetActive(true);
            StateInteract();
            if (Input.GetKeyDown(KeyCode.C))
            {
                animator.SetBool("isInteraction", true);
                StartCoroutine(dialogTyping(text[MissionScript.missionLyraCount]));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactiveText.SetActive(false);
        StateIdle();
    }
    void Update()
    {
        switch (currentState)
        {
            case NpcState.Idle:
                animator.SetBool("isWalking", false);
                Invoke("StateMoving", 5f);
                break;

            case NpcState.Moving:
                animator.SetBool("isWalking", true);
                SetDestination();
                Invoke("DestArrive", 5f);
                break;

            case NpcState.Interact:
                //currentState = NpcState.Idle;   
                agent.SetDestination(transform.position);
                animator.SetBool("isWalking", false);
                var lookPos = select.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
                break;
        }
    }

    public void SetDestination(Vector3? target = null)
    {
        if (target == null)
        {
            Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
            NavMesh.SamplePosition(randomPosition + transform.position, out NavMeshHit hit, walkRadius, 1);
            agent.SetDestination(hit.position);
            currentState = NpcState.Moving;
        }
    }

    public Vector3 RandomMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;

        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void DestArrive()
    {
        animator.SetBool("isWalking", false);
        StateIdle();
    }

    public void StateInteract()
    {
        currentState = NpcState.Interact;
    }

    public void StateIdle()
    {
        currentState = NpcState.Idle;
    }

    public void StateMoving()
    {
        currentState = NpcState.Moving;
    }

    public IEnumerator dialogTyping(string dialogContext)
    {
        dialog.text = "";
        foreach(var letter in dialogContext.ToCharArray())
        {
            dialog.text += letter;
            yield return new WaitForSeconds(1f / letterPerSeconds);
        }
        dialog.text = "";
        animator.SetBool("isInteraction", false);
    }

}
