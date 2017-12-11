using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class boss_controller : MonoBehaviour {

    //Public variables
        public int forceattack_speed;
        public GameObject player;

    //Player Variables.
        Vector3 player_pos;
    //
    public GameObject fireball;
        
    //Boss Variables
        Transform boss_transform;
        Rigidbody2D boss_rb2d;
        Vector2 current_position;
        //Is boss available to move?
        bool available;
        //BOSS HEALTH
        int boss_health;

    //Breadcrumb container.
        private Queue<Vector3> breadcrumb;
    //Tracked frames.
        double i;



	// Use this for initialization
	void Start () {
        //Instantiate breadcrumb ai.
        breadcrumb = new Queue<Vector3>();

        player_pos = player.transform.position;
        breadcrumb.Enqueue(player_pos);

        //Instantiate framecounter.
        i = 0;

       boss_transform = GetComponent<Transform>();
       boss_rb2d = GetComponent<Rigidbody2D>();
       available = true;
       boss_health = 5;
		
	}
	
	// Update is called once per frame
	void Update () {
       //Add player position ot the queue. 
       player_pos = player.transform.position;
       breadcrumb.Enqueue(player_pos);
       //Update frame counter every frame.
       i++;

       //After a certain point, allow attacks to begin.
       if (i > 15 && i % 198 == 0 && available){
            available = false;
            BeginAttack(i);

            Wait(6, () => {
                available = true;
            });
       } 
       else if(i > 15){
           //Pop a position if over 15.
            current_position = breadcrumb.Dequeue();                

            //If available, move ghost.
            if (available){
               boss_rb2d.MovePosition(current_position);
            }
           
       }

       //If Boss runs out of health, game over man, game over!
       if (boss_health < 0){
            GetComponent<Animator>().SetBool("Death", true);
            Wait(3, () => {
                SceneManager.LoadScene("Scene", LoadSceneMode.Additive);
            });
       }

    }

    /** Used in the main frame update function:
     * When called, this function instantiates one of
     * the three ghost attack functions:
     *  Fire: Shoot fireballs everywhere.
     *  Force: Project a wave of force, to blow rat away.
     *  Flying: Fly around, and collide with rat.
     */
    void BeginAttack(double frame){
        int choice = UnityEngine.Random.Range(1, 4);

        Debug.Log(choice);
        switch (choice){
            case 1:
                GetComponent<Animator>().SetBool("Attack", true);
                Wait(1, () => {

                   FireAttack(frame); 
                });
               break;
            case 2:
                GetComponent<Animator>().SetBool("Attack",true);
                Wait(1, () => {

                   ForceAttack(frame);
                });
              break;
            case 3:
                Wait(1, () => {
                   FlyingAttack(frame);
                });
             break; 
        
        }
    }
    void FireAttack(double frame){
        Vector3[] landing = new Vector3[3];

        //For 3 of the upper platforms:
        for (int i = 0; i < 3; i++){

            //A landing value is randomly selected.
            switch ((int) UnityEngine.Random.Range(1,5)){
                case 1:
                    landing[i] = new Vector3(35.9f, 15.02f, 0f);
                    break;
                case 2:
                    landing[i] = new Vector3(40f, 15.24f, 0f);
                    break;
                case 3:
                    landing[i] = new Vector3(44.5f, 15.54f, 0f);
                    break;
                case 4:
                    landing[i] = new Vector3(49.06f, 15.52f, 0f);
                    break;
                case 5:
                    landing[i] = new Vector3(54.86f, 15.43f, 0f);
                    break;
              } 


              //A fireball is sent hurtling down.
            Instantiate(fireball, landing[i], Quaternion.identity);

            //And by that, I mean, REALLY hurtling down.
            Rigidbody2D fb_rb2d = fireball.GetComponent<Rigidbody2D>();
            fb_rb2d.AddForce(new Vector2(0f, -20f), ForceMode2D.Force);

        }

    }
    void ForceAttack(double frame){
        //ALMIGHTY PUSH
        Rigidbody2D p_rb2d = player.GetComponent<Rigidbody2D>();

        if( player_pos.x < current_position.x){
            //Apply substantial amount of force up, and to the left.
            p_rb2d.AddForce(new Vector2(-.5f * forceattack_speed, 1f * forceattack_speed), ForceMode2D.Force); 
        }    
        else if (player_pos.x < current_position.x){
            //Apply substantial amount of force up and to the right. 
            p_rb2d.AddForce(new Vector2(.5f * forceattack_speed, 1f * forceattack_speed), ForceMode2D.Force); 
        }
        //IF EQUAL - Keep on your toes!
        else{
            //TODO:Damage player
           
        }

    }
    void FlyingAttack(double frame){
        int direction = UnityEngine.Random.Range(1,2) ; 
        float dir;
       //Teleport to either bottom right or bottom left. 
            switch (direction){
                case 1:
                    boss_transform.Translate( new Vector3 (66f,-5f,0f), Space.World); 
                    dir = -1;
                    break;
                case 2:
                    boss_transform.Translate( new Vector3 (31f,-5,0f), Space.World); 
                    dir = 1;
                    break;
                default:
                    dir = 0;
                    break;    
            }
            
            GetComponent<Animator>().SetBool("Move", true);

            //Fly across the screen, if hitbox is entered by rat, apply 10 dmg.
            boss_rb2d.AddForce(new Vector2(dir * forceattack_speed*10, 0f), ForceMode2D.Force); 

            //Enables hitbox
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.enabled = true;
            Wait(3, () => {
                    //Disables after 1 second.
                    boss_rb2d.velocity = Vector2.zero;
                    box.enabled = false;
            });
       
    }



    //Sleep function -- Waits seconds while allowing program to continue.
     public void Wait(float seconds, Action action){
         StartCoroutine(_wait(seconds, action));
     }
     IEnumerator _wait(float time, Action callback){
         yield return new WaitForSeconds(time);
         callback();
     }
}

