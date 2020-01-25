using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float horizontal;
    float vertical;
    Rigidbody2D rb;
    bool isGrounded;
    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), LayerMask.NameToLayer("Ground"));
        animator.SetBool("Grounded", isGrounded);
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal != 0)
        {
            Vector3 target = transform.position + new Vector3(horizontal, 0);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 3);
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(1, 1);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1);
            }
            Debug.Log("True");
            animator.SetBool("Walking", true);
        } 
        else
        {
            Debug.Log("False");
            animator.SetBool("Walking", false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            float thrust = 8.0f;
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0, thrust));
            rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }
    }
}
