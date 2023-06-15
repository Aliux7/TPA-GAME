using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextRevealController : MonoBehaviour
{
    public Animator animator;
    public GameObject Annoucer;
    public TextMeshProUGUI AnnoucerTxt;
    public static int indexTxt = 0;
    public static bool isActive = false;
 

    private void Update()
    {
        if (indexTxt == 0)
        {
            AnnoucerTxt.SetText("Cheat Active");
        }
        else
        {
            AnnoucerTxt.SetText("Mission Complete");
        }
        if (isActive == true)
        {
            reveal();
        }
    }

    public void reveal()
    {
        print("DI REVEAL");
        animator.SetBool("isOpen", true);
        Invoke("unreveal", 3f);
    }

    public void unreveal()
    {
        print("DI UNREVEAL");
        animator.SetBool("isOpen", false);
        isActive = false;
    }
}
