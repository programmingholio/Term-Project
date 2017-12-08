using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    //the bullet game object
    public GameObject bullet;

    public float fire_rate = 0.5f;
    public Vector2 offset = new Vector2 (0.5f, 0f);

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool weapon;
    private Vector2 bullet_position;
    private float next_fire = 0.0f;
    private bool flipSprite;
    private Vector2 move;

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
        move = Vector2.zero;
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
                //flip the bullet
                BulletScript.velx *= -1;
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
            /*flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }*/
        }

        /*
         * ################
         * # Pull the Gun #
         * ################
         */
        if( Input.GetMouseButtonDown(0) && Time.time > next_fire)
        {
            animator.SetInteger("State", 5);
            next_fire = Time.time + fire_rate;
            Fire();
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
    /*
     * #####################
     * # The fire function #
     * #####################
     */
    void Fire()
    {
        bullet_position = transform.position;
        //bullet_position += offset;

        //Flip the Player
        flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            bullet_position += offset;
        }
        else
        {
            bullet_position += offset * -1;
        }

        Instantiate(bullet, bullet_position, Quaternion.identity);
    }
}