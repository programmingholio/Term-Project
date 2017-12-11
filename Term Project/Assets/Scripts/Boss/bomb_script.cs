using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class bomb_script : MonoBehaviour {

    public GameObject Bomb;

    void OnTriggerEnter2D (Collider2D other){
        GetComponent<Animator>().SetBool("Explode",true);
    }

     public void Wait(float seconds, Action action){
         StartCoroutine(_wait(seconds, action));
     }
     IEnumerator _wait(float time, Action callback){
         yield return new WaitForSeconds(time);
         callback();
     }

	// Use this for initialization
	void Start () {
        
	}

	// Update is called once per frame
	void Update () {
         if(Bomb.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_exploding")){
           Wait(.4f, () => {
                Bomb.SetActive(false);
           }); 
         }
	    	
	}
}
