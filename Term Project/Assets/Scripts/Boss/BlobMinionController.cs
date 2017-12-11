using System.Collections;
using System;
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

    float health; 

    Transform player; 
    NavMeshAgent nav;

    public GameObject blob_this;

    void OnTriggerEnter2D (Collider2D other){

        //if get hit by a bullet
        if (other.gameObject.tag == "Bullet" && health >= 0)
        {
            health -= 10;
            GetComponent<Animator>().SetBool("Damage",true);
        }
        else if(other.gameObject.tag == "Player" && health >= 0){
            GetComponent<Animator>().SetBool("Punch",true);
        }
    }
    
    // Use this for initialization
    void Start () {
        health = 40;
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
        if(health <= 0){
            GetComponent<Animator>().SetBool("Dead",true);
            Wait(1, ()=> {
                blob_this.SetActive(false);
            });
        }
        //Start moving towards player. 
        //nav.SetDestination (player.position);
        if (player.position[0] > blob_this.transform.position[0]) {
            move.x +=0.1f;
        }
        else {
            move.x -=0.1f;
        }

        //Flips depending on movement.
        if (move.x > 0.01f) {
            spriteRenderer.transform.eulerAngles = new Vector3(
                spriteRenderer.transform.eulerAngles.x,
                180,
                spriteRenderer.transform.eulerAngles.z
            );
        }
        else if (move.x < -0.01f){
            spriteRenderer.transform.eulerAngles = new Vector3(
                spriteRenderer.transform.eulerAngles.x,
                0,
                spriteRenderer.transform.eulerAngles.z
            );
        }

        //move 
        targetVelocity = move * maxSpeed;
        GetComponent<Animator>().SetBool("Move",true);

    }

     public void Wait(float seconds, Action action){
         StartCoroutine(_wait(seconds, action));
     }
     IEnumerator _wait(float time, Action callback){
         yield return new WaitForSeconds(time);
         callback();
     }
}
