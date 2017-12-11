using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block1 : PhysicsObject
{
    //store the corresponding weapon
    public GameObject weapon;

    //the positon where the weapon will be generated
    private Vector2 weapon_position;

    //the offset for the position where the weapon will be generated
    private Vector2 offset = new Vector2(0, 1.5f);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            weapon_position = transform.position;
            Instantiate(weapon, weapon_position + offset, Quaternion.identity);
        }
    }

}
