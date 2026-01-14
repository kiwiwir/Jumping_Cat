using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpForce;

    [Header("Grounding")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [Header("Holt To Jump Settings")]
    public float maxJumpTime = .3f;
    public float holdForce = 3;
    private bool isJumping;
    private float jumpTimeCounter;

    private int facingDirection = 1;
    private float horizontal;
    
    // Update is called once per frame
    void Update()
    {
        //Movement
        horizontal = Input.GetAxisRaw("Horizontal");

        if(horizontal > .1f && facingDirection < 0 || horizontal < -.1f && facingDirection > 0)
        {
            Flip();
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        //Countinue Jumping
        if(Input.GetButton("Jump") && isJumping == true)
        {
            if(jumpTimeCounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, holdForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        //Stop Jumping
        if(Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
            
    }
    //Runs exactly 50 times per second
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Flip()
    {
        facingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
