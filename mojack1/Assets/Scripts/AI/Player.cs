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

    Vector3 acceleration; // 가속도
    Vector3 steeringForce;// 힘
    Vector3 mass;// 질량
    public float wanderRadius;
    public float wanderDist;
    public float wanderJitter;

    public float speed;//곱해줄 속도

    void Start()
    {
        stats = new Stats();
        stats.HP=3;

        stateMachine = new StateMachine<Player>();
        stateMachine.SetOwner(this);
        stateMachine.SetCS(BackToPlayer.Instance); //왜 이 밑으로는 안가는거지..

        steeringBehavior = new SteeringBehavior();
        steeringBehavior.SetTargetPlayer(this);
        steeringBehavior.SeparationOn();

        movingEntity = new MovingEntity();
        steeringBehavior.SetTargetAgent1(target);
        steeringBehavior.SetWander(wanderRadius, wanderDist, wanderJitter);
    }
    public StateMachine<Player> GetFSM() { return stateMachine; }
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
    //void Attack()
    //{
    //    if (isAttack) return;

    //    anim.speed = attackSpeed;
    //    anim.SetTrigger("Attack");
    //    StartCoroutine(AttackRoutine());
    //    //StartCoroutine(AttackCooldown());
    //}
    //void SetAttackDmg()
    //{
    //    atkDamage = GameLogic.CalculatePlayerBaseAttackDmg(this) + weaponDmg + bonusDmg;
    //}
    //void DealDamage(GameObject who)
    //{
    //    if (who == this.gameObject)
    //    {
    //        Debug.Log("deal damage");
    //        GetEnemiesInRange();
    //        foreach (Transform enemy in enemiesInRange)
    //        {
    //            EnemyController ec = enemy.GetComponent<EnemyController>();
    //            if (ec == null) continue;
    //            ec.getHit(atkDamage);
    //        }
    //    }
    //}
    //IEnumerator AttackRoutine()
    //{
    //    isAttack = true;
    //    yield return new WaitForSeconds(1);
    //    isAttack = false;
    //}
    //IEnumerator AttackCooldown()
    //{
    //    yield return new WaitForSeconds(1 / attackSpeed);
    //}

    //void GetEnemiesInRange()
    //{
    //    enemiesInRange.Clear();
    //    foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * 1f), 1f))
    //        if (c.gameObject.CompareTag("Enemy"))
    //        {
    //            enemiesInRange.Add(c.transform);
    //        }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("AIATTACK");
            anim.SetTrigger("Attack");
        }
    }
}
