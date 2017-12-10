using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlobMinionController : PhysicsObject {
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

    Transform player; 
    NavMeshAgent nav;

    public GameObject blob_this;

    
    // Use this for initialization
    void Start () {
        
	}
    void Awake()
    {
        //Finds where the player is.
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //nav = GetComponent <NavMeshAgent>();

        //get the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        turn_rate = 5f;
        move = new Vector2(0.1f, 0);
        time_to_turn = 0;
    }

    // Update is called once per frame
    void Update () {
        //Start moving towards player. 
        //nav.SetDestination (player.position);
        if (player.position[0] > blob_this.transform.position[0]) {
            move.x +=0.1f;
        }
        else {
            move.x -=0.1f;
        }
        //Flips depending on movement.
       flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x <-0.01f));
       if (flipSprite){
           spriteRenderer.flipX = !spriteRenderer.flipX;
       }
        //move 
        targetVelocity = move * maxSpeed;

    }
}
