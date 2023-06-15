using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator;
    private bool isDoor = false;
    public GameObject instruction;
    public TextMeshProUGUI instructionTxt;
    //public GameObject DoorScreen, openText, closeText;
    //public AudioClip openAudio, closeAudio;
    //public AudioSource door;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (!animator.GetBool("isOpen"))
        //{
        //    openText.SetActive(true);
        //    closeText.SetActive(false);
        //}
        //else
        //{
        //    closeText.SetActive(true);
        //    openText.SetActive(false);
        //}


        if (Input.GetKeyDown(KeyCode.B) && isDoor && FloorDetectScript.canOpenDoor == true)
        {
            if (!animator.GetBool("isOpen"))
            {
                door.PlayOneShot(openAudio);
                animator.SetBool("isOpen", true);
            }
            else
            {
                door.PlayOneShot(closeAudio);
                animator.SetBool("isOpen", false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && FloorDetectScript.canOpenDoor == true)
        {
            //DoorScreen.SetActive(true);
            instruction.SetActive(true);
            instructionTxt.text = "Press B to Open";
            isDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //DoorScreen.SetActive(false);
        //isDoor = false;
        //if (animator.GetBool("isOpen"))
        //{
        //    door.PlayOneShot(closeAudio);
        animator.SetBool("isOpen", false);
        //}
    }
}
