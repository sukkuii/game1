using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    idle,
    walk,
    attack,
    interact,
    stagger,
    block
}

public class CharacterControll : MonoBehaviour
{ 
    public Camera mainCamera;
    bool facingRight = true;
    float moveDirection = 0;
    Vector3 CameraPos;
    CapsuleCollider2D mainCollider;
    Transform t;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float moveSpeed = 10f;
    public PlayerState currentState;
    private float _moveDir;
    private bool _jumpPressed;
    private float _jumpYVel;
    private Rigidbody2D _rigidbody2D;
    private Animator anim;
    private Vector3 _moveVel;
    private bool isGrounded;
    private bool isBlocking;
    bool buttonBlockPressed;
    bool buttonBlockUp;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        anim.SetFloat("Move", 1);
        currentState = PlayerState.walk;
    }

    private void Update()
    {
        _moveDir = Input.GetAxisRaw("Horizontal");// takes move input
        _jumpPressed = Input.GetKeyDown(KeyCode.Space);// takes input for jumping using space
        buttonBlockPressed = Input.GetButtonDown("block");// Настройка
        buttonBlockUp = Input.GetButtonUp("block");
        if(currentState != PlayerState.block && currentState != PlayerState.stagger)
        {
            if (_moveDir < 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
                anim.SetFloat("Move", -1);
                anim.SetBool("isWalking", true);
            }
            else if(_moveDir > 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
                anim.SetFloat("Move", 1);
                anim.SetBool("isWalking", true);
            }
        }
        if (_moveDir == 0)
        {
            anim.SetBool("isWalking", false);
        }
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.block)// stagger
        {            
            StartCoroutine(AttackCo());           
        }
        if(buttonBlockPressed && (currentState == PlayerState.idle || currentState == PlayerState.walk))
        {
            currentState = PlayerState.block;
            isBlocking = true;
            anim.SetBool("isWalking", false);
            anim.SetBool("isBlocking", true);
        }
        if(buttonBlockUp && currentState == PlayerState.block)
        {
            anim.SetBool("isBlocking", false);
            currentState = PlayerState.walk;
            isBlocking = false;
        }
    }

    private IEnumerator AttackCo()
    {
        currentState = PlayerState.attack;
        anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0);
        anim.SetBool("isAttacking", false);
        yield return new WaitForSeconds(1);
        currentState = PlayerState.walk;
    }
    
    private void FixedUpdate()
    {
        if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            Move();
            HandleJump();
        }

    }
    private void HandleJump()
    {
        if(isGrounded && _jumpPressed)
        {
            anim.SetBool("isJumping", true);
            _jumpYVel = CalculateJumpVel(jumpHeight);
            _jumpPressed = false;

            _moveVel = _rigidbody2D.velocity;
            _moveVel.y = _jumpYVel;
            _rigidbody2D.velocity = _moveVel;
        }
    }

    private void Move()
    {
        _moveVel = _rigidbody2D.velocity;
        _moveVel.x = _moveDir * moveSpeed * Time.fixedDeltaTime;
        _rigidbody2D.velocity = _moveVel;
    }

    private float CalculateJumpVel(float height)
    {
        return MathF.Sqrt((-22 * _rigidbody2D.gravityScale*Physics2D.gravity.y * height));
    }

    void GetInput()
    {
        _moveDir = Input.GetAxisRaw("Horizontal");// takes move input
        _jumpPressed = Input.GetKeyDown(KeyCode.Space);// takes input for jumping using space
        buttonBlockPressed = Input.GetButtonDown("block");// Настройка
        buttonBlockUp = Input.GetButtonUp("block");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        isGrounded = true;
        anim.SetBool("isGrouned", true);
        anim.SetBool("isJumping", false);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
        anim.SetBool("isGrouned", false);
    }
}

