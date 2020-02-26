using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiAI : MonoBehaviour
{
    public GameObject player;
    public Rigidbody rb;
    public SphereCollider hitBall;
    
    public GameObject ground;
    private BoxCollider groundHitBox;
    public GameObject JumpReticle;
    public GameObject Shockwave;
    private bool shockSpawned = false;
    private Rigidbody playerBody;
    InputManager.InputConfig activateAbilities;
    private float speed = 100f;
    private float forwardAccel = 5.5f; // 5.5f
    private float moveDistance;
    private float attackTimeDelay = 3f;
    private bool attacking = false;
    
    private bool jumpReticleMade = false;
    public Vector3 jumpPoint = new Vector3();
    private bool hasJumped = false;
    public bool JumpLanded = false;
    private float landCooldown = 2.0f;
    private float jumpTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerBody = player.GetComponent<Rigidbody>();
        activateAbilities = InputManager.player2;
        hitBall = GetComponent<SphereCollider>();
        
}

    // Update is called once per frame
    void Update()
    {
        if (activateAbilities.OnJump(true))
        {
            //Do all the AI things!
            
            attacking = true;

        }
        if (attacking)
        {
           
            JumpAttack();
           
        }
        StandStill();
        UpdateReticle();
        

    }
    private void FixedUpdate()
    {
        moveDistance = Mathf.Abs(rb.position.x - playerBody.position.x);
        if (moveDistance >= 1)
        {
            if (!hasJumped)
            {
                if (rb.position.x - playerBody.position.x < 0) rb.AddForce(moveDistance * speed * Time.deltaTime, 0f, forwardAccel);
                else rb.AddForce(-moveDistance * speed * Time.deltaTime, 0f, forwardAccel);
            }
            

        }
        else
        {

            if (!hasJumped)
            {
                rb.AddForce(0f, 0f, forwardAccel);
                rb.velocity = new Vector3((float)(rb.velocity.x * .99), rb.velocity.y, rb.velocity.z);
            }

            

        }

        if (rb.velocity.z >= 35 || (Mathf.Abs(rb.position.z - playerBody.position.z) <= 2) || rb.position.z > playerBody.position.z) forwardAccel = 0f;
        else if ((Mathf.Abs(rb.position.z - playerBody.position.z) >= 50)) forwardAccel = 8f;
        else forwardAccel = 5.5f;
    }
    private void JumpAttack()
    {
        
        attackTimeDelay -= Time.deltaTime;
        if (attackTimeDelay > 0f)
        {
            
            if (!jumpReticleMade)
            {
                Instantiate(JumpReticle, jumpPoint, Quaternion.identity);
                jumpReticleMade = true;
                
            }
            else
            {
                
                
            }

        }
        else
        {
            float playerDX = jumpPoint.x - rb.position.x;
            float playerDZ = jumpPoint.z - rb.position.z;
            float groundAngle = Mathf.Atan2(playerDZ, playerDX);


            
            rb.velocity = new Vector3(playerDX / 2.35f, 11.5f, Mathf.Abs(rb.velocity.z - playerBody.velocity.z) * (playerDZ / 2.35f));
            forwardAccel = 0f;
            jumpReticleMade = false;
            attackTimeDelay = 3;
            hasJumped = true;
            shockSpawned = false;
            attacking = false;
        }




    }
    private void StandStill()
    {
        
        if (JumpLanded)
        {
            

            landCooldown -= Time.deltaTime;
                if (landCooldown > 0f)
                {
                    rb.velocity = new Vector3(0f, 0f, 0f);
                }
                else
                {
                    JumpLanded = false;
                    //hasJumped = false;
                    
                }
            
        }
        if (!JumpLanded)
        {
            landCooldown = 2.0f;
        }
        //print(landCooldown);
        
        
    }
    private void OnCollisionEnter(Collision other)
    {
        
        if (hasJumped && !shockSpawned)
        {
            print("Huzzah!");
            Instantiate(Shockwave);
            Shockwave.transform.position = this.transform.position;
            shockSpawned = true;
            JumpLanded = true;
            hasJumped = false;
        }
    }
    private void UpdateReticle()
    {
        if(!JumpLanded)
        {
            if (hasJumped)
            {
                jumpTimer += Time.deltaTime;
                print(jumpTimer + " Jump Timer");
            }
            
            if (!hasJumped) {
                jumpPoint = new Vector3(playerBody.position.x, playerBody.position.y, playerBody.position.z + 20);
            }
            else
            {
                jumpPoint = new Vector3(jumpPoint.x, jumpPoint.y, playerBody.position.z + 20);
            }
        }
        
    }
    

}
