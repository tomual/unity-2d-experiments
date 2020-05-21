using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Animator animator;

    public int hp;
    public int mp;
    public int maxHp;
    public int maxMp;

    void Awake()
    {
        animator = GetComponent<Animator>();

        InitStats();
        InitSpriteResolve();
    }

    void Update()
    {
        UpdateSpriteSortingLayer();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButton("Fire1"))
        {
            Portal portal = collision.GetComponent<Portal>();
            if (portal && !string.IsNullOrEmpty(portal.destination))
            {
                UIManager.instance.Portal(portal.destination);
            }
        }
    }

    private void InitSpriteResolve()
    {
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

    private void InitStats()
    {
        hp = 96;
        mp = 100;
        maxHp = 100;
        maxMp = 100;
        UIManager.instance.InitHP(maxHp);
        UIManager.instance.InitMP(maxMp);
        UIManager.instance.UpdateHP(hp);
        UIManager.instance.UpdateMP(mp);
    }

    private void UpdateSpriteSortingLayer()
    {
        int layer = (int)(transform.position.y * 10);
        layer *= 100;

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            int baseLayer = spriteRenderers[i].sortingOrder % 100;
            spriteRenderers[i].sortingOrder = baseLayer + layer * -1;
        }
    }

    private void UpdateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 target = transform.position + new Vector3(horizontal, vertical);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 4);
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
