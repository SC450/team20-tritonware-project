using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MovementSnapshot
{
    public Vector3 position;
    public bool flipX;

    public MovementSnapshot(Vector3 pos, bool flip)
    {
        position = pos;
        flipX = flip;
    }
}

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D PlayerRB;
    public SpriteRenderer PlayerSR;
    public float Speed;
    public float JumpStrength;
    public float RecordInterval = 0.02f;
    public GameObject RespawnPoint;
    public GameObject clonePrefab;

    private bool canJump = false;
    private bool isRecording = true;
    private float recordTimer = 0f;
    private List<MovementSnapshot> positionHistory = new List<MovementSnapshot>();

    // Invincibility system
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    private float invincibilityDuration = 1f;
    private const string normalLayer = "player";
    private const string invincibleLayer = "invinciblePlayer";

    void Start()
    {
        canJump = true;
    }

    void Update()
    {
        // Handle invincibility timer
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                gameObject.layer = LayerMask.NameToLayer(normalLayer); // Restore collision
                PlayerSR.color = new Color(1f, 1f, 1f, 1f); // Reset transparency
            }
        }

        // Movement
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.right;
            PlayerSR.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
            PlayerSR.flipX = true;
        }

        Move(direction);

        // Jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && canJump)
        {
            PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, JumpStrength);
            canJump = false;
        }

        // Spawn reverse clone when `D` is pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            ReverseMovement();
        }

        // Record movement history
        if (isRecording)
        {
            recordTimer += Time.deltaTime;
            if (recordTimer >= RecordInterval)
            {
                positionHistory.Add(new MovementSnapshot(transform.position, PlayerSR.flipX));
                recordTimer = 0f;
            }
        }
    }

    void Move(Vector2 direction)
    {
        PlayerRB.linearVelocity = new Vector2(direction.x * Speed, PlayerRB.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Object"))
        {
            canJump = true;
        } else if (collision.collider.CompareTag("Player") && !isInvincible)
        {
            Destroy(collision.gameObject);
            Respawn();
        }
    }

    

    void Respawn()
    {
        transform.position = RespawnPoint.transform.position;
        PlayerRB.linearVelocity = Vector2.zero;
        positionHistory.Clear();

        
    }

    void ReverseMovement()
    {
        isRecording = false;

        if (positionHistory.Count == 0) return;

        GameObject clone = Instantiate(clonePrefab, transform.position, Quaternion.identity);
        clone.layer = LayerMask.NameToLayer("Clone");

        ClonePlayback playbackScript = clone.GetComponent<ClonePlayback>();
        playbackScript.Initialize(positionHistory);

        positionHistory.Clear();
        isRecording = true;

        // Start invincibility after spawning clone
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        gameObject.layer = LayerMask.NameToLayer(invincibleLayer);
        PlayerSR.color = new Color(1f, 1f, 1f, 0.5f);
    }
}