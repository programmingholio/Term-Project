using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool weapon;
    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        /*
         * ######################
         * # Track the Movement #
         * ######################
         */
        Vector2 move = Vector2.zero;


        move.x = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapon = true;
            animator.SetInteger("State", 6);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            weapon = false;
            animator.SetInteger("State", 0);
        }


        /* ########
         * # Jump #
         * ########
         */


        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            if(weapon)
            {
                animator.SetInteger("State", 7);
            }
            else animator.SetInteger("State", 2);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
            if(weapon)
            {
                animator.SetInteger("State", 6);
            }
            else animator.SetInteger("State", 0);
        }

        /*
         * ########
         * # Walk #
         * ########
         */
        bool flipSprite;
        if (Input.GetButtonDown("Horizontal"))
        {
            //Change Animation
            if(weapon)
            {
                animator.SetInteger("State", 8);
            }
            else animator.SetInteger("State", 4);

            //Flip the Player
            flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
        else if (Input.GetButtonUp("Horizontal"))
        {
            //Change Animation
            if(weapon)
            {
                animator.SetInteger("State", 6);
            }
            else animator.SetInteger("State", 0);

            //Flip the Player
            flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }

        /*
         * ################
         * # Pull the Gun #
         * ################
         */
        if( Input.GetMouseButtonDown(0) )
        {
            animator.SetInteger("State", 5);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetInteger("State", 0);
        }

        targetVelocity = move * maxSpeed;

    }

    /*
     * 
     * ###########################################
     * # Set the animator back to previous state #
     * ###########################################
     */
    void BackToPreviousState()
    {

    }
}