using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;

    public float speed;
    public float jumpForce;

    [Header("Grounding")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [Header("Hold To Jump Settings")]
    public float maxJumpTime = .3f;
    public float holdForce = 3;
    private bool isJumping;
    private float jumpTimeCounter;

    public float deceleration = 40f;
    private float previousHorizontal;
    private float stopVelocityThreshold = 2f;

    private int facingDirection = 1;
    private float horizontal;
    
    // Update is called once per frame
    void Update()
    {
        //Movement
        horizontal = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetBool("IsGrounded", IsGrounded());
        anim.SetFloat("VerticalVelocity", rb.linearVelocity.y);


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

        // Continue Jumping
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.AddForce(Vector2.up * holdForce, ForceMode2D.Force); // zamiast nadpisywać velocity
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

        // Stop animation detection
        bool isStopping = false;

        if (IsGrounded())
        {
            if (Mathf.Abs(previousHorizontal) > 0.1f && Mathf.Abs(horizontal) < 0.1f && Mathf.Abs(rb.linearVelocity.x) > stopVelocityThreshold)
            {
                isStopping = true;
            }
        }

        anim.SetBool("IsStopping", isStopping);

        previousHorizontal = horizontal;
            
    }
    //Runs exactly 50 times per second
    /*void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }*/
    void FixedUpdate()
    {
        float targetSpeed = horizontal * speed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? speed : deceleration;

        float movement = speedDiff * accelRate * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);
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
