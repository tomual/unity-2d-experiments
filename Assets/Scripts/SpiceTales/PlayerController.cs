using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = transform.Find("body").GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/SpiceTales/player_e.overrideController");
        Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/SpiceTales/player_w.overrideController");
        Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/SpiceTales/player_n.overrideController");
        Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/SpiceTales/player_s.overrideController");
        Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/SpiceTales/player_se.overrideController");
        Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/SpiceTales/player_sw.overrideController");
        Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/SpiceTales/player_ne.overrideController");
        Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/SpiceTales/player_nw.overrideController");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }


    private void UpdateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            string direction = "s";
            if (horizontal > 0)
            {
                direction = "e";
                horizontal = 1;
            }
            else if (horizontal < 0)
            {
                direction = "w";
                horizontal = -1;
            }
            else if (vertical > 0)
            {
                direction = "n";
                vertical = 1;
            }
            else if (vertical < 0)
            {
                direction = "s";
                vertical = -1;
            }

            if (vertical == 1 && horizontal == 1)
            {
                direction = "ne";
            }
            else if (vertical == -1 && horizontal == 1)
            {
                direction = "se";
            }
            else if (vertical == 1 && horizontal == -1)
            {
                direction = "nw";
            }
            else if (vertical == -1 && horizontal == -1)
            {
                direction = "sw";
            }

            Debug.Log(direction);

            Addressables.LoadAssetAsync<AnimatorOverrideController>("Assets/Sprites/SpiceTales/player_" + direction + ".overrideController").Completed += OnLoadDone;

            Vector3 target = transform.position + new Vector3(horizontal, vertical);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 1);
        }
        else
        {
        }
    }

    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<AnimatorOverrideController> obj)
    {
        // In a production environment, you should add exception handling to catch scenarios such as a null result.
        animator.runtimeAnimatorController = obj.Result;
    }
}
