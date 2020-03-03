using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* To Do List
 * Make death upon size == 0
 * interaction when running into something (power ups)
 * Make sure z speed and stuff is good with everyone else
 * take out velocity texts?
 * take out the space, v, m keys
 */


public class PlayerController : MonoBehaviour
{
    //display and required variables
    InputManager.InputConfig playerController;
    public Rigidbody rb;
    public int points;
    public Text pointDisplay;
    public float vel;
    public Text velocityDisplay;
    public float zVel;
    public Text zVelocityDisplay;
    //for changing sphere size and velocity
    private float distanceTraveled;
    private float prevZ;
    private float maxVel;
    private float deltaDistance;
    private float distanceToPoints;
    private float scaleChanger;
    private float acceleration;
    //power ups
    private float acornTime;
    private float pointsMultTime;
    private int acorn;
    private int pointsMult;
    private bool hit;
    
    

    // Start is called before the first frame update
    void Start()
    {
        playerController = InputManager.player1;
        rb = GetComponent<Rigidbody>();
        points = 0;
        setPointDisplay();
        distanceTraveled = 0;
        prevZ = transform.position.z;
        distanceToPoints = 0;
        scaleChanger = 0;
        acceleration = 0;
        rb.angularDrag = 0f;
        acorn = 1;
        acornTime = 0f;
        hit = false;
        pointsMult = 1;
        pointsMultTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //display
        setPointDisplay();
        vel = rb.velocity.x;
        setVelocityDisplay();
        zVel = rb.velocity.z;
        zSetVelocityDisplay();
        
        
        /* pause
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0f;
        }
        */
        
        // manually change size for testing
        if (Input.GetKeyDown(KeyCode.Space)) distanceTraveled += 10f;
        if (Input.GetKeyDown(KeyCode.V)) distanceTraveled -= 10f;
        if (Input.GetKeyDown(KeyCode.M)) print(maxVel);


        //calculate mass, angular drag, and size by mapping distanceTraveled to them
        rb.mass = Mathf.Lerp(1f, 2f, (distanceTraveled / 100f));
        scaleChanger = Mathf.Lerp(2f, 5f, (distanceTraveled / 100f));
        Vector3 scaleChange = new Vector3(scaleChanger, scaleChanger, scaleChanger);
        rb.transform.localScale = scaleChange;

        //set horizontal (and forward/backward) speed cap based on mass. calculate maxVel, time (acceleration). addVelocity. in fixedUpdate()

        //friction/returning to 0: in fixedUpdate()

        //cap velocity if necessary: in fixedUpdate()

        //distance traveled, points: in fixedUpdate()
    }

    // FixedUpdate is called once per physics calculation
    void FixedUpdate()
    {
        //set horizontal (and forward/backward) speed cap based on mass. calculate maxVel, time (acceleration). addVelocity
        if (rb.mass < 1.33f)
        {
            maxVel = 4f * rb.mass;//min = 4, max = 5.32
            acceleration = map(distanceTraveled, 0f, 33f, .5f, .75f);
            rb.velocity += new Vector3(playerController.GetAxis1x() * acorn * maxVel * (Time.fixedDeltaTime / acceleration), 0f, .5f * maxVel * (Time.fixedDeltaTime / acceleration));//min = 8 m/s^2, max = 6.259 m/s^2
        }
        else if (rb.mass < 1.66f)
        {
            maxVel = 5f * rb.mass;//min = 6.65, max = 8.3
            acceleration = map(distanceTraveled, 33f, 66f, 1.25f, 2f);
            rb.velocity += new Vector3(playerController.GetAxis1x() * acorn * maxVel * (Time.fixedDeltaTime / acceleration), 0f, .5f * maxVel * (Time.fixedDeltaTime / acceleration));//min = 5.32 m/s^2, max = 4.15 m/s^2
        }
        else
        {
            maxVel = 6f * rb.mass;//min = 9.96, max = 12
            acceleration = map(distanceTraveled, 66f, 100f, 3f, 4.2f);
            rb.velocity += new Vector3(playerController.GetAxis1x() * acorn * maxVel * (Time.fixedDeltaTime / acceleration), 0f, .5f * maxVel * (Time.fixedDeltaTime / acceleration));//min = 3.32 m/s^2, max = 2.857 m/s^2
        }

        //friction so that you actually slow down when not pressing something. Return to 0 horizontal velocity faster at lower masses
        //gets absoulte value of amount a joystick is pushed in a direction
        float adjustment = map(Mathf.Abs(playerController.GetAxis1x()), 0, 1, 1, 0);

        if (rb.velocity.x != 0f && playerController.GetAxis1x() == 0)//if no player input, but horizontal movement
        {
            rb.velocity += new Vector3((rb.velocity.x / Mathf.Abs(rb.velocity.x)) * -1 * adjustment * acorn * maxVel * (Time.fixedDeltaTime / acceleration), 0f, 0f);
        }
        
        //for when you're moving in one direction and then try to move in the other
        if((rb.velocity.x < 0f && playerController.GetAxis1x() > 0f) || (rb.velocity.x > 0f && playerController.GetAxis1x() < 0f))
        {
            rb.velocity += new Vector3(playerController.GetAxis1x() * acorn * maxVel * (Time.fixedDeltaTime / acceleration), 0f, 0f);
        }


        //speed cap for horizontal direction
        if (Mathf.Abs(rb.velocity.x) > maxVel)
        {
            rb.velocity = new Vector3(maxVel * (rb.velocity.x / Mathf.Abs(rb.velocity.x)), rb.velocity.y, rb.velocity.z);
        }
        //velocity = 0 when low enough velocity
        else if ((playerController.GetAxis1x() == 0) && Mathf.Abs(rb.velocity.x) < .2f) rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);

