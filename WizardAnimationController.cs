using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WizardAnimationController : MonoBehaviour
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
    public Image crosshair;
    bool alreadyPickMeat = false;
    bool alreadyPickPotion = false;

    float lastDodge;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;

    float attackSpeed = 1f;

    public CinemachineVirtualCamera aimCam;
    public GameObject fireSkill, particle, smallExplode;

    public GameObject bullet;
    public Transform firePos;
    public Camera mCam;
    private Vector3 dest;

    private bool doneSkill1 = false;
    private bool doneSkill2 = false;
    private bool nearPortal = false;

    //Cooldown
    [SerializeField]
    public Image imageCoolDown1;
    public Image imageCoolDown2;
    [SerializeField]
    public TMP_Text textCoolDown1;
    public TMP_Text textCoolDown2;

    private bool isCoolDown1 = false;
    private bool isCoolDown2 = false;
    private float coolDownTime1 = 10.0f;
    private float coolDownTime2 = 5.0f;
    private float coolDownTimer1 = 0.0f;
    private float coolDownTimer2 = 0.0f;

    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject playerStats;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    public AudioSource landSound, flySound, shootSound, stepSound, flameSound;

    public TwoBoneIKConstraint leftArmRig, rightArmRig;
    public MultiAimConstraint headRig;
    public static bool isAttack = false;
    int startFly = 0;

    Quaternion rotation;
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
        leftArmRig.weight = 0;
        rightArmRig.weight = 0;
        headRig.weight = 0;
    }

    void Update()
    {

        float yPosCam = Camera.main.transform.position.y;
        if (yPosCam > 27.5)
        {
            yPosCam = 27.5f;
        }
        headRig.weight = ((27.5f - yPosCam) / 2);
        if(headRig.weight > 0.7)
        {
            headRig.weight = 0.7f;
        }
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
                if (Input.GetKey(KeyCode.LeftShift) && StaminaSlider.value != 0)
                {
                    StaminaSlider.value = StaminaSlider.value - 0.2f;
                    _animator.SetBool("isRunning", true);
                    _animator.SetBool("isMoving", false);
                    transform.position += movement * 0.2f;
                    //if (SceneManager.GetActiveScene().buildIndex == 1)
                    //{
                    //}
                    //else
                    //{
                    //    transform.position += movement * 0.05f;
                    //}
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
                _animator.SetBool("isRunning", false);
                _animator.SetBool("isMoving", false);
                StaminaSlider.value = StaminaSlider.value + 0.2f;
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
            if (Input.GetKey(KeyCode.Mouse1))
            {

                leftArmRig.weight = (27.5f - yPosCam);
                rightArmRig.weight = (27.5f - yPosCam);
                
                crosshair.gameObject.SetActive(true);
                aimCam.gameObject.SetActive(true);
                aimCam.Priority = 14;
                _animator.SetBool("isAiming", true);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    isAttack = true;
                    Invoke("AttackFalse", 1f);
                    shootSound.Play();
                    smallExplode.SetActive(true);
                    if (MissionScript.missionLyraCount == 1 && MissionScript.missionProgress[1] < 10)
                    {
                        MissionScript.missionProgress[1]++;
                    }
                    Ray ray = mCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                    //Debug.Log(ray);
                    RaycastHit hit;
                    //if(Physics.Raycast(firePos.position, firePos.forward, out hit))
                    if (Physics.Raycast(ray, out hit))
                    {
                        dest = hit.point;
                        EnemyController enemy = hit.transform.GetComponent<EnemyController>();
                        if (enemy != null)
                        {
                            enemy.health.value = enemy.health.value - 34;
                        }
                    }
                    else
                    {
                        dest = ray.GetPoint(1000);
                    }

                    //Debug.Log("oit" + ray + " des" + dest);

                    var projectileObj = Instantiate(bullet, firePos.position, Quaternion.identity) as GameObject;
                    projectileObj.SetActive(true);

                    projectileObj.GetComponent<Rigidbody>().velocity = (dest - firePos.position).normalized * 20;
                    Invoke("smallExp", 0.5f);
                }

            }
            else
            {
                leftArmRig.weight = 0;
                rightArmRig.weight = 0;
                crosshair.gameObject.SetActive(false);
                aimCam.gameObject.SetActive(false);
                smallExplode.SetActive(false);
                aimCam.Priority = 9;
                _animator.SetBool("isAiming", false);
            }

            //Skill 1
            if (Input.GetKeyDown(KeyCode.R) && isCoolDown1 == false)
            {
                if (MissionScript.missionLyraCount == 2 && MissionScript.missionProgress[2] < 2 && doneSkill1 == false)
                {
                    MissionScript.missionProgress[2]++;
                    doneSkill1 = true;
                }
                _animator.SetBool("isSkill2", true);
                flySound.Play();
                Invoke("FlyingFalse", 10f);
                //lastFlyTime = Time.time;
                //skill1.SetActive(false);
                //skill1Cooldown.SetActive(true);
                UseSpell1();
            }
            else
            {

            }
            if (isCoolDown1)
            {
                ApplyCoolDown1();
            }

            if (_animator.GetBool("isSkill2"))
            {
                var pos = transform.position;
                if(SceneManager.GetActiveScene().buildIndex == 1)
                {
                    pos.y = 35f;
                    transform.position = pos;
                    transform.position += transform.forward * 0.5f;
                }
                else
                {
                    pos.y = 3f;
                    transform.position = pos;
                    transform.position += transform.forward * 0.3f;
                }
                if (Input.GetKey(KeyCode.E))
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -45f);
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 45f);
                }
                else
                {
                    rotation = transform.rotation;
                }

            }

            //skill 2
            if (Input.GetKeyDown(KeyCode.F) && isCoolDown2 == false)
            {
                if (MissionScript.missionLyraCount == 2 && MissionScript.missionProgress[2] < 2 && doneSkill2 == false)
                {
                    MissionScript.missionProgress[2]++;
                    doneSkill2 = true;
                }
                _animator.SetBool("isSkill1", true);
                fireSkill.SetActive(false);
                flameSound.Play();
                UseSpell2();
                StartCoroutine(Cast());
            }
            if (isCoolDown2)
            {
                ApplyCoolDown2();
            }

            if(nearPortal == true && Input.GetKey(KeyCode.J))
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

    void landSoundPlay()
    {
        landSound.Play();
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

        _animator.SetBool("isSkill2", false);
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

            
            if (grandparent.ToString() == "EnemyRoom(Clone) (UnityEngine.GameObject)")
            {
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
            }else if(grandparent.ToString() == "BossRoom(Clone) (UnityEngine.GameObject)")
            {
                var vampire1 = grandparent.transform.Find("Warrok W Kurniawan").gameObject;
                var vampire1Canvas = vampire1.transform.Find("Canvas").gameObject;
                var vampire1Slider = vampire1Canvas.transform.Find("Slider").gameObject;
                Slider vampire1SliderValue = vampire1Slider.GetComponent<Slider>();

                if (vampire1SliderValue.value > 0)
                {
                    FloorDetectScript.canOpenDoor = false;
                }
                else
                {
                    FloorDetectScript.canOpenDoor = true;
                }
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
            if(MissionScript.missionLyraCount == 5)
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

    void smallExp()
    {
        smallExplode.SetActive(false);
    }
    void fullStamina()
    {
        StaminaSlider.value = 100f;
    }

    public void FlyingFalse()
    {
        _animator.SetBool("isSkill2", false);
        startFly = 0;
        //land.Play();
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

    private void AttackFalse()
    {
        isAttack = false;
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
        float distance = 1f;

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

    IEnumerator Cast()
    {
        yield return new WaitForSeconds(1.2f);
        fireSkill.SetActive(true);
        particle.SetActive(true);
        yield return new WaitForSeconds(3f);
        _animator.SetBool("isSkill1", false);
        yield return new WaitForSeconds(0.3f);
        fireSkill.SetActive(false);
        particle.SetActive(false);
        //lastFireTime = Time.time;
        //yield return new WaitForSeconds(5f);
        //skill2.SetActive(true);
        //skill2Cooldown.SetActive(false);
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
