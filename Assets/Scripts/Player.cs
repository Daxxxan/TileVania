using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpSpeed = 28f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(250f, 250f);

    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    CapsuleCollider2D playerCollider;
    BoxCollider2D myFeet;
    float playerGravity;

    bool isAlive = true;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        playerGravity = playerRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive){return;}
        Die();
        Run();
        Jump();
        ClimbLadder();
        SplitSprite();
    }

    private void Run()
    {
        float controlThrow = Input.GetAxisRaw("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;

        playerAnimator.SetBool("Running", IsRunning());
    }

    private void SplitSprite()
    {
        if(IsRunning())
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }
    }

    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            playerRigidbody.velocity += jumpVelocityToAdd;
        }
    }

    private void Die()
    {
        if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            playerAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            isAlive = false;
        }
    }

    private void ClimbLadder()
    {
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        { 
            playerRigidbody.gravityScale = playerGravity;
            playerAnimator.SetBool("Climbing", false);
            return; 
        }

        float controlThrow = Input.GetAxisRaw("Vertical");
        bool playerHasVerticalSpeed = Mathf.Abs(playerRigidbody.velocity.y) > Mathf.Epsilon;

        Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, controlThrow * climbSpeed);
        playerRigidbody.velocity = climbVelocity;
        playerAnimator.SetBool("Climbing", playerHasVerticalSpeed);
        playerRigidbody.gravityScale = 0f;
    }

    private bool IsRunning()
    {
        return Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
    }
}