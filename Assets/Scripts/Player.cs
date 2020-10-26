using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;

    Rigidbody2D playerRigidbody;
    Animator playerAnimator;

    bool isAlive = true;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
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

    private bool IsRunning()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            return true;
        }
        return false;
    }
}