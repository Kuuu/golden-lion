using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    CharacterController2D character;

    public float speed = 40f;
    public float ladderSpeed = 40f;
    private float movement = 0f;
    private float ladderMovement = 0f;

    public float bottom = -10f;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        if (transform.position.y <= bottom)
        {
            GameController.Instance.RestartLevel();
        }

        movement = Input.GetAxisRaw("Horizontal");


        ladderMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
}