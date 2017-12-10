using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobBulletScript : MonoBehaviour
{

    //The speed of the bullet

    public static float blob_velx = -2f;
    float vely = 0;

    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(blob_velx, vely);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
