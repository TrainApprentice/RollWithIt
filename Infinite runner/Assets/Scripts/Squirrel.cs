using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Squirrel : MonoBehaviour
{
    public GameObject other;
    public Rigidbody otherRB;
    private Vector3 lastRotate;// = new Vector3(0f, 3.14f, 0f);
    private Vector3 newRotate;
    private float theta;
    private float jumpHeight = 0f;
    public float jumpTimer = 3f;
    private float multiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        lastRotate = new Vector3(0f, 0f, 0f);
        newRotate = new Vector3(0f, 0f, 0f);
        otherRB = other.GetComponent<Rigidbody>();
        theta = 0f;
    }

    // Update is called once per physics calculation
    void FixedUpdate()
    {
        //transform.position used to be here

        //transform.Rotate(rate);
        //angleBoardRotX = Mathf.Rad2Deg*Mathf.Atan(angleBoardZtan);
        if (otherRB.velocity.x == 0f) newRotate = lastRotate;
        else
        {
            theta = Mathf.Rad2Deg * Mathf.Atan(otherRB.velocity.z / otherRB.velocity.x);
            //map to positive values
            if(theta >= 0) newRotate = new Vector3(0f, Mathf.Lerp(90f, 0f, (theta/90f)), 0f);
            //map to negative values
            else newRotate = new Vector3(0f, Mathf.Lerp(-90f, 0f, (theta / -90f)), 0f);
        }

        //rotate the squirrel
        transform.Rotate(newRotate - lastRotate);
        lastRotate = newRotate;


        //make the squirrel jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpTimer > 3f)
        {
            //start jump timer
            jumpTimer = 0f;
            //as long as it starts at +y and ends at -y
            jumpHeight = 0f;
        }
        if(jumpTimer <= 1.5f)
        {
            transform.position = new Vector3(other.transform.position.x, other.transform.position.y + (other.transform.localScale.y / 2f) + (jumpHeight * Time.deltaTime), other.transform.position.z);
            jumpHeight += (1000/3f) * Time.deltaTime * multiplier;
            multiplier -= Time.deltaTime;
        }
        else if (jumpTimer <= 3f)
        {
            transform.position = new Vector3(other.transform.position.x, other.transform.position.y + (other.transform.localScale.y / 2f) + (jumpHeight * Time.deltaTime), other.transform.position.z);
            jumpHeight -= (1000 / 3f) * Time.deltaTime * multiplier;
            multiplier += Time.deltaTime;
        }
        else
        {
            transform.position = new Vector3(other.transform.position.x, other.transform.position.y + (other.transform.localScale.y / 2f), other.transform.position.z);
        }

        if (multiplier < 0f) multiplier = 0f;
        jumpTimer += Time.deltaTime;
    }
}
