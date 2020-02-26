using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    private float speed = 100f;
    private float turnSpeedAdjust = 1f;
    private float forwardAccel = 6f;
    public int points;
    public Text pointDisplay;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        points = 0;
        setPointDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        //update points based on distance traveled
        //[insert some function for ++points on movement]
        ++points;
        setPointDisplay();
    }

    // FixedUpdate is calle once per physics calculation
    void FixedUpdate()
    {
        rb.AddForce(Input.GetAxis("Horizontal") * speed * turnSpeedAdjust * Time.deltaTime, 0f, forwardAccel);
        //print(rb.velocity);
        if (rb.velocity.z >= 35) forwardAccel = 0f;
        else forwardAccel = 6f;
        //print(GetComponent<Rigidbody>().position.z);
        //print(transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Power up"))
        {
            other.gameObject.SetActive(false);
        }
    }

    void setPointDisplay()
    {
        pointDisplay.text = "Points: " + points.ToString();
    }
}
