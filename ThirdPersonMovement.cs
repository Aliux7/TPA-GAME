using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    //[SerializeField]
    //private Animator _animator;
    private LoadCharacter instance = LoadCharacter.getInstance();
    public static float movementSpeed = 0.07f;
    public CinemachineVirtualCamera cam;
    private GameObject active;
    public GameObject paladin, wizard;
    public Slider staminaSlider;

    string[] cheatCode = { "hesoyam", "ihateyou", "iloveyou", "akusayangangkatan221", "budi" };
    string activeCode = "";
    int startCheat = 0;

    void Start()
    {
        paladin.SetActive(false);
        wizard.SetActive(false);
        //Temp instance for(wizard)
        instance.setcharNum(1);
        if (instance.getcharNum() == 0)
        {
            active = paladin;
            //paladin.SetActive(true);
            //wizard.SetActive(false);
        }
        else
        {
            active = wizard;
            //wizard.SetActive(true);
            //paladin.SetActive(false);
        }
        instance.player = active;
        active.SetActive(true);
        //cam.LookAt = active.transform;
        cam.Follow = active.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(startCheat == 0)
            {
                startCheat = 1;
            }
            else
            {
                int foundCheat = -1;
                for(int i = 0; i < 5; i++)
                {
                    if (activeCode.ToLower() == cheatCode[i].ToLower())
                    {
                        foundCheat = i;
                        break;
                    }
                }
                print(foundCheat);
                if (foundCheat != -1)
                {
                    TextRevealController.isActive = true;
                    TextRevealController.indexTxt = 0;
                    print(cheatCode[foundCheat]);
                    switch (foundCheat)
                    {
                        case 0:
                            instance.setCurrHealth(100);
                            staminaSlider.value = 100;
                            break;
                        case 1:
                            movementSpeed += 0.2f;
                            break;
                        case 2:
                            instance.setCurrHealth(0);
                            break;
                        case 3:
                            WinScript.GameWin = true;
                            print(WinScript.GameWin);
                            break;
                        case 4:
                            paladin.SetActive(false);
                            wizard.SetActive(false);
                            if (active == paladin)
                            {
                                active = wizard;
                            }
                            else
                            {
                                active = paladin;
                            }
                            instance.player = active;
                            active.SetActive(true);
                            cam.Follow = active.transform;
                            break;

                    }
                    foundCheat = -1;
                }
                activeCode = "";
                startCheat = 0;
            }
        }

        if (startCheat == 1 && !Input.GetKeyDown(KeyCode.Return))
        {
            activeCode += Input.inputString;
            print(activeCode + " " + activeCode.Length);
        }
    }

}
