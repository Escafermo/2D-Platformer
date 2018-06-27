using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{

    [SerializeField] float timeToWalk;
    [SerializeField] float timeToIdle;
    [SerializeField] float timeWalking;
    [SerializeField] float walkSpeed = 1f;

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    CapsuleCollider2D myCapsuleCollider2D;
    Player thisPlayer;

    float timeUntilWalk;
    float timeUntilIdle;

    void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();

        thisPlayer = FindObjectOfType<Player>();
	}
	
	
	void Update ()
    {
        Idle();
        Walk();
        //Die();
        Die();
    }

    void Idle()
    {
        timeUntilWalk += Time.deltaTime;
        if (timeUntilIdle >= timeToIdle)
        {
            myAnimator.SetBool("Walk", false);
            Vector2 enemyIdle = new Vector2(0f, 0f);
            myRigidBody.velocity = enemyIdle;
        }
    }

    void Walk()
    {
        if (timeUntilWalk >= timeToWalk)
        {
            timeUntilIdle += Time.deltaTime;
            myAnimator.SetBool("Walk", true);

            if (IsFacingRight())
            {
                Vector2 enemyVelocity = new Vector2(walkSpeed, myRigidBody.velocity.y);
                myRigidBody.velocity = enemyVelocity;
            }
            else
            {
                Vector2 enemyVelocity = new Vector2(-walkSpeed, myRigidBody.velocity.y);
                myRigidBody.velocity = enemyVelocity;
            }

            if (timeUntilWalk >= timeWalking)
            {
                timeUntilWalk = 0f;
            }

        }
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            thisPlayer.Die();
        }
        transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
    }

    bool IsFacingRight()
    {
        return transform.localScale.x < 0;
    }

    void Die()
    {
        if (myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            Destroy(this.gameObject);
        }
    }

  
    //void FlipSprite()
    //{
    //    bool enemyIsMoving = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

    //    if (enemyIsMoving)
    //    {
    //        transform.localScale = new Vector2(-Mathf.Sign(myRigidBody.velocity.x), 1f);
    //    }
    //}

}
