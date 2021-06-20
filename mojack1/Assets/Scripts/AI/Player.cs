using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MovingEntity movingEntity;
    public SteeringBehavior steeringBehavior;
    public StateMachine<Player> stateMachine;
    public Stats stats;

    public Animator anim;
    public GameObject target;
    public GameObject mainPlayer;

    Vector3 acceleration; // 가속도
    Vector3 steeringForce;// 힘
    Vector3 mass;// 질량
    public float wanderRadius;
    public float wanderDist;
    public float wanderJitter;

    public float speed;//곱해줄 속도
    public short alertState;
    public short attackState;
    public short hideState;

    [Header("Combat")]
    public bool isAttack;
    public float atkDamage;
    public float attackSpeed;
    public float weaponDmg;
    public float bonusDmg;

    private List<Transform> enemiesInRange = new List<Transform>(); //enemies List in attack range

    void Start()
    {
        attackState = 0;
        alertState = 0;
        hideState = 0;
        mainPlayer = target;
        Alert.backUpTarget += BackUpTarget;
        AttackEvents.HitEnemyEvent += GetTarget;
        stats = new Stats();
        stats.HP = 15;

        stateMachine = new StateMachine<Player>();
        stateMachine.SetOwner(this);
        stateMachine.SetCS(Idle.Instance); 
        stateMachine.SetGS(GlobalState.Instance); 

        steeringBehavior = new SteeringBehavior();
        steeringBehavior.SetTargetPlayer(this);
        steeringBehavior.SeparationOn();

        movingEntity = new MovingEntity();
        steeringBehavior.SetTargetAgent1(target);
        steeringBehavior.SetWander(wanderRadius, wanderDist, wanderJitter);
    }
    public StateMachine<Player> GetFSM() { return stateMachine; }
    public void GetTarget(GameObject g)
    {
        if (target == mainPlayer)
        {
            target = g;
            alertState = 1;
        }
    }
    public void BackUpTarget() { target = mainPlayer; }
    void Update()
    {
        transform.LookAt(transform.position + movingEntity.m_vVelocity * Time.deltaTime * speed);

        stateMachine.sUpdate();

        steeringForce = steeringBehavior.Calculate();

        acceleration = steeringForce;// /mass;
        movingEntity.m_vVelocity = acceleration * Time.deltaTime; //가 = 속도변화량 / 시
        transform.position += movingEntity.m_vVelocity * Time.deltaTime * speed; // 거 = 속 * 시


        if (steeringForce.magnitude < 0.1f)
        {
            anim.SetInteger("Condition", 0);
        }
        else
        {
            anim.SetInteger("Condition", 1);
        }
    }

    public List<GameObject> GetTargets(float dis)
    {
        List<GameObject> enemiesInRange = new List<GameObject>();
        enemiesInRange.Clear();
        foreach (Collider c in Physics.OverlapSphere
                                    (transform.position, dis))
        {
            enemiesInRange.Add(c.gameObject);
        }
        return enemiesInRange;
    }

    public void Attack()
    {
        if (isAttack) return;
        Debug.Log("AttackEnemy");

        anim.speed = attackSpeed*2;

        anim.SetTrigger("Attack");
        DealDamage();
        StartCoroutine(AttackRoutine());
        StartCoroutine(AttackCooldown());
    }
    void DealDamage()
    {
        GetEnemiesInRange();
        //if (enemiesInRange.Count == 0)
        //{
        //    attackState = 0;
        //    return;
        //}
        bool check = true;
        foreach (Transform enemy in enemiesInRange)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec == null) continue;
            if(!enemy.GetComponent<EnemyController>().dead)
                ec.getHit(atkDamage);
            if(check == true)
                check = enemy.GetComponent<EnemyController>().dead;
        }
        if(check == true )
        {
            attackState = 0;
            anim.speed = attackSpeed ;

            Debug.Log("check == true ... all death ...attackState");

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
        yield return new WaitForSeconds(1 / attackSpeed);
    }

    void GetEnemiesInRange()
    {
        enemiesInRange.Clear();
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * 1f), 2f))
            if (c.gameObject.CompareTag("Enemy"))
            {
                enemiesInRange.Add(c.transform);
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("AIATTACK");
            //anim.SetTrigger("Attack");
            //target = other.gameObject;
            attackState = 1;
        }
    }
}
