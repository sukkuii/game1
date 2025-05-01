using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasingenemy : Enemy
{
    [Header("Rigidbody")]
    public Rigidbody2D myRigidbody;

    [Header("Target Variables")]
    public Transform target;
    public float chaseRadius;
    public float attackRadius;

    [Header("Animator")]
    public Animator anim;
    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        anim.SetBool("Wakeup", true); 
    }

    void FixedUpdate()
    {
        CheckDistance();
    }

    private IEnumerator Attack()
    {
        ChangeState(EnemyState.attack);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.15f);
        ChangeState(EnemyState.idle);       
    }

    public virtual void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if(currentState == EnemyState.idle || EnemyState.walk == currentState && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed*Time.deltaTime);
                ChangeAnim(target);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                anim.SetBool("Wakeup", true);
            }
        }
        else if(Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            anim.SetBool("Wakeup", false);
        }
        else if(Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            if(currentState != EnemyState.stagger && isAttackingEnemy && currentState != EnemyState.attack)
            {
                StartCoroutine(Attack());
            }
        }
    }

}
