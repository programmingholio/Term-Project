using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmenyPlatformerController : PhysicsObject
{
    public GameObject target;
    private float distance;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    bool right = true;
    // Use this for initialization
    void Start () {
		
	}
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update () {
        distance = target.transform.position.x - transform.position.x;
        if(distance < 0 && !right)
        {
            right = true;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        else if (distance > 0 && right)
        {
            right = false;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if ( Mathf.Abs(distance) <= 2)
        {
            animator.SetInteger("State", 2);
        }
        else
        {
            animator.SetInteger("State", 0);
        }

    }
}
