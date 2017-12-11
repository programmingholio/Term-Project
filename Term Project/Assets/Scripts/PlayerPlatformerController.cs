using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPlatformerController : PhysicsObject
{
    /*
     * ###############################
     * # VARIABLE DECLRARTION BEGINS # 
     * ###############################
     */
    //max running speed
    public float maxSpeed = 7;

    //max jumping speed
    public float jumpTakeOffSpeed = 7;

    //the bullet game object
    public GameObject bullet;

    //the bullet fire rate
    public float fire_rate = 0.5f;

    //the offset for the position where the bullet will be generated
    private Vector2 offset = new Vector2(0.7f, 0f);

    //sprite renderer of the player
    private SpriteRenderer spriteRenderer;

    //animator of the player
    private Animator animator;

    //boolean which represent if the weapon (machine) has been pull out
    private bool weapon;

    //the positon where the bullet will be generated
    private Vector2 bullet_position;

    // determine when the next bullet can be fired
    private float next_fire = 0.0f;

    // boolean which represents whether the player has fliped or not
    private bool flipSprite;

    //the player movement
    private Vector2 move;

    //the player health
    private int health;

    //Score: the total number of cheese collected
    private int score;

    //health text
    public Text health_text;

    //cheese text
    public Text cheese_text;

    /*
     * #############################
     * # VARIABLE DECLRARTION ENDS # 
     * #############################
     */

    public Vector2 Offset
    {
        get
        {
            return offset;
        }

        set
        {
            offset = value;
        }
    }

    // Use this for initialization
    void Awake()
    {
        //get the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //get the animator
        animator = GetComponent<Animator>();

        //initialize the player health to 100
        health = 100;

        //initialize the score to 0
        score = 0;

        //initialize the health text to 100
        health_text.text = "HEALTH : " + health.ToString();

        //initialize the cheese score to 0
        cheese_text.text = "CHEESE : " + score.ToString();
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

        //if the player is still alive
        if (health > 0)
        {
            /*
         * ####################################
         * # Switch the weapon to machine gun #
         * ####################################
         */
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
                if (weapon)
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
                if (weapon)
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
                if (weapon)
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
                if (weapon)
                {
                    animator.SetInteger("State", 6);
                }
                else animator.SetInteger("State", 0);
            }

            /*
             * ################
             * # Pull the Gun #
             * ################
             */
            if (Input.GetMouseButtonDown(0) && Time.time > next_fire)
            {
                animator.SetInteger("State", 5);
                next_fire = Time.time + fire_rate;
                Fire();

            }
            else if (Input.GetMouseButtonUp(0))
            {
                animator.SetInteger("State", 0);
            }

            //move 
            targetVelocity = move * maxSpeed;
        }

        //if no health, game ends
        else
        {
            //play die animation
            animator.SetInteger("State", 10);

        }
    }

    /*
     * #####################
     * # The fire function #
     * #####################
     */
    void Fire()
    {
        bullet_position = transform.position;

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

    /*
     * ######################
     * # Collision Function #
     * ######################
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if get hit by a bullet
        if (collision.gameObject.tag == "BlobBullet" || collision.gameObject.tag == "Spider" && health >= 0)
        {
            health -= 10;
            animator.SetInteger("State", 3);
            health_text.text = "HEALTH : " + health.ToString();
        }

        //if collide with a piece of yum yum cheese, score + 1
        else if (collision.gameObject.tag == "Cheese")
        {
            score += 1;
            cheese_text.text = "CHEESE : " + score.ToString();
        }
    }

    /*
     * ##############################
     * # Collision leaving function #
     * ##############################
     */
    private void OnCollisionExit2D(Collision2D collision)
    {

        if ( (collision.gameObject.tag == "BlobBullet" || collision.gameObject.tag == "Spider") && health != 0)
        {
            animator.SetInteger("State", 0);
        }
    }
}