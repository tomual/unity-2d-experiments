using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
    bool canDoubleJump = false;
    Effects effects;
    GameItem[] inventory;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PopulateAnimators();
        animator = transform.Find("body").GetComponent<Animator>();
        weaponCollider = transform.Find("WeaponCollider").GetComponent<BoxCollider2D>();
        weaponCollider.enabled = false;
        effects = GameObject.FindGameObjectWithTag("Effects").GetComponent<Effects>();
        InitiatePanelInventory();
    }

    void InitiatePanelInventory()
    {

    }

    void PopulateAnimators()
    {
        animators = new Animator[] {
            transform.Find("armor-back").GetComponent<Animator>(),
            transform.Find("body").GetComponent<Animator>(),
            transform.Find("hair").GetComponent<Animator>(),
            transform.Find("armor").GetComponent<Animator>()
        };

        Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/MagicalWorld/armor-2.overrideController").Completed += OnLoadDone;
    }

    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<AnimatorOverrideController> obj)
    {
        // In a production environment, you should add exception handling to catch scenarios such as a null result.
        Debug.Log(obj.Result);
        animators[3].runtimeAnimatorController = obj.Result;
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
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 2f);
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(-2, 2);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector3(2, 2);
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

        if (Input.GetButton("Jump"))
        {
            if (isGrounded)
            {
                float thrust = 7.0f;
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(new Vector2(0, thrust));
                rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
                StartCoroutine(EnableDoubleJump());
            } else if (canDoubleJump == true)
            {
                canDoubleJump = false;
                float thrust = 5.0f;
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(new Vector2(0, thrust));
                Vector3 direction = new Vector3(1, 1, 0);
                if (transform.localScale.x > 0)
                {
                    direction = new Vector3(-1, 1, 0);
                }
                rb.AddForce(direction * thrust, ForceMode2D.Impulse);
                effects.Play("DoubleJump", transform.localPosition, (int) transform.localScale.x);
            }
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
    IEnumerator EnableDoubleJump()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("enable!");
        canDoubleJump = true;
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

public enum GameItemType
{
    EQUIP = 1,
    USE = 2,
    QUEST = 3,
    MISC = 4
}

public class GameItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public GameItemType Type { get; set; }

}

public class GameBagSlot
{
    public int GameItemId { get; set; }
    public int Quantity { get; set; }

}
