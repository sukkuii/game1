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
        anim.SetBool("wakeup", true); 
    }

    void FixedUpdate()
    {
        CheckDistance();
    }

    public virtual void CheckDistance()
    {
        
    }

}
