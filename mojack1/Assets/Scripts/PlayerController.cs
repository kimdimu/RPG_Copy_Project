using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator anim;

    [Header("Attribute")]
    private int level = 1;
    private Text levelText;
    public float Experience { get; private set; } // property. set is private. we can use set only in this class(instance)
    private Transform expbar;

    [Header("Health")]
    public float incapacitatedTime; //피격 후 무력화 시간 
    public bool dead;
    public float totalHealth;
    public float curHealth;

    [Header("Movement")]
    private bool canMove = true;
    public float moveSpeed;
    public float velocity;
    public Rigidbody rb;

    [Header("Combat")]
    private List<Transform> enemiesInRange = new List<Transform>(); //enemies List in attack range
    public bool isAttack;
    public float atkDamage;
    public float attackSpeed;
    void Start()
    {
        dead = false;
        Experience = 0;
        AnimatinoEvents.OnSlashAnimationHit += DealDamage;
        curHealth = totalHealth;
        expbar = UIController.instance.transform.Find("Background/Exp");
        levelText = UIController.instance.transform.Find("Background/LvText").GetComponent<Text>();
    }

    void Update()
    {
        if (incapacitatedTime > 0) return;
        GetInput();
        Move();
    }

    void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (Input.GetKey(KeyCode.A))
        {
            SetVelocity(-1);
        }
        if (Input.GetKeyUp(KeyCode.A))
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
        if (dir > 0) transform.LookAt(transform.position + Vector3.right);
        velocity = dir;
    }

    void Attack()
    {
        if (isAttack) return;
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
        isAttack = true;
        yield return new WaitForSeconds(1);
        isAttack = false;
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

    public void GetHit(float dmg)
    {
        if (dead) return;
        anim.SetTrigger("GetHit");
        curHealth -= dmg;

        if (curHealth <= 0)
        {
            Die();
            return;
        }

        GetIncapacitated(0.2f);
    }

    void Die()
    {
        dead = true;
        anim.SetTrigger("IsDead");
    }

    private void GetIncapacitated(float time)
    {
        if (incapacitatedTime < time)
        {
            StopCoroutine("GetIncapacitatedRoutine");
            incapacitatedTime = time;
            StartCoroutine("GetIncapacitatedRoutine");
        }
    }
    IEnumerator GetIncapacitatedRoutine()
    {
        while (incapacitatedTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            incapacitatedTime -= 0.1f;
        }
    }
    public void SetExp(float exp)
    {
        Experience += exp;

        float expNeeded = GameLogic.ExpForNextLevel(level);
        float prevExp = GameLogic.ExpForNextLevel(level - 1);

        while(Experience>=expNeeded)//한번에 2업 시 1업만 되는 경우 방어
        {
            LevelUp();
            expNeeded = GameLogic.ExpForNextLevel(level);
            prevExp = GameLogic.ExpForNextLevel(level - 1);
        }
        expbar.Find("Fill_bar").GetComponent<Image>().fillAmount = (Experience - prevExp) / (expNeeded -prevExp);     

        Debug.Log("expNeeded: "+ expNeeded + "experience: " +Experience);
    }

    void LevelUp()
    {
        level++;
        levelText.text = "Lv. " + level.ToString("00");
    }
}
