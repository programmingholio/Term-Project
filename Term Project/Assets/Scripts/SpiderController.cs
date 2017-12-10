using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : PhysicsObject
{
    //maximum speed 
    public float maxSpeed = 0.1f;

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

    // Use this for initialization
    void Start () {
        
	}
    void Awake()
    {
        //get the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        turn_rate = 5f;
        move = new Vector2(0.1f, 0);
        time_to_turn = 0;
    }

    // Update is called once per frame
    void Update () {
     
        if(Time.time > time_to_turn)
        {
            move.x *= -1;
            time_to_turn = Time.time + turn_rate;

        }

       flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
       if (flipSprite)
       {
           spriteRenderer.flipX = !spriteRenderer.flipX;
       }
        //move 
        targetVelocity = move * maxSpeed;

    }
}
