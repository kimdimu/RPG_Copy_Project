using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MovingEntity movingEntity;
    public SteeringBehavior steeringBehavior;
    public StateMachine<Player> stateMachine;
    public StateMachine<Player> stateMachine2;
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
    public short interposeState;

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
        interposeState = 0;
        mainPlayer = target;
        Alert.backUpTarget += BackUpTarget;
        AttackEvents.HitEnemyEvent += GetTarget;
        stats = new Stats();
        stats.HP = 100;

        stateMachine = new StateMachine<Player>();
        stateMachine.SetOwner(this);
        stateMachine.SetCS(Idle.Instance); 
        stateMachine.SetGS(GlobalState.Instance);

        stateMachine2 = new StateMachine<Player>();
        stateMachine2.SetOwner(this);
        stateMachine2.SetCS(NoneState.Instance);
        stateMachine2.SetGS(GlobalState.Instance);

        steeringBehavior = new SteeringBehavior();
        steeringBehavior.SetTargetPlayer(this);
        steeringBehavior.SeparationOn();

        movingEntity = new MovingEntity();
        steeringBehavior.SetTargetAgent1(target);
        steeringBehavior.SetWander(wanderRadius, wanderDist, wanderJitter);
    }
    public StateMachine<Player> GetFSM() { return stateMachine; }
    public StateMachine<Player> GetFSM2() { return stateMachine2; }
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
        if(Input.GetKeyDown(KeyCode.U))
        {
            stats.HP += 10;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            stats.HP -= 10;
        }
        foreach (GameObject go in GetTargets(5f))
        {
            if (go.CompareTag("Enemy"))
            {
                if (isInRange(go.transform.position))
                {
                    //현재 타겟과go가 다르면 Alert 하자! 그리고 이미 공격중이라면 타겟을 바꾸지 않는다 까지?
                    Debug.Log("InRange");
                    attackState = 1;
                    break;
                }
            }
        }
        
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

    public bool isInRange(Vector3 targetPoint)
    {
        Vector3 toTarget = targetPoint - transform.position;
        if (Vector3.Dot(toTarget, transform.forward) > 0)
        {
            Vector3 meetPoint = new Vector3(0, 0, 0);
            float normalA = 0;

            if (transform.forward.z == 0) //z=0그래프
            {
                normalA = 0; //2
                meetPoint.x = targetPoint.x + transform.position.x;
                meetPoint.z = transform.position.z;
            }
            if (transform.forward.x == 0) //x=0그래프
            {
                normalA = 0; //2
                meetPoint.x = transform.position.x;
                meetPoint.z = targetPoint.z + transform.position.z;
            }
            else
            {
                normalA = transform.forward.z / transform.forward.x; //2

                float normalB = transform.forward.z - transform.forward.x * normalA; //2  . . y = 2x + 2
                float inverseA = -1 / normalA; //-1/2

                float inverseB = targetPoint.z + inverseA * -targetPoint.x;//b = y + -1/2x, b=13/2

                meetPoint.x = (inverseB - normalB) / (normalA - inverseA);
                meetPoint.z = normalA * meetPoint.x + normalB;
            }

            float rad = 1f;
            float addDist = Vector3.Distance(transform.position, meetPoint) / rad;
            float targetToMeetPointDist = Vector3.Distance(meetPoint, targetPoint);
            if (targetToMeetPointDist < addDist)
            {
                //인식
                return true;
            }
        }
        return false;
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
        bool check = true;
        foreach (Transform enemy in enemiesInRange)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec == null) continue;
            if(!enemy.GetComponent<EnemyController>().dead)
                ec.getHit(atkDamage, this.gameObject);
            if(check == true)
                check = enemy.GetComponent<EnemyController>().dead;
        }
        if(check == true )
        {
            //attackState = 0;
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

    public List<Transform> GetEnemiesInRange()
    {
        enemiesInRange.Clear();
        foreach (GameObject c in GetTargets(5f))//Physics.OverlapSphere((transform.position + transform.forward * 1f), 2f))
            if (c.gameObject.CompareTag("Enemy") && isInRange(c.transform.position))
            {
                Debug.Log(c.name);
                enemiesInRange.Add(c.transform);
            }
        return enemiesInRange;
    }
}
