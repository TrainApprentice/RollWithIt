using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public Rigidbody rb;
    private float coolspeed = 750f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent < Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Input.GetAxis("Horizontal") * coolspeed * Time.deltaTime, 0f, Input.GetAxis("Vertical") * coolspeed * Time.deltaTime);
        //print("velocity = " + rb.velocity);
        if (rb.velocity.z >= 100)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 100f);
        }
        
    }
}
