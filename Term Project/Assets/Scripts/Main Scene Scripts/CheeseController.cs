using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseController : PhysicsObject {

    // Update is called once per frame
    void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collide with player
        if (collision.gameObject.tag == "Player")
        {            
            Destroy(gameObject);
        }
    }
}
