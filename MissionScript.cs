using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionScript : MonoBehaviour
{
    public TextMeshProUGUI missionText;
    public GameObject lyra, darian, cesiya;
    public static int missionLyraCount = 0;
    public GameObject portalScene;
    public CinemachineVirtualCamera mazeCamera;
    public CinemachineVirtualCamera tpc;
    public TextMeshProUGUI dialog;

    private static string[] missionList = 
      { "Find and talk to Lyra",
        "INSTANTIATE ATTACK",
        "USE YOUR SKILL ABILITY",
        "TALK TO ALL PEOPLE" };
    public static int[] missionProgress = { 0, 0, 0, 0 };
    // Start is called before the first frame update
    private bool interactLyra, interactDarian, interactCesiya;
    private int missionInteractLyra, missionInteractDarian, missionInteractCesiya;
    void Start()
    {
        missionLyraCount = 0;
        missionProgress[0] = 0;
        missionProgress[1] = 0;
        missionProgress[2] = 0;
        missionProgress[3] = 0;
        missionText.SetText("Find and talk to Lyra");
    }

    // Update is called once per frame
    void Update()
    {
        if(missionLyraCount == 0)
        {
            missionText.SetText(missionList[missionLyraCount]);
            if (Input.GetKeyDown(KeyCode.C) && interactLyra == true)
            {
                missionText.color = Color.green;
                TextRevealController.isActive = true;
                TextRevealController.indexTxt = 1;
            }
            if(Input.GetKeyDown(KeyCode.C) && interactLyra == true && missionText.color == Color.green)
            {
                missionLyraCount++;
                missionText.color = Color.white;
            }
        }
        else if (missionLyraCount == 1)
        {
            missionText.SetText(missionList[missionLyraCount] + "(" + missionProgress[missionLyraCount] + "/10)");
            if(missionProgress[1] >= 10)
            {
                missionText.color = Color.green;
                TextRevealController.isActive = true;
                TextRevealController.indexTxt = 1;
            }
            if(missionProgress[1] >= 10 && Input.GetKeyDown(KeyCode.C) && interactLyra == true)
            {
                missionLyraCount++;
                missionText.color = Color.white;
            }
        }
        else if ( missionLyraCount == 2)
        {
            missionText.SetText(missionList[missionLyraCount] + "(" + missionProgress[missionLyraCount] + "/2)");
            if (missionProgress[2] >= 2)
            {
                missionText.color = Color.green;
                TextRevealController.isActive = true;
                TextRevealController.indexTxt = 1;
            }
            if (missionProgress[2] >= 2 && Input.GetKeyDown(KeyCode.C) && interactLyra == true)
            {
                missionLyraCount++;
                missionText.color = Color.white;
            }
        }
        else if (missionLyraCount == 3)
        {
            missionText.SetText(missionList[missionLyraCount] + "(" + missionProgress[missionLyraCount] + "/3)");
            if (missionProgress[3] >= 3)
            {
                missionText.color = Color.green;
                TextRevealController.isActive = true;
                TextRevealController.indexTxt = 1;
            }
            if (missionProgress[3] >= 3 && Input.GetKeyDown(KeyCode.C) && interactLyra == true)
            {
                missionLyraCount++;
                missionText.color = Color.white;
            }
        }else if(missionLyraCount == 4)
        {
            missionLyraCount++;
            portalScene.SetActive(true);
            //mazeCamera.Priority = 20;
            tpc.Priority = 1;
            missionText.SetText("GO TO MAZE");
            Invoke("cutScene", 13f);
        }

        if(missionInteractLyra == 0 && missionLyraCount == 3 && interactLyra == true && Input.GetKeyDown(KeyCode.C))
        {
            missionInteractLyra = 1;
        }
        if (missionInteractDarian == 0 && missionLyraCount == 3 && interactDarian == true && Input.GetKeyDown(KeyCode.C))
        {
            missionInteractDarian = 1;
        }
        if (missionInteractCesiya == 0 && missionLyraCount == 3 && interactCesiya == true && Input.GetKeyDown(KeyCode.C))
        {
            missionInteractCesiya = 1;
        }

        missionProgress[3] = missionInteractLyra + missionInteractDarian + missionInteractCesiya;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == lyra)
        {
            interactLyra = true;
        }
        else if(other.gameObject == darian)
        {
            interactDarian = true;
        }
        else if (other.gameObject == cesiya)
        {
            interactCesiya = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == lyra)
        {
            interactLyra = false;
        }
        else if (other.gameObject == darian)
        {
            interactDarian = false;
        }
        else if (other.gameObject == cesiya)
        {
            interactCesiya = false;
        }
    }

    void cutScene()
    {
        mazeCamera.Priority = 8;
        tpc.Priority = 12;
    }

}
