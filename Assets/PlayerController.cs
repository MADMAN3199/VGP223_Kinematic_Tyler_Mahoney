using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpSpeed = 14f;
    public float playerGravity = -9.81f;
    public float gravityFactor = 1;
    public float gravityGoingUp = 1.0f;
    public float gravityFalling = 2.0f;
    public float groundCheckDistance = 0.5f;

    public float hSpeed = 0;
    public float hAccelRate = 0.1f;
    public float hDeccelRate = 0.1f;
    
    // Jump Time
    public float maxJumpButtonTime = 0.5f;
    [SerializeField] private float jumpTime = 0.0f;

    public bool isLeft;
    public bool isRight;
    
    public bool isJumping = false;
    public bool isFalling = false;
    public bool isJumpingUp = false;
    public bool grounded = false;

    public Vector3 playerInput = Vector3.zero;
    public Vector3 playerVelocity = Vector3.zero;
    public LayerMask groundMask;
    
    private RaycastHit hitGround;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isLeft = Input.GetKey(KeyCode.A);
        isRight = Input.GetKey(KeyCode.D);
        
        // Basic Movement
        if (isLeft)
        {
            playerInput.x = -1.0f;
        }

        if (isRight)
        {
            playerInput.x = 1.0f;
        }

        // Check Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            grounded = false;
            isJumping = true;
            isJumpingUp = true;
            
            jumpTime = 0.0f;
        }

        if (isJumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime > maxJumpButtonTime)
            {
                isJumping = false;
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Horizontal Movement
        if (isLeft || isRight)
        {
            hSpeed = Mathf.Lerp(hSpeed, maxSpeed * playerInput.x, hAccelRate);
        }
        else
        {
            hSpeed = Mathf.Lerp(hSpeed, 0, hDeccelRate);
        }

        playerVelocity.x = hSpeed;
        
        // Vertical Movement
        // Change Gravity based on whether the player is falling or jumping
        if (isFalling)
        {
            gravityFactor = gravityFalling;
        }
        
        if (isJumpingUp)
        {
            gravityFactor = gravityGoingUp;
        }
        playerVelocity.y += (playerGravity * gravityFactor) * Time.fixedDeltaTime;

        if (isJumping)
        {
            playerVelocity.y += jumpSpeed * Time.fixedDeltaTime;
        }
        
        // Check if grounded
        if (!isJumpingUp)
        {
            grounded = IsGrounded();
        }

        // Handle ground check
        if (grounded)
        {
            // Settings up for jump throughs
            transform.position = new Vector3(transform.position.x, hitGround.transform.position.y + 1.0f, transform.position.z);
            playerVelocity.y = 0.0f;

            isFalling = false;
            isJumpingUp = false;
        }
        else
        {
            // TODO before telling the character fall
            // Add a hover time here
            if (playerVelocity.y < 0)
            {
                isFalling = true;
                isJumpingUp = false;
            }
            else
            {
                isFalling = false;
                isJumpingUp = true;
            }
    
        }

        rb.MovePosition(rb.position + playerVelocity * Time.fixedDeltaTime);
    }

    private bool IsGrounded()
    {
        Vector3 lineStart = transform.position;
        Vector3 vectorToSearch = new Vector3(lineStart.x, lineStart.y - groundCheckDistance, lineStart.z);
        
        return Physics.Linecast(lineStart, vectorToSearch, out hitGround, groundMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, -transform.up * groundCheckDistance);
    }
}
