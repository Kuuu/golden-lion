using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float movementSmoothing = 0;
    public Transform topLadderCheck;
    public Vector2 topLadderCheckSize;

    public float ladderJump = 2f;
    public float speed = 40f;
    public float ladderSpeed = 40f;

    private Vector3 refVelocity = Vector3.zero;
    private bool facingRight = true;
    private SpriteRenderer spriteRenderer;

    private float movement = 0f;
    private float ladderMovement = 0f;
    private bool onLadder = false;
    Vector2 direction;
    private float gravityScale = 1;
    private int ladderLayer;

    public float nextWaypointDistance = 3f;

    private Vector2 currentWaypointVector;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    private float pathUpdateRate = 0.5f;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ladderLayer = LayerMask.NameToLayer("Ladder");
        gravityScale = rb.gravityScale;
        InvokeRepeating("UpdatePath", 0f, pathUpdateRate);
        
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

    private void Move(float move, float ladderMove)
    {

        float xVelocity = move * 10f;
        float yVelocity = rb.velocity.y;

        if (onLadder && ladderMove != 0)
        {
            yVelocity = ladderMove * 10f / gravityScale;
            /*
            if (yVelocity > 0 && !Physics2D.OverlapBox(topLadderCheck.position, topLadderCheckSize, 0, 1 << ladderLayer))
            {
                Debug.Log("Power JUMP!");
                yVelocity *= ladderJump; // Do A Jump
            }*/
        }
        // Move the character by finding the target velocity
        Vector2 targetVelocity = new Vector2(xVelocity, yVelocity);

        // And then smoothing it out and applying it to the character
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref refVelocity, movementSmoothing);

        /*
        if (preMovePosition == (Vector2)transform.position)
        {
            Debug.Log("Got Stuck!");
            isStuck = true;
        }*/

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
        if (col.gameObject.layer == ladderLayer)
        {
            rb.gravityScale = 0;
            onLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == ladderLayer)
        {
            rb.gravityScale = gravityScale;
            onLadder = false;
        }
    }
}
