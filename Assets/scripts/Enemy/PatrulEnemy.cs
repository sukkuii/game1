using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrulEnemy : Chasingenemy
{
    public Transform[] path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundDistance;

    public override void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if(currentState == EnemyState.idle || EnemyState.walk == currentState && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed*Time.deltaTime);
                ChangeAnim(target.position.x);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                anim.SetBool("Wakeup", true);
            }
        }
        else if(Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            if(Vector3.Distance(transform.position, currentGoal.position) > roundDistance)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, currentGoal.position, moveSpeed * Time.deltaTime);
                ChangeAnim(currentGoal.position.x);
                myRigidbody.MovePosition(temp);
            }
            else
            {
                ChangeGoal();
            }
        }
    }

    public void ChangeGoal()
    {
        if(currentPoint == path.Length -1)
        {
            currentPoint = 0;
            currentGoal = path[0];
        }
        else
        {
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }
}