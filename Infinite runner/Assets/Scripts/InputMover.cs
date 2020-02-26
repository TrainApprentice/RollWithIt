using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMover : MonoBehaviour {

    LineRenderer line;
    InputManager.InputConfig playerController;
    public Rigidbody rb;
    private float speed = 100f;
    private float turnSpeedAdjust = 1f;
    private float forwardAccel = 6f; // 6f
    public int points;
    public Text pointDisplay;

    void Start () {
        line = GetComponent<LineRenderer>();

        playerController = InputManager.player1;
        rb = GetComponent<Rigidbody>();
        points = 0;
        setPointDisplay();
    }
	
	// Update is called once per frame
	void Update () {

        GetComponent<MeshRenderer>().material.color = playerController.OnJump(true) ? Color.yellow : Color.white;

        Vector3 input = playerController.GetAxis1();

        if (input.sqrMagnitude > 1) input.Normalize();

        //transform.position = input;
        //line.SetPosition(1, input);
        ++points;
        setPointDisplay();

    }
    void FixedUpdate()
    {
        rb.AddForce(playerController.GetAxis1x(false) * speed * turnSpeedAdjust * Time.deltaTime, 0f, forwardAccel);
        //print(rb.velocity);
        if (rb.velocity.z >= 35) forwardAccel = 0f;
        else forwardAccel = 6f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Power up"))
        {
            other.gameObject.SetActive(false);
        }
    }

    void setPointDisplay()
    {
        pointDisplay.text = "Points: " + points.ToString();
    }
}
