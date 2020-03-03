using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMover : MonoBehaviour {

    LineRenderer line;
    InputManager.InputConfig playerController;

	void Start () {
        line = GetComponent<LineRenderer>();

        playerController = InputManager.player1;
	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<MeshRenderer>().material.color = playerController.OnJump(true) ? Color.yellow : Color.white;

        Vector3 input = playerController.GetAxis1();

        if (input.sqrMagnitude > 1) input.Normalize();

        transform.position = input;
        line.SetPosition(1, input);
        
    }
}
