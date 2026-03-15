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
