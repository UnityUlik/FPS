using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private Vector3 crouchScale = new Vector3(1f, 0.8f, 1f);
    private Vector3 playerScale = new Vector3(1f, 1.2841f, 1f);

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y -0.5f, transform.position.z);
        }

        if(Input.GetKeyUp(KeyCode.Z))
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y +0.5f, transform.position.z);
        }
    }
}