        //speed cap for z direction
        if (Mathf.Abs(rb.velocity.z) > maxVel)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxVel * (rb.velocity.z / Mathf.Abs(rb.velocity.z)));
        }
        //keep a minimum z speed
        else if (Mathf.Abs(rb.velocity.z) < .5f) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, .5f);


        //add to distance traveled
        deltaDistance = transform.position.z - prevZ;
        distanceTraveled += Mathf.Abs(deltaDistance);
        if (distanceTraveled > 100f) distanceTraveled = 100f;
        if (distanceTraveled < 0f) distanceTraveled = 0f;                                                                                                       //change to death screen
        prevZ = transform.position.z;

        //points power up time
        pointsMultTime += Time.fixedDeltaTime;
        if (pointsMultTime == 2 && (pointsMultTime > 10f || hit))
        {
            pointsMultTime = 0;
            pointsMult = 1;
            hit = false;
        }
        else if (pointsMult == 1)
        {
            pointsMultTime = 0f;
        }

        //add points based on deltaDistance
        distanceToPoints += Mathf.Abs(deltaDistance);
        if(distanceToPoints > 3f)
        {
            points += pointsMult;
            distanceToPoints -= 3f;
        }


        //acorn power up
        acornTime += Time.fixedDeltaTime;
        if (acorn == 2 && (acornTime > 10f || hit))
        {
            acornTime = 0;
            acorn = 1;
            hit = false;
        }
        else if (acorn == 1)
        {
            acornTime = 0f;
        }

        //at the very end of this code, if you were hit, reset hit to false because otherwise you would be in permanent hit without power up
        if (hit) hit = false;
    }//fixedUpdate()



    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Acorn"))
        {
            acorn = 2;
            other.gameObject.SetActive(false);
        }

        else if(other.gameObject.CompareTag("Pinecone"))
        {
            pointsMult = 2;
            other.gameObject.SetActive(false);
        }

        else if (other.gameObject.CompareTag("Obstacle"))
        {
            hit = true;
            other.gameObject.SetActive(false);
            distanceTraveled -= 33;
            if (rb.mass < 1.33) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, .5f);
            else if (rb.mass < 1.66) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 2f);
            else rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 4f);
        }
    }

    void setPointDisplay()
    {
        pointDisplay.text = "Points: " + points.ToString();
    }

    void setVelocityDisplay()
    {
        velocityDisplay.text = "X Velocity: " + vel.ToString();
    }

    void zSetVelocityDisplay()
    {
        zVelocityDisplay.text = "Z Velocity: " + zVel.ToString();
    }

    float map(float input, float range1, float range2, float mapped1, float mapped2)
    {
        return (input - range1) / (range2 - range1) * (mapped2 - mapped1) + mapped1;
    }
}
