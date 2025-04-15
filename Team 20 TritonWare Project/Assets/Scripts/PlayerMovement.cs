using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Snapshot of position and flip state
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
    public GameObject clonePrefab; // assign in Inspector

    private bool canJump = false;
    private bool isRecording = true; // control whether to record movements
    private float recordTimer = 0f;
    private List<MovementSnapshot> positionHistory = new List<MovementSnapshot>();

    void Start()
    {
        canJump = true;
    }

    void Update()
    {
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

        // Record position and flip state only if we're still recording
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
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = true;
        } else if(collision.collider.CompareTag("Object"))
        {
            canJump = true;
        }
        else if (collision.collider.CompareTag("danger"))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = RespawnPoint.transform.position;
        PlayerRB.linearVelocity = Vector2.zero;
        positionHistory.Clear();
    }

    // Method to stop recording and spawn the clone
    void ReverseMovement()
    {
        isRecording = false; // Stop recording after pressing D

        if (positionHistory.Count == 0) return;

        // Instantiate the clone and pass the recorded movement history
        GameObject clone = Instantiate(clonePrefab, transform.position, Quaternion.identity);
        ClonePlayback playbackScript = clone.GetComponent<ClonePlayback>();
        playbackScript.Initialize(positionHistory);

        // After spawning the clone, reset and start recording new movements
        positionHistory.Clear(); // Clear old history to discard it
        isRecording = true; // Start recording new movements
    }
}