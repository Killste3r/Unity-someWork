using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Controlleris : MonoBehaviour
{
    Rigidbody rb;

    //Movement
    float speed = 2f;
    public float Mspeed = 3f;
    float Dspeed = 9f;
    Vector3 movement;
    Vector3 MoveDir;
    //Inputs
    float FB;
    float LR;
    //Camera & rotation
    public Transform Cam;
    float smoothTime = 0.01f;
    float SmoothVelocity;
    //Stamina/Health
    public Slider SliderSP;
    public Slider SliderHP;
    public float maxStamina = 100;
    public int maxHealt = 100;
    float staminausagespring = 0.1f;
    public int currentHealth;
    public float currentStamina;
    public GameObject HealthBar;
    public GameObject StaminaBar;
    private Coroutine regenerationSP;
    private Coroutine regenerationHP;
    private WaitForSeconds regenSP = new WaitForSeconds(0.01f);
    private WaitForSeconds regenHP = new WaitForSeconds(0.1f);
    //Animator
    private Animator anim;
    //Roll
    public float PushRoll = 10;




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //stamina bar values
        currentStamina = maxStamina;
        SliderSP.maxValue = maxStamina;
        SliderSP.value = maxStamina;
        //Health bar values
        currentHealth = maxHealt;
        SliderHP.maxValue = maxHealt;
        SliderHP.value = maxHealt;
    }
    void Update()
    {
        if (!Quickcommands.GameIsPause)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            FB = Input.GetAxisRaw("Vertical");
            LR = Input.GetAxisRaw("Horizontal");
            movement = new Vector3(LR, 0, FB);
            SliderHealth();
            SliderStamina();
            //for tests only
            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentStamina = maxStamina;
                SliderSP.value = maxStamina;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                anim.SetBool("IsRunning", true);
            }
            else anim.SetBool("IsRunning", false);

            
            if (Input.GetKey(KeyCode.P))
            {
                SceneManager.LoadScene("Demo");
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                anim.SetTrigger("Attack");
            }

            roll();
        }
    
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }
    void FixedUpdate()
    {
        //Movement & Rotation
        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref SmoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            MoveDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            transform.Translate(MoveDir * speed * Time.deltaTime, Space.World);

        }
        //Sprint
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.LeftShift)
            && Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A))
        {
            bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
            if (IsGrounded == true)
            {
                speed = Dspeed;
                if (currentStamina != 0)
                {
                    StaminaUsage(staminausagespring);
                }
            }
        }
        else
            speed = 5;
        if (currentStamina == 0)
        {
            speed = 5;
        }

    }
//roll error recheck full roll code
    public void roll()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (currentStamina >= 25)
            {
                bool isGrounded = Physics.BoxCast(transform.position, Vector3.one / 2, Vector3.down, transform.rotation, PushRoll);
                if (isGrounded == true)
                {
                    rb.velocity = transform.forward * PushRoll;
                    StaminaUsage(25);
                    anim.SetTrigger("IsRoll");
                }
            }
        }
    }
    public void StaminaUsage(float ammout)
    {
        if (currentStamina - ammout >= 0)
        {
            currentStamina -= ammout;
            SliderSP.value = currentStamina;
            if (regenerationSP != null)
            {
                StopCoroutine(regenerationSP);
            }
            regenerationSP = StartCoroutine(regenstamina());
        }
    }
    private IEnumerator regenstamina()
    {
        yield return new WaitForSeconds(1f);
        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            SliderSP.value = currentStamina;
            yield return regenSP;
        }
    }
    public void Healthbar(int ammout)
    {
        if (currentHealth - ammout >= 0)
        {
            currentHealth -= ammout;
            SliderHP.value = currentHealth;
            if (regenerationHP != null)
            {
                StopCoroutine(regenerationHP);
            }
            regenerationHP = StartCoroutine(regenHealth());
        }
    }
    private IEnumerator regenHealth()
    {
        yield return new WaitForSeconds(3f);
        while (currentHealth < maxHealt)
        {
            currentHealth += maxHealt / 100;
            SliderHP.value = currentHealth;
            yield return regenHP;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Healthbar(20);
        }
    }
    public void SliderHealth()
    {
        if (currentHealth == maxHealt)
        {
            HealthBar.SetActive(false);
        }
        else
            HealthBar.SetActive(true);
    }
    public void SliderStamina()
    {


        if (currentStamina == maxStamina)
        {
            StaminaBar.SetActive(false);
        }
        else
            StaminaBar.SetActive(true);
    }


}

