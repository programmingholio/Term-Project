using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmenyPlatformerController : PhysicsObject
{
    /*
     * ###############################
     * # VARIABLE DECLRARTION BEGINS # 
     * ###############################
     */
    //get the player object
    public GameObject target;

    //the variable which holds the distance between the enemy and the player
    private float distance;
    
    //the animator
    private Animator animator;

    //the sprite renderer
    private SpriteRenderer spriteRenderer;

    //boolean variable which indicates if the player is on the right side of the enemy
    bool right = true;

    //enemy health
    int health;

    //enemy jump off speed
    float jumpTakeOffSpeed = 9;

    //bullet
    public GameObject bullet;

    //the positon where the bullet will be generated
    private Vector2 bullet_position;

    //the offset for the position where the bullet will be generated
    private Vector2 offset = new Vector2(0.7f, 0f);

    //the bullet fire rate
    public float fire_rate = 10f;

    // determine when the next bullet can be fired
    private float next_fire = 0.0f;

    /*
     * #############################
     * # VARIABLE DECLRARTION ENDS # 
     * #############################
     */

    // Use this for initialization
    void Start () {
        health = 100;
	}
    void Awake()
    {
        //get the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        //get the animator
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        //compute the disance between the enemy and the player
        distance = target.transform.position.x - transform.position.x;

        //if player is on the right, but enemy is facing left
        if(distance < 0 && !right)
        {
            right = true;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            BlobBulletScript.blob_velx *= -1;
        }

        //if player is on the left, but enemy is facing right
        else if (distance > 0 && right)
        {
            right = false;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            BlobBulletScript.blob_velx *= -1;
        }

        //the threshold which enemy will attack
        if(health != 0)
        {
            if (Mathf.Abs(distance) <= 3 && Time.time > next_fire)
            {
                animator.SetInteger("State", 2);
                next_fire = Time.time + fire_rate;
                Fire();
            }
            else
            {
                animator.SetInteger("State", 0);
            }
        }
        

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if get hit by a bullet
        if(collision.gameObject.tag == "Bullet")
        {
            health -= 25;
            if(health == 0)
            {
                animator.SetInteger("State", 10);
            }
        }

        //remove the enemy object when died
        else if(collision.gameObject.tag == "Player" && health == 0)
        {
            Destroy(gameObject);
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

        //flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (right)
        {
            bullet_position += offset * -1;
        }
        else
        {
            bullet_position += offset;
        }

        Instantiate(bullet, bullet_position, Quaternion.identity);
    }

}
