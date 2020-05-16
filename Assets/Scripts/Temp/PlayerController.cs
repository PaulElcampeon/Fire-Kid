using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private float speed = 9;

    [SerializeField]
    private float walkAcceleration = 75;

    [SerializeField]
    private float groundDeceleration = 70;

    [SerializeField]
    private float airAcceleration = 30;


    [SerializeField]
    private float jumpHeight = 15;

    [SerializeField]
    private LayerMask groundLayerMask;

    private Animator animator;
    private BoxCollider2D boxCollider;
    private Vector2 velocity;
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    private float movementInput;
    private bool hasGroundDetectionBeenReset = true;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        PlayerInput();

        SimulateMovement();

        Flip(movementInput);

        if (!isGrounded) CheckIfHeadIsHittingPlatform();

        if (!isGrounded) SimulateGravity();

        transform.Translate(velocity * Time.deltaTime);

        GroundPlayer();
    }

    private void PlayerInput()
    {
        movementInput = Input.GetAxisRaw("Horizontal");

        isJumping = Input.GetKeyDown(KeyCode.Space);
    }

    private void SimulateMovement()
    {
        if (movementInput !=0)
        {
            if (!isGrounded) velocity.x = 0;
            if (isGrounded) velocity = Vector2.zero;
            if (isGrounded) animator.SetBool("isMoving", true);
            if (movementInput < 0) velocity.x -= speed * movementInput;
            if (movementInput > 0) velocity.x += speed * movementInput;
        } else
        {
            velocity.x = 0;
            
            animator.SetBool("isMoving", false);
        }

        if (isGrounded) SimulateJump();

        transform.Translate(velocity * Time.deltaTime);

    }

    private void SimulateJump()
    {
        if (isJumping)
        {
            isGrounded = false;
            StartCoroutine(ResetGroundDetection());
            animator.SetTrigger("Jump");
            velocity.y = 0;
            velocity.y += jumpHeight;
        }
    }

    private void SimulateFall()
    {
        if (velocity.y < 0) animator.SetBool("isFalling", true);
    }

    private void SimulateGravity()
    {

        if (isFalling)
        {
            velocity.y -= airAcceleration * Time.deltaTime;
        }
        else
        {
            velocity.y += /*Physics2D.gravity.y*/ -40f * Time.deltaTime;
        }

        SimulateFall();
    }

    private void Flip(float movementInput)
    {
        if (movementInput < 0) transform.rotation = Quaternion.Euler(0, 180, 0);

        if (movementInput > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void CheckIfHeadIsHittingPlatform()
    {
        RaycastHit2D hit1;

        hit1 = Physics2D.Raycast(transform.position, Vector2.up, 0.9f, groundLayerMask);

        Debug.DrawRay(transform.position, Vector2.up * 0.9f, Color.blue);

        if (hit1)
        {
            animator.SetBool("isFalling", true);
            isJumping = false;
            isFalling = true;
            velocity.y = 0;
        }
    }

    private void GroundPlayer()
    {
        if (!hasGroundDetectionBeenReset) return;

        RaycastHit2D hit1;

        hit1 = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayerMask);

        Debug.DrawRay(transform.position, Vector2.down * 1.1f, Color.red);

        if (hit1)
        {
            float topOfGroundCollider = hit1.collider.bounds.max.y;
            float rayCastMinimalYPoint = transform.position.y - 1.1f;

            if (topOfGroundCollider - rayCastMinimalYPoint > 0.1f) transform.Translate(new Vector2(0f, topOfGroundCollider - rayCastMinimalYPoint));
            animator.SetBool("isFalling", false);
            isFalling = false;
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }
    }

    private IEnumerator ResetGroundDetection()
    {
        hasGroundDetectionBeenReset = false;

        yield return new WaitForSeconds(0.2f);

        hasGroundDetectionBeenReset = true;
    }
}
