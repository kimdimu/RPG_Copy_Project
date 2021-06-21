using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Ani")]
    public Animator anim;
    public GameObject hitParticle;
    public CapsuleCollider cap;
    public float hitParticleTime = 0.2f;

    [Header("Stats")]
    public int monsterID;
    Transform targetPlayer;
    public float totalHealth;
    public float curHealth;
    public float expGranted;
    public bool dead;

    [Header("Movement")]
    public float moveSpeed;
    private FollowTarget followTarget;

    [Header("Combat")]
    public float atkDamage;
    public float atkSpeed;
    public float atkRange;
    public bool atkCoolDown;

    [Header("Animation")]
    public AnimationEvent animationEvents;

    private GameObject[] players;

    void Start()
    {
        cap = GetComponent<CapsuleCollider>();
        players = GameObject.FindGameObjectsWithTag("Player");
        curHealth = totalHealth;

        //followTarget = this.gameObject.AddComponent<FollowTarget>();
        //followTarget.enabled = false;
        //followTarget.followTime = 10 / movementSpeed;
        //followTarget.lookAtTarget = true;
        //followTarget.StopAtRange = attackRange;
        //followTarget.axisNulifier = new Vector3(1, 0, 1);

        //StartCoroutine(FindPlayerInRange());

        //animationEvents.OnAnimationAttackEvent += () => targetPlayer.Getcomponent<PlayerController>().GetHit(attackDamage);
    }
    void FindPlayerInRange()
    {

    }
    void Update()
    {
    }
    public void getHit(float dmg, GameObject obj)
    {
        if (dead) return;

        anim.SetTrigger("GetHit");
        anim.SetInteger("Condition", 100);
        curHealth -= dmg;
        if(obj.CompareTag("Player"))
            AttackEvents.HitEnemyEvent(this.gameObject);
        Debug.Log("-3");

        if (curHealth<=0)
        {
            Die();
            return;
        }
        //때려서 죽었다면 이벤트 X

        StartCoroutine(RecoverFromHit());
    }
    bool CanAttack()
    {
        if (atkCoolDown) return false;
        return true;
    }
    void Die()
    {
        //데이터에 해당 몬스터의 아이디가 없다? 추가.
        if (!PlayerData.monstersKilled.ContainsKey(monsterID))
            PlayerData.monstersKilled.Add(monsterID, new PlayerData.MonsterKills());
        PlayerData.monstersKilled[monsterID].amount++; //죽인 양 증가

        Alert.backUpTarget();

        dead = true;
        cap.enabled=false;
        DropLoot();

        foreach(GameObject go in players)
        {
            //Debug.Log("foreach");
            go.GetComponent<PlayerController>().SetExp(expGranted / players.Length);
        }

        anim.SetTrigger("IsDead");
        Destroy(this.gameObject, 3);
    }

    void DropLoot()
    {
        //print("You Get the bounty");
    }

    IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetInteger("Condition", 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().GetHit(20);


            Debug.Log("Oo!!!");
        }
    }
}
