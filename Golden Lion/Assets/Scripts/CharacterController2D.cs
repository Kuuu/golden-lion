using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private Transform m_TopLadderCheck;
    [SerializeField] private LayerMask m_LadderLayer;

    private bool onLadder = false;
    private bool checkLadderAbove = false;
    private bool isThereLadderAbove = false;
    const float topLadderCheckRadius = 0.02f; // Radius of the overlap circle to determine if there is ladder above
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;
    private float gravityScale = 1;
    public Vector2 topLadderCheckSize;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        gravityScale = m_Rigidbody2D.gravityScale;
    }

    private void FixedUpdate()
    {
        if (checkLadderAbove)
        {
            if (Physics2D.OverlapBox(m_TopLadderCheck.position, topLadderCheckSize, 0, m_LadderLayer))
            {
                //Debug.Log("LADDER ABOVE!");
                isThereLadderAbove = true;
            }
            else
            {
                //Debug.Log("NO LADDER!");
                isThereLadderAbove = false;
            }

            checkLadderAbove = false;
        }
    }

    public void Move(float move, float ladderMove)
    {

        float xVelocity = move * 10f;
        float yVelocity = m_Rigidbody2D.velocity.y;

        if (ladderMove > 0 && !isThereLadderAbove) ladderMove = 0; // If there is no ladder above, disable up movement (removes a glitch)
        if (ladderMove < 0 && !isThereLadderAbove) checkLadderAbove = true;

        if (onLadder)
        {
            yVelocity = ladderMove * 10f / gravityScale;
        }

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(xVelocity, yVelocity);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
                // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (LayerMask.LayerToName(col.gameObject.layer) == "Ladder") { m_Rigidbody2D.gravityScale = 0; onLadder = true; checkLadderAbove = true; }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (LayerMask.LayerToName(col.gameObject.layer) == "Ladder") { m_Rigidbody2D.gravityScale = gravityScale; onLadder = false; checkLadderAbove = true; }
    }

    /*
    public void DeathJump()
    {
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        enabled = false;
    }*/
}