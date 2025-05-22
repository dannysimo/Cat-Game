using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator animator;
    private bool isDodging = false;
    private float dodgeTime;
    private bool facingRight = true;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        bool isGrounded = IsGrounded();

        
        float moveInput = Input.GetAxis("Horizontal");

        
        if (!isDodging)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }

       
        if (Input.GetButtonDown("Fire3") && !isDodging) 
        {
            StartDodge(moveInput);
        }

        
        if (isDodging && Time.time > dodgeTime)
        {
            EndDodge();
        }

       
        animator.SetBool("isGrounded", isGrounded);
    }

    private bool IsGrounded()
    {
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private void StartDodge(float moveInput)
    {
        isDodging = true;
        dodgeTime = Time.time + dodgeDuration;
        rb.velocity = new Vector2(moveInput * dodgeSpeed, rb.velocity.y);
        animator.SetTrigger("Dodge");
    }

    private void EndDodge()
    {
        isDodging = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}