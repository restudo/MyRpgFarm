using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMonobehaviour<PlayerController>
{
    private bool facingRight;
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float xInput, yInput;
    private int isWalking;

    [Header("MoveController")]
    [SerializeField] private float moveSpeed;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        facingRight = true;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    
        isWalking = Animator.StringToHash("isWalking");
    }

    void Update()
    {
        PlayerMovementInput();
    }

    void FixedUpdate() // physics
    {
        PlayerMovement();
    }

    void PlayerMovementInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(xInput, yInput).normalized;

        if(xInput != 0 || yInput != 0){
            anim.SetBool(isWalking, true);
        }
        else{
            anim.SetBool(isWalking, false);
        }

        if (xInput < 0 && facingRight)
        {
            Flip();
        }
        else if (xInput > 0 && !facingRight)
        {
            Flip();
        }
    }

    void PlayerMovement()
    {
        rb.MovePosition(rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime));
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
