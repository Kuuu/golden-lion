using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    CharacterController2D character;

    public float speed = 40f;
    public float ladderSpeed = 40f;
    private float movement = 0f;
    private float ladderMovement = 0f;

    public float bottom = -10f;

    private Vector2 touch1pos;
    private Vector2 touch2pos;
    
    private bool touchMovement = true;
    private bool leftBtn, rightBtn, upBtn, downBtn = false;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController2D>();
        SetEventTriggers();
    }

    void Update()
    {

        /*
        // Handle screen touches.
        */

        if (touchMovement)
        {
            if (leftBtn) movement = -1f;
            else if (rightBtn) movement = 1f;
            else movement = 0;

            if (downBtn) ladderMovement = -1f;
            else if (upBtn) ladderMovement = 1f;
            else ladderMovement = 0;
            
        } else
        {
            movement = Input.GetAxisRaw("Horizontal");
            ladderMovement = Input.GetAxisRaw("Vertical");
        }
        if (transform.position.y <= bottom)
        {
            GameController.Instance.RestartLevel();
        }

       
    }

    void FixedUpdate()
    {
        character.Move(movement * speed * Time.fixedDeltaTime, ladderMovement * ladderSpeed * Time.fixedDeltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (LayerMask.LayerToName(col.gameObject.layer) == "Coin") {
            Destroy(col.gameObject);
            GameController.Instance.CollectedCoin();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "Enemy")
        {
            GameController.Instance.RestartLevel();
        }
    }

    public void ButtonDown(string button)
    {
        
        switch (button)
        {
            case "left":
                leftBtn = true;
                break;
            case "right":
                rightBtn = true;
                break;
            case "down":
                downBtn = true;
                break;
            case "up":
                upBtn = true;
                break;
            case "menu":
                GameController.Instance.ExitToMenu();
                break;
            default:
                break;
        }
    }

    public void ButtonUp(string button)
    {
        Debug.Log("BUTTON UP");
        switch (button)
        {
            case "left":
                leftBtn = false;
                break;
            case "right":
                rightBtn = false;
                break;
            case "down":
                downBtn = false;
                break;
            case "up":
                upBtn = false;
                break;
            default:
                Debug.Log("See Button Event in PlayerController.cs!");
                break;
        }
    }

    private void SetEventTriggers()
    {
        EventTrigger leftButton = GameObject.Find("LeftButton").GetComponent<EventTrigger>();
        EventTrigger rightButton = GameObject.Find("RightButton").GetComponent<EventTrigger>();
        EventTrigger downButton = GameObject.Find("DownButton").GetComponent<EventTrigger>();
        EventTrigger upButton = GameObject.Find("UpButton").GetComponent<EventTrigger>();
        EventTrigger menuButton = GameObject.Find("MenuButton").GetComponent<EventTrigger>();

        EventTrigger.Entry entry;

        // LEFT

        entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
        entry.callback.AddListener((eventData) => { ButtonDown("left"); });
        leftButton.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((eventData) => { ButtonUp("left"); });
        leftButton.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entry.callback.AddListener((eventData) => { ButtonUp("left"); });
        leftButton.triggers.Add(entry);

        // RIGHT

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((eventData) => { ButtonDown("right"); });
        rightButton.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((eventData) => { ButtonUp("right"); });
        rightButton.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entry.callback.AddListener((eventData) => { ButtonUp("right"); });
        rightButton.triggers.Add(entry);

        // DOWN

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((eventData) => { ButtonDown("down"); });
        downButton.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((eventData) => { ButtonUp("down"); });
        downButton.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entry.callback.AddListener((eventData) => { ButtonUp("down"); });
        downButton.triggers.Add(entry);

        // UP

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((eventData) => { ButtonDown("up"); });
        upButton.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((eventData) => { ButtonUp("up"); });
        upButton.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entry.callback.AddListener((eventData) => { ButtonUp("up"); });
        upButton.triggers.Add(entry);

        // MENU

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((eventData) => { ButtonDown("menu"); });
        menuButton.triggers.Add(entry);

    }
}