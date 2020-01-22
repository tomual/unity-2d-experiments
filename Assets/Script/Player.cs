using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float timeMove = 0.8f;
    private float inverseMoveTime;
    private Rigidbody2D rb2D;

    float timeAttack = 1f;
    bool isAttacking = false;

    void Start()
    {

        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / timeMove;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            AttackStart();
        }

        if (CanMove())
        {
            Move();
        }
    }

    bool CanMove()
    {
        if (isAttacking)
        {
            return false;
        }
        return true;
    }

    void Move()
    {
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontal, vertical);
        Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
        rb2D.MovePosition(newPosition);
    }

    void AttackStart()
    {
        isAttacking = true;

    }
    void AttackStop()
    {

    }
}
