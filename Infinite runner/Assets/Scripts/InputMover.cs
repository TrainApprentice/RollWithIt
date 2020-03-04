using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMover : MonoBehaviour {

    LineRenderer line;
    InputManager.InputConfig playerController;

    public Rigidbody rb;
    private float forwardAccel = 6f;
    private float speed = 100f;


	void Start () {
        line = GetComponent<LineRenderer>();

        playerController = InputManager.player1;
	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<MeshRenderer>().material.color = playerController.OnJump(true) ? Color.yellow : Color.white;

        Vector3 input = playerController.GetAxis1();
        transform.position = input;
        line.SetPosition(1, input);

        if (input.sqrMagnitude > 1) input.Normalize();

// HEAD
        //print("Running");
        //++points;
        setPointDisplay();

    }
    void FixedUpdate()
    {
        rb.AddForce(playerController.GetAxis1x(false) * speed * Time.deltaTime, 0f, forwardAccel);
        print(rb.velocity);
        if (rb.velocity.z >= 35) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 35f);
        else forwardAccel = 6f;
        //print("Player: " + transform.position);
        
    }
   

    void setPointDisplay()
    {
        //pointDisplay.text = "Points: " + points.ToString();

        
        
// Justin
    }
}
