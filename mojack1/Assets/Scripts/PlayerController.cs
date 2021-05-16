using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator anim;

    public float experience;

    [Header("Movement")]
    private bool canMove=true;
    public float moveSpeed;
    public float velocity;
    public Rigidbody rb;

    [Header("Combat")]
    private List<Transform> enemiesInRange = new List<Transform>();
    public bool attacking;
    public float atkDamage;
    public float attackSpeed;
    void Start()
    {
        experience = 0;
        AnimatinoEvents.OnSlashAnimationHit += DealDamage;
    }

    void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if(Input.GetKey(KeyCode.A))
        {
            SetVelocity(-1);
        }
        if(Input.GetKeyUp(KeyCode.A))
        {
            SetVelocity(0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            SetVelocity(1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            SetVelocity(0);
        }
    }

    void Move()
    {
        if (canMove)
        {
            if (velocity == 0)
            {
                anim.SetInteger("Condition", 0);
                return;
            }
            else
            {
                anim.SetInteger("Condition", 1);
            }
            rb.MovePosition(transform.position + Vector3.right * velocity * moveSpeed * Time.deltaTime);
        }
    }
    void SetVelocity(float dir)
    {
        if (dir < 0) transform.LookAt(transform.position + Vector3.left);
        if(dir>0)transform.LookAt(transform.position + Vector3.right);
        velocity = dir;
    }

    void Attack()
    {
        if (attacking) return;
        anim.speed = attackSpeed;
        anim.SetTrigger("Attack");
        StartCoroutine(AttackRoutine());
        //StartCoroutine(AttackCooldown());
    }

    void DealDamage()
    {
        Debug.Log("deal damage");
        GetEnemiesInRange();
        foreach (Transform enemy in enemiesInRange)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec == null) continue;
            ec.getHit(atkDamage);
        }
    }
    IEnumerator AttackRoutine()
    {
        attacking = true;
        yield return new WaitForSeconds(1);
        attacking = false;
    }
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1/attackSpeed);
    }
    void GetEnemiesInRange()
    {
        enemiesInRange.Clear();
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * 1f), 1f))
            if(c.gameObject.CompareTag("Enemy"))
            {
                enemiesInRange.Add(c.transform);
            }
    }

    public void GetExp(float exp)
    {
        experience += exp;
        Debug.Log(experience);
    }
}
