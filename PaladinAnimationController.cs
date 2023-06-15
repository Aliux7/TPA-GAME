using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PaladinAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    public Transform cam;
    private Rigidbody playerRb;
    public Slider StaminaSlider;
    public Slider HealthSlider;

    public static Item meat = new Item();
    public static Item potion = new Item();

    bool pickMeat = false;
    bool pickPotion = false;

    public Image meatImg1, meatImg2;
    public Image potionImg1, potionImg2;
    public TextMeshProUGUI amount, name;
    
    public GameObject instructions;
    public TextMeshProUGUI instructionsText;

    public GameObject meatObject, potionObject;
    bool alreadyPickMeat = false;
    bool alreadyPickPotion = false;

    float lastDodge;

    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    float attackSpeed = 1f;

    float lastRageTime = 0;

    private bool doneSkill1 = false;
    private bool doneSkill2 = false;
    private bool nearPortal = false;


    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject playerStats;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;
    public static bool isAttack = false;

    public AudioSource landSound, rollSound, rageSound, stepSound, swordSound;

    //Cooldown
    [SerializeField]
    public Image imageCoolDown1;
    public Image imageCoolDown2;
    [SerializeField]
    public TMP_Text textCoolDown1;
    public TMP_Text textCoolDown2;

    private bool isCoolDown1 = false;
    private bool isCoolDown2 = false;
    private float coolDownTime1 = 5.0f;
    private float coolDownTime2 = 3.0f;
    private float coolDownTimer1 = 0.0f;
    private float coolDownTimer2 = 0.0f;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        StaminaSlider.value = 100;
        HealthSlider.value = 100;

        meatImg1.enabled = true;
        meatImg2.enabled = false;
        potionImg1.enabled = true;
        potionImg2.enabled = false;
        meat.setName("Meat");
        potion.setName("Potion");

        instructions.SetActive(false);
        doneSkill1 = false;
        doneSkill2 = false;

        textCoolDown1.gameObject.SetActive(false);
        imageCoolDown1.fillAmount = 0.0f;
        textCoolDown2.gameObject.SetActive(false);
        imageCoolDown2.fillAmount = 0.0f;

    }

    void Update()
    {
        if (!_animator.GetBool("isDeath"))
        {
            if (LoadCharacter.getInstance().getCurrHealth() <= 0 && !_animator.GetBool("isDeath"))
            {
                _animator.SetBool("isDeath", true);
            }
            //Movement
            float verticalAxis = Input.GetAxis("Vertical");
            float horizontalAxis = Input.GetAxis("Horizontal");

            Vector3 movement = verticalAxis * cam.forward + horizontalAxis * cam.right;

            if (movement.magnitude != 0)
            {
                //Sprint
                if (Input.GetKey(KeyCode.LeftShift) && StaminaSlider.value >= 1)
                {
                    StaminaSlider.value = StaminaSlider.value - 0.2f;
                    _animator.SetBool("isRunning", true);
                    _animator.SetBool("isMoving", false);
                    transform.position += movement * 0.1f;
                }

                //Walk
                else
                {
                    StaminaSlider.value = StaminaSlider.value + 0.2f;
                    _animator.SetBool("isRunning", false);
                    _animator.SetBool("isMoving", true);
                    transform.position += movement * ThirdPersonMovement.movementSpeed;
                }
            }
            else
            {
                StaminaSlider.value = StaminaSlider.value + 0.2f;
                _animator.SetBool("isRunning", false);
                _animator.SetBool("isMoving", false);
            }
            _animator.SetFloat("Vertical", verticalAxis);
            _animator.SetFloat("Horizontal", horizontalAxis);

            //Jump
            float jumpForce = 5f;
            if (Input.GetKeyDown(KeyCode.Space) && !_animator.GetBool("isJump"))
            {
                _animator.SetBool("isJump", true);
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                Invoke("landSoundPlay", 0.9f);
            }

            //Dodge
            if (Input.GetKeyDown(KeyCode.V))
            {
                _animator.SetBool("isDodge", true);
                StartCoroutine(Dodge());
            }
            else
            {
                //transform.position += movement * 0.01f;
                _animator.SetBool("isDodge", false);
            }

            //Invetory
            if (meatImg1.enabled == true)
            {
                amount.text = meat.getAmount().ToString();
                name.text = meat.getName().ToString();
            }
            else if (potionImg2.enabled == true)
            {
                amount.text = potion.getAmount().ToString();
                name.text = potion.getName().ToString();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                meatImg1.enabled = !meatImg1.enabled;
                meatImg2.enabled = !meatImg2.enabled;
                potionImg1.enabled = !potionImg1.enabled;
                potionImg2.enabled = !potionImg2.enabled;

                if (meatImg1.enabled == true)
                {
                    amount.text = meat.getAmount().ToString();
                    name.text = meat.getName().ToString();
                }
                else if (potionImg2.enabled == true)
                {
                    amount.text = potion.getAmount().ToString();
                    name.text = potion.getName().ToString();
                }

            }

            //Action Inventory
            if (Input.GetKeyDown(KeyCode.C) && pickMeat && !alreadyPickMeat)
            {
                _animator.SetBool("isLooting", true);
                meat.setAmount(meat.getAmount() + 1);
                meatObject.SetActive(false);
                alreadyPickMeat = true;
                instructions.SetActive(false);

            }
            else if (Input.GetKeyDown(KeyCode.C) && pickPotion && !alreadyPickPotion)
            {
                _animator.SetBool("isLooting", true);
                potion.setAmount(potion.getAmount() + 1);
                potionObject.SetActive(false);
                alreadyPickPotion = true;
                instructions.SetActive(false);
            }
            else
            {
                _animator.SetBool("isLooting", false);
            }

            //Use Item
            if (Input.GetKeyDown(KeyCode.G) && meatImg1.enabled == true && meat.getAmount() > 0)
            {
                _animator.SetBool("isItem", true);
                meat.setAmount(meat.getAmount() - 1);
            }
            else if (Input.GetKeyDown(KeyCode.G) && potionImg2.enabled == true && potion.getAmount() > 0)
            {
                _animator.SetBool("isItem", true);
                potion.setAmount(potion.getAmount() - 1);
                Invoke("fullStamina", 2f);
            }
            else
            {
                _animator.SetBool("isItem", false);
            }

            //Basic Attack
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Hit1"))
            {
                _animator.SetBool("hit1", false);
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Hit2"))
            {
                _animator.SetBool("hit2", false);
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > attackSpeed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Hit3"))
            {
                _animator.SetBool("hit3", false);
                noOfClicks = 0;
            }
            if (Time.time - lastClickedTime > maxComboDelay)
            {
                noOfClicks = 0;
            }
            if (Time.time > nextFireTime)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isAttack = true;
                    Invoke("AttackFalse", 1f);
                    if (MissionScript.missionLyraCount == 1 && MissionScript.missionProgress[1] < 10)
                    {
                        MissionScript.missionProgress[1]++;
                    }
                    OnClick();
                }
            }

            //Skill 1
            if (Input.GetKeyDown(KeyCode.R) && isCoolDown1 == false)
            {
                _animator.SetBool("isRage", true);
                ThirdPersonMovement.movementSpeed = 0.05f;
                attackSpeed = 0.4f;

                lastRageTime = Time.time;
                print(Time.time);
                if (MissionScript.missionLyraCount == 2 && MissionScript.missionProgress[2] < 2 && doneSkill1 == false)
                {
                    MissionScript.missionProgress[2]++;
                    doneSkill1 = true;
                }
                rageSound.Play();
                UseSpell1();
            }
            else
            {
                _animator.SetBool("isRage", false);
                if (Time.time - lastRageTime > 7f)
                {

                    ThirdPersonMovement.movementSpeed = 0.02f;
                    attackSpeed = 1f;
                }
            }
            if (isCoolDown1)
            {
                ApplyCoolDown1();
            }

            //skill 2
            if (Input.GetKeyDown(KeyCode.F) && isCoolDown2 == false)
            {
                if (MissionScript.missionLyraCount == 2 && MissionScript.missionProgress[2] < 2 && doneSkill2 == false)
                {
                    MissionScript.missionProgress[2]++;
                    doneSkill2 = true;
                }
                _animator.SetBool("isRoll", true);
                rollSound.Play();
                UseSpell2();
                StartCoroutine(Roll());
            }
            else
            {
                _animator.SetBool("isRoll", false);

            }
            if (isCoolDown2)
            {
                ApplyCoolDown2();
            }

            if (nearPortal == true && Input.GetKey(KeyCode.J))
            {
                playerStats.SetActive(false);
                loadingScreen.SetActive(true);

                StartCoroutine(LoadLevelASync(2));
            }
        }
    }
    
    private void Step()
    {
        stepSound.Play();
    }

    private void Blade()
    {
        swordSound.Play();
    }

    void landSoundPlay()
    {
        landSound.Play();
    }
    private void AttackFalse()
    {
        isAttack = false;
    }
    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if(noOfClicks == 1)
        {
            _animator.SetBool("hit1", true);
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        }else if(noOfClicks >= 2 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _animator.GetCurrentAnimatorStateInfo(0).IsName("Hit1"))
        {
            _animator.SetBool("hit1", false);
            _animator.SetBool("hit2", true);
        }else if (noOfClicks >= 3 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _animator.GetCurrentAnimatorStateInfo(0).IsName("Hit2"))
        {
            _animator.SetBool("hit2", false);
            _animator.SetBool("hit3", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    Destroy(collision.gameObject);
        //}

        if (collision.gameObject.CompareTag("Ground"))
        {
            _animator.SetBool("isJump", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "RoomFloor")
        {
            var parent = collision.gameObject.transform.parent.gameObject;
            var grandparent = parent.gameObject.transform.parent.gameObject;
            var minimap = grandparent.transform.Find("MinimapTiles").gameObject;
            minimap.SetActive(true);

            var enemy = grandparent.transform.Find("Enemy").gameObject;

            var vampire1 = enemy.transform.Find("Vampire A Lusth").gameObject;
            var vampire1Canvas = vampire1.transform.Find("Canvas").gameObject;
            var vampire1Slider = vampire1Canvas.transform.Find("Slider").gameObject;
            Slider vampire1SliderValue = vampire1Slider.GetComponent<Slider>();

            var vampire2 = enemy.transform.Find("Vampire A Lusth (1)").gameObject;
            var vampire2Canvas = vampire2.transform.Find("Canvas").gameObject;
            var vampire2Slider = vampire2Canvas.transform.Find("Slider").gameObject;
            Slider vampire2SliderValue = vampire2Slider.GetComponent<Slider>();

            if (vampire1SliderValue.value > 0 || vampire2SliderValue.value > 0)
            {
                FloorDetectScript.canOpenDoor = false;
            }
            else
            {
                FloorDetectScript.canOpenDoor = true;
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meat"))
        {
            pickMeat = true;
            pickPotion = false;
            instructions.SetActive(true);
            instructionsText.text = "Press [C] To Take A Meat";
        }
        else if (other.CompareTag("Potion"))
        {
            pickPotion = true;
            pickMeat = false;
            instructions.SetActive(true);
            instructionsText.text = "Press [C] To Take A Potion";
        }
        else if (other.CompareTag("Portal"))
        {
            if (MissionScript.missionLyraCount == 5)
            {
                instructions.SetActive(true);
                instructionsText.text = "Press [J] To Use Portal";
                nearPortal = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        instructions.SetActive(false);
        nearPortal = false;
    }

    void fullStamina()
    {
        StaminaSlider.value = 100f;
    }

    void ApplyCoolDown1()
    {
        coolDownTimer1 -= Time.deltaTime;

        if (coolDownTimer1 < 0.0f)
        {
            isCoolDown1 = false;
            textCoolDown1.gameObject.SetActive(false);
            imageCoolDown1.fillAmount = 0.0f;
        }
        else
        {
            textCoolDown1.text = Mathf.RoundToInt(coolDownTimer1).ToString();
            imageCoolDown1.fillAmount = coolDownTimer1 / coolDownTime1;
        }
    }

    void ApplyCoolDown2()
    {
        coolDownTimer2 -= Time.deltaTime;

        if (coolDownTimer2 < 0.0f)
        {
            isCoolDown2 = false;
            textCoolDown2.gameObject.SetActive(false);
            imageCoolDown2.fillAmount = 0.0f;
        }
        else
        {
            textCoolDown2.text = Mathf.RoundToInt(coolDownTimer2).ToString();
            imageCoolDown2.fillAmount = coolDownTimer2 / coolDownTime2;
        }
    }

    public void UseSpell1()
    {
        if (isCoolDown1)
        {

        }
        else
        {
            isCoolDown1 = true;
            textCoolDown1.gameObject.SetActive(true);
            coolDownTimer1 = coolDownTime1;
        }
    }
    public void UseSpell2()
    {
        if (isCoolDown2)
        {

        }
        else
        {
            isCoolDown2 = true;
            textCoolDown2.gameObject.SetActive(true);
            coolDownTimer2 = coolDownTime2;
        }
    }

    IEnumerator Dodge()
    {
        float duration = 0.8f;
        float speed = 2f;
        float distance = 1f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - transform.forward * distance;

        float t = 0f;
        while (t < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t / duration);
            t += Time.deltaTime * speed;
            yield return null;
        }
        transform.position = endPosition;

        yield return new WaitForSeconds(duration / 2f);
        lastDodge = Time.time;
    }

    IEnumerator Roll()
    {
        float duration = 0.8f;
        float speed = 2f;
        float distance = 4f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + transform.forward * distance;

        float t = 0f;
        while (t < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t / duration);
            t += Time.deltaTime * speed;
            yield return null;
        }
        transform.position = endPosition;

        yield return new WaitForSeconds(duration / 2f);
        lastDodge = Time.time;
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
}
