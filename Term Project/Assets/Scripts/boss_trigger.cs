using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_trigger : MonoBehaviour {
    public GameObject  BOSS, boss_light1, boss_light2, boss_light3, boss_light4, boss_music, music, bomb1,
           bomb2, bomb3, invis_floor;
    public Camera Camera;
    void OnTriggerEnter2D (Collider2D other){
        boss_battle();
    }

    void boss_battle(){
        Wait(2, () => {
            BOSS.SetActive(true);
            boss_music.SetActive(true);
            bomb1.SetActive(true);
            bomb2.SetActive(true);
            bomb3.SetActive(true);
            Camera.main.fieldOfView = 60.0f;
        });

        invis_floor.SetActive(false);
        music.SetActive(false); 
        
        boss_light1.SetActive(true);
        boss_light2.SetActive(true);
        boss_light3.SetActive(true);
        boss_light4.SetActive(true);



    }
     public void Wait(float seconds, Action action){
         StartCoroutine(_wait(seconds, action));
     }
     IEnumerator _wait(float time, Action callback){
         yield return new WaitForSeconds(time);
         callback();
     }

}
