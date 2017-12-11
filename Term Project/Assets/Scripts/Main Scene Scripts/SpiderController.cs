using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : PhysicsObject
{
    //the target to chase, which is the player
    public GameObject target;

    //maximum speed 
    public float maxSpeed = 10f;

    //take off speed (jump)
    private float jumpTakeOffSpeed = 10f;

    //the player movement
    private Vector2 move;

    //if the spider is fliped or not 
    bool flipSprite;

    //sprite renderer of the Spider
    private SpriteRenderer spriteRenderer;

    //turn rate
    float turn_rate;

    //next time to turn
    float time_to_turn;

    //the x distance between spider and target
    float distance_x;

    //the y distance between spider and target
    float distance_y;

    //the threshold which spiders will start to chase the player
    float threshold = 7f;

    //the boolean which represnets if the player is on the right side of the spider
    bool right;

    //the array contains all block
    GameObject[] objects;

    //the animator
    private Animator animator;

    // Use this for initialization
    void Start () {
        
	}
    void Awake()
    {
        //get the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //when the spider is ideling, this is the rate which spider will turn
        turn_rate = 2f;

        //the initial direction of the spider's movement
        move = new Vector2(0.1f, 0);

        //count when to turn
        time_to_turn = 0;

        //initialize both distance to 0
        distance_x = distance_y = 0f;

        //the spider is facing right initially
        right = true;

        //find all blocks
        objects = GameObject.FindGameObjectsWithTag("Block");

        //get the animator
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

        //get the distance on x axis between the player and the spider
        distance_x = target.transform.position.x - transform.position.x;

        //get the distance on y axis between the player and the spider
        distance_y = target.transform.position.y - transform.position.y;

        //if close enough
        if(Mathf.Abs(distance_x) <= threshold && Mathf.Abs(distance_y) <= threshold)
        {
            //spider speed up
            maxSpeed = 20;

            //Find the path
            FindThePath();
        }

        //if out of range, abck to idel
        else
        {
            //spider slow down
            maxSpeed = 10;

            //time to turn
            if (Time.time > time_to_turn)
            {
                //turn
                move.x *= -1;

                //computer the time turn time
                time_to_turn = Time.time + turn_rate;

                //face the opposite direction
                spriteRenderer.flipX = !spriteRenderer.flipX;

                //change the boolean
                right = !right;
            }
        }   
        //move 
        targetVelocity = move * maxSpeed;

    }


    void FindThePath()
    {
        //if the player and the spider is at the same level
        if(distance_y <= 1)
        {
            //if player is on the left, same level with the spider
            if (distance_x < 0 && right)
            {
                //turn left
                move.x *= -1;

                //face left
                spriteRenderer.flipX = !spriteRenderer.flipX;

                //on the left
                right = false;
            }

            //if player is on the right, but spider is moving (facing) left
            else if (distance_x > 0 && !right)
            {
                //turn right
                move.x *= -1;

                //face right
                spriteRenderer.flipX = !spriteRenderer.flipX;

                //on the right
                right = true;
            }
        }

        //at different level
        else
        {
            //the best block to jump on
            GameObject best_block = null;

            //infinity
            float mini_distance = Mathf.Infinity;
            
            //spider's current position
            Vector2 spider_position = transform.position;

            //player's current position
            Vector2 player_position = target.transform.position;

            //The distance between the block and the spider (weight)
            Vector2 spider_block_vector;

            //The distance betweenn the block and the player (admissible heuristic) 
            Vector2 player_block_vector;

            //higher?
            bool higher = false;

            if(player_position.y > spider_position.y)
            {
                higher = true;
            }

            //w + h
            float distance;

            foreach (GameObject o in objects)
            {
                spider_block_vector = new Vector2(o.transform.position.x - spider_position.x, o.transform.position.y - spider_position.y);
                player_block_vector = new Vector2(player_position.x - o.transform.position.x, player_position.y - o.transform.position.y);
                distance = spider_block_vector.SqrMagnitude() + player_position.SqrMagnitude();

                if(distance < mini_distance && higher && o.transform.position.y > spider_position.y)
                {
                    best_block = o;
                    mini_distance = distance;
                }
            }//end foreach

            if (best_block.transform.position.x - spider_position.x < -2 && right)//6
            {
                //turn left
                move.x *= -1;

                //face left
                spriteRenderer.flipX = !spriteRenderer.flipX;

                //on the left
                right = false;
            }
            else if (best_block.transform.position.x - spider_position.x > 2 && !right)//6
            {
                //turn right
                move.x *= -1;

                //face right
                spriteRenderer.flipX = !spriteRenderer.flipX;

                //on the right
                right = true;
            }

            if (Mathf.Abs(best_block.transform.position.x - spider_position.x) < 2)//5
            {

                velocity.y = jumpTakeOffSpeed;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
 
            }

        }//end else

    }//end FindThePath()

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            animator.SetInteger("State", 1);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            animator.SetInteger("State", 0);
        }
    }

}//end class
