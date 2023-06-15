using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoverScript : MonoBehaviour
{
    private LoadCharacter instance = LoadCharacter.getInstance();
    public GameObject selected, paladin, wizard;
    public GameObject text;
    public GameObject particle;
    private Animator animator;
    private AudioSource audioSource;
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;
    // Start is called before the first frame update
    public Animator mainCamera;
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        text.SetActive(false);
        particle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        
    }
    private void OnMouseEnter()
    {

        animator.SetTrigger("HoverTrigger");
        audioSource.Play();
        text.SetActive(true);
        particle.SetActive(true);

    }

    public void OnMouseExit()
    {
        animator.SetTrigger("IdleTrigger");
        audioSource.Stop();
        text.SetActive(false);
        particle.SetActive(false);
    }

    IEnumerator LoadLevelASync(int levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }

    public void OnMouseUp()
    {
        if(selected == paladin)
        {
            instance.setcharNum(0);
        }
        else
        {
            instance.setcharNum(1);
        }

        //mainCamera.speed = 0;
        //mainCamera.Play(0);

        //mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(1));
    }
}
