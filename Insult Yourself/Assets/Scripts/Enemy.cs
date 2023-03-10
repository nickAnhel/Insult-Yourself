using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public int health;
    public float speed;

    public int attackDamage;
    private float timeBtwattack;
    public float startTimeBtwattack;

    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;
    private Animator camAnim;
    private Animator hitScreen;
    public Light2D[] redEyes;

    private void Start()
    {
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        hitScreen = GameObject.FindGameObjectWithTag("HitScreen").GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private IEnumerator OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (timeBtwattack <= 0)
            {
                timeBtwattack = startTimeBtwattack;
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(0.33f);
                FindObjectOfType<AudioManager>().Play("ZombieBite");
                if (other != null)
                {
                    if ((Vector2.Distance(transform.position, other.transform.position) <= 1))
                    {
                        other.gameObject.GetComponent<HeroHealth>().UpdateHealth(-attackDamage);
                        camAnim.SetTrigger("Shake");
                        hitScreen.SetTrigger("Hit");
                    }
                }
            }
            else
            {
                timeBtwattack -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!target)
            {
                FindObjectOfType<AudioManager>().Play("ZombieRage");
            }
            target = other.transform;
        }
    }

    private void Move()
    {
        if (target == null)
            return;
        foreach (Light2D l in redEyes)
        {
            l.enabled = true;
        }
        movement.x = (target.position.x - transform.position.x);
        movement.y = (target.position.y - transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (target.position.x - transform.position.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        foreach (Light2D l in redEyes)
        {
            l.enabled = false;
        }
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            FindObjectOfType<AudioManager>().Play("ZombieDeath");
            GetComponent<BoxCollider2D>().enabled = false;
            return;
        }
        animator.SetTrigger("TakeDamage");
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
