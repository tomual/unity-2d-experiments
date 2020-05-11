using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            if (horizontal > 0)
            {
                horizontal = 1;
            } else if (vertical > 0)
            {
                vertical = 1;
            }
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
        }
        else
        {
        }
    }
}
