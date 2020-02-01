using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class Player : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        string[] toDefault =
        {
            "arm.l",
            "arm.r",
            "leg.l",
            "leg.r",
            "torso",
            "head",
            "head.face",
        };
        foreach (Transform child in transform)
        {
            Debug.Log(child.name);
            SpriteResolver spriteResolver = child.GetComponent<SpriteResolver>();
            if (spriteResolver)
            {
                if (Array.IndexOf(toDefault, child.name) >= 0)
                {
                    spriteResolver.SetCategoryAndLabel(child.name, "default");
                }
                else
                {
                    spriteResolver.SetCategoryAndLabel(child.name, "empty");
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 target = transform.position + new Vector3(horizontal, vertical);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 3);
            if (horizontal < 0)
            {
                transform.localScale = new Vector3(1, 1);
            }
            else if (horizontal > 0)
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
}
