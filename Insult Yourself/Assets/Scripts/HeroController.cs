using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class HeroController : MonoBehaviour
{
    public GameObject Particle;
    public Transform particlePoint;
    bool isLeftLeg;

    [HideInInspector]
    public GameObject Gun;

    public GameObject ParTrigger;
    public Light2D spotlight;

    public float speed;
    public float dashSpeed;
    public Rigidbody2D rb;
    public Animator animator;
    private bool isDashing = false;
    Vector2 movement;
    Vector2 dashMovement;
    private float dashingTime;
    public float startDashingTime;

    private bool isStepping = false;

    public float startDashCooldown;
    private float dashCooldown;
    private bool canDash = true;

    public DashCooldownController dashCooldownController;

    private void Start()
    {
        dashCooldownController.SetCDTime(startDashCooldown);
    }

    private void Update()
    {
        Move();
    }

    private void Dashing()
    {
        if (dashingTime <= 0) 
        {
            isDashing = false;
            Gun.SetActive(true);
            dashCooldown = startDashCooldown;
            canDash = false;
            spotlight.enabled = true;
        }
        else
        {
            dashingTime -= Time.deltaTime;
            rb.MovePosition(rb.position + dashMovement * dashSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Enemy") && (isDashing == true) && (canDash == true))
        {
            other.GetComponent<Enemy>().TakeDamage(5);
            FindObjectOfType<AudioManager>().Play("ZombieTakeDamage");
        }
    }

    private void Move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x == 0 && movement.y == 0)
        {
            FindObjectOfType<AudioManager>().Stop("Step");
            isStepping = false;
        }

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (!isDashing)
        {
            if (rotZ > 90 || rotZ < -90)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && canDash)
        {
            isDashing = true;
            FindObjectOfType<AudioManager>().Play("Dash");
            Gun.SetActive(false);
            spotlight.enabled = false;
            dashMovement = movement.normalized;
            dashingTime = startDashingTime;
            dashCooldownController.Cooldown(startDashingTime - dashingTime);
        }
        else if (!isDashing)
        {
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
        }
        else if (isDashing)
        {
            movement = Vector2.zero;
            Dashing();
        }
        
        if (canDash == false)
        {
            DashCooldown();
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("isDashing", isDashing);
    }

    private void DashCooldown()
    {
        if (dashCooldown <= 0)
        {
            canDash = true;
            dashCooldownController.Cooldown(startDashCooldown);
        }
        else
        {
            dashCooldown -= Time.deltaTime;
            dashCooldownController.Cooldown(startDashCooldown - dashCooldown);
        }
    }

    private void SpawnParticle()
    {
        if (!isStepping)
        {
            FindObjectOfType<AudioManager>().Play("Step");
            isStepping = true;
        }
        Instantiate(Particle, particlePoint.position, particlePoint.rotation);
        if (isLeftLeg)
        {
            particlePoint.position = transform.position + new Vector3(-0.222f, 0, 0);
            isLeftLeg = false;
        }
        else
        {
            particlePoint.position = transform.position + new Vector3(0.222f, 0, 0);
            isLeftLeg = true;
        }
    }
}
