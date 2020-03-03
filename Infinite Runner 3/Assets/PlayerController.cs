using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* To Do List
 * The squirrel girl should just be facing in the direction of the hypotenuse of the velocity vector components (x and z)
 * Make death upon size == 0
 * fine-tune friction (horizontally, don't worry about vertical yet. Don't forget that turning in opposite direction > letting go then turning)
 * might need to include mass in addForce to account for same force on larger mass leading to lower max speeds?
 * interaction when running into something (losing distance/mass/angular drag/size)
 * Later: z distance and speed stuff (cap, movement in general. Possibly done by gravity and angular drag, i.e. not player input)
 */

/* Fine-tuning
 * Ball cannot reach max horizontal speed at larger sizes
 *      Solution?: add mass to addForce()
 * Ball slowing down is a lot quicker than speeding up
 *      Solution?
 * Adjust mass/size/angular drag as necessary
 */


public class PlayerController : MonoBehaviour
{
    //display and required variables
    InputManager.InputConfig playerController;
    public Rigidbody rb;
    private float speed = 100f;
    public int points;
    public Text pointDisplay;
    public float vel;
    public Text velocityDisplay;
    public float zVel;
    public Text zVelocityDisplay;
    //for changing sphere size
    private float distanceTraveled;
    private float prevZ;
    private float maxVel;
    private float deltaDistance;
    private float distanceToPoints;
    private float scaleChanger;
    

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
        rb.AddForce(playerController.GetAxis1x() * speed * Time.fixedDeltaTime, 0f, playerController.GetAxis1z() * speed * Time.fixedDeltaTime);
        //print(speed * Time.fixedDeltaTime);



        /* pause
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0f;
        }
        */

        /* manually change size
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0.1f);
            rb.transform.localScale += scaleChange;
            rb.angularDrag += 0.05f;
            rb.mass += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0.1f);
            rb.transform.localScale -= scaleChange;
            rb.angularDrag -= 0.05f;
            rb.mass -= 0.1f;
        }*/
    }

    // FixedUpdate is called once per physics calculation
    void FixedUpdate()
    {
        //add to distance traveled
        deltaDistance = transform.position.z - prevZ;
        distanceTraveled += Mathf.Abs(deltaDistance);
        if (distanceTraveled > 100f) distanceTraveled = 100f;
        prevZ = transform.position.z;

        //add points based on deltaDistance
        distanceToPoints += Mathf.Abs(deltaDistance);
        if(distanceToPoints > 3f)
        {
            ++points;
            distanceToPoints -= 3f;
        }

        //map distanceTraveled to snowball size, mass, and angular drag
        rb.mass = Mathf.Lerp(1f, 2f, (distanceTraveled/100f));
        rb.angularDrag = Mathf.Lerp(.25f, .75f, (distanceTraveled / 100f));
        scaleChanger = Mathf.Lerp(1f, 5f, (distanceTraveled / 100f));
        Vector3 scaleChange = new Vector3(scaleChanger, scaleChanger, scaleChanger);
        rb.transform.localScale = scaleChange;


        //set horizontal speed cap based on mass
        if (rb.mass < 1.33) maxVel = 5 * rb.mass;
        else if (rb.mass < 1.66) maxVel = 5 * rb.mass * 2;
        else maxVel = 5 * rb.mass * 5;

        //speed cap for horizontal direction. Something else should ta
        if(Mathf.Abs(rb.velocity.x) > maxVel)
        {
            rb.velocity = new Vector3(maxVel * (rb.velocity.x / Mathf.Abs(rb.velocity.x)), 0f, 0f);
        }
        //velocity = 0 when low enough velocity
        else if((playerController.GetAxis1x() == 0) && Mathf.Abs(rb.velocity.x) < .2f) rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);

        if ((playerController.GetAxis1z() == 0) && Mathf.Abs(rb.velocity.z) < .2f) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);



        /*friction so that you actually slow down when not pressing something
         * die faster at lower masses
         * change since you will turn slower by pressing opposite direction than not touching anything
         * all of them have the same rate of acceleration BUT NOT NEGATIVE ACCELERATION. Different max speeds
        */

        float adjustment = map(Mathf.Abs(playerController.GetAxis1x()), 0, 1, 1, 0);
        rb.AddForce((rb.velocity.x / Mathf.Abs(rb.velocity.x)) * speed * Time.fixedDeltaTime * adjustment * -1, 0f, 0f);
        if((rb.velocity.x < 0f && playerController.GetAxis1x() > 0f) || (rb.velocity.x > 0f && playerController.GetAxis1x() < 0f))
        {
            rb.AddForce(speed * Time.fixedDeltaTime * playerController.GetAxis1x(), 0f, 0f);
        }



        /*if (rb.mass < .3) rb.velocity = new Vector3(rb.velocity.x * .94f, rb.velocity.y, rb.velocity.z);// * rb.mass * 3
        else if (rb.mass < .6) rb.velocity = new Vector3(rb.velocity.x * .97f, rb.velocity.y, rb.velocity.z); // * rb.mass * 1.5f
        else rb.velocity = new Vector3(rb.velocity.x * .99f, rb.velocity.y, rb.velocity.z); // * rb.mass
        */




        /*
        if (playerController.GetAxis1x() == 0)
        {
            if (Mathf.Abs(rb.velocity.x) < .2f) rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);//(playerController.GetAxis1x() == 0) &&    add this back in
            else rb.velocity = new Vector3(rb.velocity.x * .94f, rb.velocity.y, rb.velocity.z);        //needed because friction is booty
            //the above line is for "friction" (really it's just a speed decreaser)
        }
        */
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
