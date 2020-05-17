using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public float speed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpTakeOffSpeed;
            animator.SetTrigger("Jump");

        } else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0) velocity.y = velocity.y * 0.5f;
            //animator.SetBool("isFalling", true);
        }

        if (move.x != 0)
        {
            animator.SetBool("isMoving", true);

            bool flipSprite = (spriteRenderer.flipX ? (move.x > 0) : (move.x < 0));

            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        } else
        {
            animator.SetBool("isMoving", false);
        }

        if (velocity.y <= 0) animator.SetBool("isFalling", true);

        if (isGrounded) animator.SetBool("isFalling", false);

        targetVelocity = move * speed;
    }
}
