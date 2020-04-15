using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float movementSmoothing = 0;
    public Transform topLadderCheck;
    public Vector2 topLadderCheckSize;

    private bool onLadder = false;
    private bool checkLadderAbove = false;
    private bool isThereLadderAbove = false;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Vector3 refVelocity = Vector3.zero;
    private float gravityScale = 1;
    private SpriteRenderer spriteRenderer;
    private int ladderLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gravityScale = rb.gravityScale;
        ladderLayer = LayerMask.NameToLayer("Ladder");
    }

    private void FixedUpdate()
    {
        if (checkLadderAbove)
        {
            if (Physics2D.OverlapBox(topLadderCheck.position, topLadderCheckSize, 0, 1 << ladderLayer))
            {
                isThereLadderAbove = true;
            }
            else
            {
                isThereLadderAbove = false;
            }

            checkLadderAbove = false;
        }
    }

    public void Move(float move, float ladderMove)
    {

        float xVelocity = move * 10f;
        float yVelocity = rb.velocity.y;

        if (ladderMove > 0 && !isThereLadderAbove) ladderMove = 0; // If there is no ladder above, disable up movement (removes a glitch)
        if (ladderMove < 0 && !isThereLadderAbove) checkLadderAbove = true;

        if (onLadder)
        {
            yVelocity = ladderMove * 10f / gravityScale;
        }

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(xVelocity, yVelocity);
        // And then smoothing it out and applying it to the character
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref refVelocity, movementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !facingRight)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && facingRight)
        {
            spriteRenderer.flipX = true;
            facingRight = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == ladderLayer) {
            rb.gravityScale = 0;
            onLadder = true;
            checkLadderAbove = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == ladderLayer) {
            rb.gravityScale = gravityScale;
            onLadder = false;
            checkLadderAbove = true;
        }
    }

    /*
    public void DeathJump()
    {
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        enabled = false;
    }*/
}