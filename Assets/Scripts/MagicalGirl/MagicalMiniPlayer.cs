using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalMiniPlayer : MonoBehaviour
{
    float horizontal;
    float vertical;
    Rigidbody2D rb;
    Animator animator;
    Animator[] animators;
    bool isGrounded;
    bool isAttacking;
    bool isDead;
    BoxCollider2D weaponCollider;
    int health = 20;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PopulateAnimators();
        animator = transform.Find("body").GetComponent<Animator>();
        weaponCollider = transform.Find("WeaponCollider").GetComponent<BoxCollider2D>();
        weaponCollider.enabled = false;
    }

    void PopulateAnimators()
    {
        animators = new Animator[] {
            transform.Find("armor-back").GetComponent<Animator>(),
            transform.Find("body").GetComponent<Animator>(),
            transform.Find("hair").GetComponent<Animator>(),
            transform.Find("armor").GetComponent<Animator>()
        };

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        isGrounded = Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), LayerMask.NameToLayer("Ground"));
        SetAnimationBool("Grounded", isGrounded);
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 && !isAttacking)
        {
            Vector3 target = transform.position + new Vector3(horizontal, 0);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 1.5f);
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(-1, 1);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector3(1, 1);
            }
            SetAnimationBool("Walking", true);
        } 
        else
        {
            SetAnimationBool("Walking", false);
        }

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            float thrust = 6.0f;
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0, thrust));
            rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        SetAnimationTrigger("Attack");
        StartCoroutine(TriggerWeaponCollider());
        yield return new WaitForSeconds(0.7f);
        weaponCollider.enabled = false;
        isAttacking = false;
    }
    IEnumerator TriggerWeaponCollider()
    {
        yield return new WaitForSeconds(0.4f);
        weaponCollider.enabled = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            TestUIController.instance.GameOver();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "EnemyWeaponCollider")
        {
            Debug.Log(collision.GetComponentInParent<TestEnemy>().damage);
        }
        if (collision.name == "Teleporter")
        {
            TestTeleporter teleporter = collision.GetComponent<TestTeleporter>();
            if (Input.GetAxis("Vertical") > 0)
            {
                TestUIController.instance.TriggerTeleport(teleporter.destination);
            }
        }
    }

    void SetAnimationBool(string name, bool boolean)
    {
        foreach (var animator in animators)
        {
            animator.SetBool(name, boolean);
            if (name == "Walking" && boolean == false)
            {
                animator.speed = 0.5f;
            } 
            else
            {
                animator.speed = 1;
            }
        }
    }

    void SetAnimationTrigger(string name)
    {
        foreach (var animator in animators)
        {
            animator.SetTrigger(name);
            animator.speed = 1f;
        }
    }
}

