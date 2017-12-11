using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

/* potion_get.cs
 * This quick script, applied to a potion game object,
 * restores the rat's health to full, plays a tune,
 * and then promptly kills it's self.
 */

[RequireComponent(typeof(AudioSource))]
public class potion_get: MonoBehaviour
{
    public GameObject Potion;

    void Start()
    {
    }

    void OnTriggerEnter2D (Collider2D other){
        //Play sound
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.Play(44100);
        //FULL RESTORE MOUSE HERE
        
        //Deactivate game object.
        Wait(5, () => {
            Potion.SetActive(false);
        });
    }

     public void Wait(float seconds, Action action){
         StartCoroutine(_wait(seconds, action));
     }
     IEnumerator _wait(float time, Action callback){
         yield return new WaitForSeconds(time);
         callback();
     }
}
