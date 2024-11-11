using System.Collections;
using System.Collections.Generic;
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

    public void ChangeAnim(float moveX)
    {
        if(transform.position.x > moveX)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
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