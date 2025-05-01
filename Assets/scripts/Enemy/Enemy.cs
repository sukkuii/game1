using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger,
} 

public class Enemy : MonoBehaviour
{
    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Health Stats")]
    public float maxHealth;// Сделать переменную сохраняемого типа
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public Vector2 homePosition;
    private bool facingRight;
    [SerializeField] protected bool isAttackingEnemy = false;
    
    [Header("Death Effects")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f;

    private void Awake()
    {
        health = maxHealth;
    }

    public void Knock(Rigidbody2D myRb, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(myRb, knockTime));
        TakeDamage(damage);
    }

    public void ChangeAnim(Transform target)
    {
        Vector3 targetPosition = target.position; 
        Vector3 diff = targetPosition - transform.position;
        diff.Normalize();
        if(diff.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingRight = false;
        }
        else if(!facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingRight = true;
        }
    }

    private IEnumerator KnockCo(Rigidbody2D myRb, float knockTime)
    {
        if(myRb != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRb.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }

    private void Start()
    {
        health = maxHealth;
    }

    void OnEnable()
    {
        transform.position = homePosition;
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DeathEffect();
            this.gameObject.SetActive(false);
        }
    }

    private void DeathEffect()
    {
        if(deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
        }
    }
}