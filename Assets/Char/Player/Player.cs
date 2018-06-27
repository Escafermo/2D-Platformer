using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour 
{
    [SerializeField] float timeToRun = 1;
    [SerializeField] float walkSpeed = 1;
    [SerializeField] float runSpeed = 2;
    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float climbSpeed = 3;
    [SerializeField] Vector2 deathKick = new Vector2(250f,250f);

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCollider;
    PolygonCollider2D feetCollider;


    private float time;
    private float playerGravity;
    private float climbValue;
    bool isAlive = true;

    void Start ()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<PolygonCollider2D>();
        playerGravity = myRigidbody.gravityScale;
	}


	void Update ()
    {
        if (!isAlive) { return; }

        FlipSprite();
        Walk();
        Run();
        Climb();
        Jump();
        //Die();
    }


    void FlipSprite()
    {
        bool playerIsMoving = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerIsMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }


    void Walk()
    {
        float controlValue = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlValue * walkSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
       
        if (controlValue != 0)
        {
            time += Time.deltaTime;
            myAnimator.SetBool("Walk", true);
        }
        else
        {
            time = 0;
            myAnimator.SetBool("Walk", false);
            myAnimator.SetBool("Run", false);
        }
    }

    void Run()
    {

        if (time >= timeToRun)
        {
            myAnimator.SetBool("Run", true);
            float controlValue = CrossPlatformInputManager.GetAxis("Horizontal");
            Vector2 playerVelocity = new Vector2(controlValue * runSpeed, myRigidbody.velocity.y);
            myRigidbody.velocity = playerVelocity;
        }
        
    }

    
    void Jump()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAnimator.SetBool("Jump", false);
            if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
            {
                if (CrossPlatformInputManager.GetButtonDown("Jump"))
                {
                    myAnimator.SetBool("Jump", true);
                    Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
                    myRigidbody.velocity += jumpVelocity;
                }
            }
        }
    }

    void Climb()
    {
        myAnimator.SetBool("Climb", false);
        myRigidbody.gravityScale = playerGravity;
        climbValue = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 playerClimb = new Vector2(myRigidbody.velocity.x, climbValue * climbSpeed);

        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            myRigidbody.gravityScale = 0.0001f;

            if (climbValue != 0f)
            {
                myAnimator.SetBool("Climb", true);
                myRigidbody.velocity = playerClimb;
            }
        }
    }

    public void Die()
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Die");
            myRigidbody.velocity = deathKick;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Die");
            myRigidbody.velocity = deathKick;
        }
    }


}
