using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    idle,
    walk,
    attack,
    interact,
    stagger
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
    private float _jumpPressed;
    private float _jumpYVel;
    private Rigidbody2D _rigidbody2D;
    private Animator anim;
    private Vector3 _moveVel;
    private bool isGrounded;

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
        GetInput();
        if (_moveDir < 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            anim.SetFloat("Move", -1);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            anim.SetFloat("Move", 1);
        }
        if (_moveDir == 0)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
        if (GetInput.GetButtonDown("attack") && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCo());
        }
    }

    private IEnumerator AttackCo()
    {
        currentState = PlayerState.attack;
        anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("isAttacking", false);
        yield return new WaitForSeconds(0.3f);
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
        _moveVel.x = _moveDir * moveSpeed *Time.fixedDeltaTime;
        _rigidbody2D.velocity = _moveVel;
    }

    private float CalculateJumpVel(float height)
    {
        return MathF.Sqrt((-22 * _rigidbody2D.gravityScale*Physics2D.gravity.y * height));
    }

    void GetInput()
    {
        _moveDir = GetInput.GetAxisRaw("Horizontal");// takes move input
        _jumpPressed = GetInput.GetKeyDown(KeyCode.Space);// takes input for jumping using space
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        isGrounded = true;
        anim.SetBool("isGrouned", true);
        anim.SetBool("isJumping", false);
    }

    private void OnCollisionExit2D(Collision other)
    {
        isGrounded = false;
        anim.SetBool("isGrouned", false);
    }
}

