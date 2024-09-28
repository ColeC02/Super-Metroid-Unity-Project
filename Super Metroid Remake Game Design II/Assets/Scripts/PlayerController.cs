using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 5.0f;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckCircle;
    private float InputDirection;

    private bool isRunning;
    private bool isFacingRight = true;
    private bool isGrounded;

    private Rigidbody2D rb;
    private Animator animator;

    public float jumpForce = 8.0f;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    private bool canJump;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckifCanJump();

        if (rb.velocity.y < 0)
        {
            rb.velocity += UnityEngine.Vector2.up * Physics2D.gravity.y * (fallMultiplier -1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += UnityEngine.Vector2.up * Physics2D.gravity.y * (fallMultiplier -1) * Time.deltaTime;
        }
        
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckEnviroment();
    }

    private void CheckInput()
    {
        InputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Jump() 
    { if (canJump)
    {
        rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpForce);
    }
}

    private void CheckifCanJump()
    {
        if(isGrounded && rb.velocity.y <= 3)
        {
            canJump = true;
        }
    }


    private void ApplyMovement()
    {
        rb.velocity = new UnityEngine.Vector2(movementSpeed * InputDirection, rb.velocity.y);
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && InputDirection < 0){
            Flip();
        }
        else if (!isFacingRight && InputDirection > 0)
        {
            Flip();
        }

        if (rb.velocity.x <= -0.5f | rb.velocity.x >= 0.5f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private  void UpdateAnimation()
    {
        animator.SetBool("isRunning",isRunning);
        animator.SetBool("isGrounded",isGrounded);
    } 


    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void CheckEnviroment()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckCircle, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckCircle);
    }
}
