using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private Transform m_TopLadderCheck;
    [SerializeField] private LayerMask m_LadderLayer;
    public Vector2 topLadderCheckSize;
    private Vector3 m_Velocity = Vector3.zero;
    public Transform target;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    public float ladderJump = 2f;
    public float speed = 40f;
    public float ladderSpeed = 40f;
    private float movement = 0f;
    private float ladderMovement = 0f;
    private bool onLadder = false;
    Vector2 direction;
    private float gravityScale = 1;

    public float nextWaypointDistance = 3f;

    private Vector2 currentWaypointVector;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        currentWaypointVector = path.vectorPath[currentWaypoint];


        direction = (currentWaypointVector - rb.position).normalized;
        movement = direction.x;
        ladderMovement = direction.y;

        Move(movement * speed * Time.fixedDeltaTime, ladderMovement * ladderSpeed * Time.fixedDeltaTime);

        float distance = Vector2.Distance(rb.position, currentWaypointVector);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public void Move(float move, float ladderMove)
    {

        float xVelocity = move * 10f;
        float yVelocity = rb.velocity.y;

        if (onLadder && ladderMove != 0)
        {
            yVelocity = ladderMove * 10f / gravityScale;

            if (yVelocity > 0 && !Physics2D.OverlapBox(m_TopLadderCheck.position, topLadderCheckSize, 0, m_LadderLayer))
            {
                Debug.Log("Power JUMP!");
                yVelocity *= ladderJump; // Do A Jump
            }
        }
        // Move the character by finding the target velocity
        Vector2 targetVelocity = new Vector2(xVelocity, yVelocity);

        // And then smoothing it out and applying it to the character
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        /*
        if (preMovePosition == (Vector2)transform.position)
        {
            Debug.Log("Got Stuck!");
            isStuck = true;
        }*/

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
        if (LayerMask.LayerToName(col.gameObject.layer) == "Ladder") { rb.gravityScale = 0; onLadder = true; }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (LayerMask.LayerToName(col.gameObject.layer) == "Ladder") { rb.gravityScale = gravityScale; onLadder = false; }
    }
}
