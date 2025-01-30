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
    CapsuleCollider2D mainCollider;

    [Header("Movement")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float moveSpeed = 10f;
    public PlayerState currentState;
    private bool facingRight;

    [Header("Health")]
    public FloatValue currentHealth;
    public PSignal playerHealthSignal;

    [Header("Iframe Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;
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

    public void Knock(float knockTime, float damage, Vector3 positionOfEnemy)
    {
        if(!(isBlocking && (facingRight && positionOfEnemy.x > this.gameObject.transform.position.x || !facingRight && positionOfEnemy.x < this.gameObject.transform.position.x)))
        {
            currentHealth.runtimeValue -= damage;
            playerHealthSignal.Raise();
            if(currentHealth.runtimeValue > 0)
            {
                StartCoroutine(KnockCo(knockTime));
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if(_rigidbody2D != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            _rigidbody2D.velocity = Vector2.zero;
            currentState = PlayerState.idle;
        }
    }
    void Start()
    {
        anim.SetFloat("Move", 1);
        currentState = PlayerState.walk;
    }

    private void Update()
    {
        _moveDir = Input.GetAxisRaw("Horizontal");
        _jumpPressed = Input.GetButton("Jump");
        buttonBlockPressed = Input.GetButtonDown("block");// Настройка
        buttonBlockUp = Input.GetButtonUp("block");
        if(currentState != PlayerState.block && currentState != PlayerState.stagger)
        {
            if (_moveDir < 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
                anim.SetFloat("Move", -1);
                anim.SetBool("isWalking", true);
                facingRight = false;
            }
            else if(_moveDir > 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
                anim.SetFloat("Move", 1);
                anim.SetBool("isWalking", true);
                facingRight = true;
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
        _moveDir = Input.GetAxisRaw("Horizontal");
        _jumpPressed = Input.GetButtonDown("Jump");
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

    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while(temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }
}

