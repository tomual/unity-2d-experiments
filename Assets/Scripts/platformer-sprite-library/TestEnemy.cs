using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    bool isActing;
    int walkHorizontal;
    Animator animator;
    GameObject player;
    BoxCollider2D weaponCollider;
    public int damage = 5;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        weaponCollider = transform.Find("EnemyWeaponCollider").GetComponent<BoxCollider2D>();
        weaponCollider.enabled = false;
    }

    private void Update()
    {
        Vector3 playerDistance = (transform.position - player.transform.position);
        if (!isActing)
        {
            if (Mathf.Abs(playerDistance.x) > 1)
            {
                StartCoroutine(Walk());
            }
            else
            {
                if (playerDistance.x > 0)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                else if (playerDistance.x < 0)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                StartCoroutine(Attack());
            }
        }
    }
    private void FixedUpdate()
    {
        if (walkHorizontal != 0)
        {
            Vector3 target = transform.position + new Vector3(walkHorizontal, 0);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
            if (walkHorizontal > 0)
            {
                transform.localScale = new Vector3(1, 1);
            }
            else if (walkHorizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1);
            }
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    IEnumerator Walk()
    {
        System.Random r = new System.Random();
        isActing = true;
        int[] directions = {1, -1};
        walkHorizontal = directions[r.Next(0, directions.Length)];
        yield return new WaitForSeconds(r.Next(1, 3));
        walkHorizontal = 0;
        yield return new WaitForSeconds(r.Next(1, 3));
        isActing = false;
    }

    IEnumerator Attack()
    {
        isActing = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(1);
        isActing = false;
        weaponCollider.enabled = false;
    }

}
