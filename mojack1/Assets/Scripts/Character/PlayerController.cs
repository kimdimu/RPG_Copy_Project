using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController main;

    public Animator anim;

    //[Header("Attribute")]
    public int level = 1;
    public Text levelText;
    public float Experience { get; private set; } // property. set is private. we can use set only in this class(instance)
    public Transform expbar;
    public Transform hpbar;
    public Transform spbar;

    [Header("Health")]
    public float incapacitatedTime; //피격 후 무력화 시간 
    public bool dead;
    public float totalHealth;
    public float curHealth;

    [Header("Combat")]
    public bool isAttack;
    public float atkDamage;
    public float attackSpeed;
    public float weaponDmg;
    public float bonusDmg;
    
    private List<Transform> enemiesInRangeAttack = new List<Transform>(); //enemies List in attack range
    private List<Transform> enemiesInRange_ = new List<Transform>(); //enemies List in attack range

    [Header("Movement")]
    Vector3 moveAmount;
    public float moveSpeed;
    public float velocity;
    public Rigidbody rb;
    private bool canMove = true;

    public float strength;// { get; private set; }

    void Start()
    {
        if (main == null) main = this;

        dead = false;
        Experience = 0;
        AnimationEvents.OnSlashAnimationHit += DealDamage;
        AnimationEvents.OnOffMove += ChangeIsMove;
        curHealth = totalHealth;
        expbar = UIController.instance.canvas.Find("Background/Exp");
        hpbar = UIController.instance.canvas.Find("Background/Health");
        levelText = UIController.instance.canvas.Find("Background/LvText").GetComponent<Text>();
        hpbar.Find("Fill_bar").GetComponent<Image>().fillAmount = curHealth / totalHealth;
        hpbar.Find("Text").GetComponent<Text>().text = "HP " + curHealth + "/" + totalHealth;
        SetExp(0);
        SetAttackDmg();
    }
    //-------------------------------------------------------------UPDATE
    void Update()
    {
        if (incapacitatedTime > 0) return;
        GetInput();
        Move();
    }
    //-------------------------------------------------------------INPUT
    void GetInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                NPCController npcController = hit.transform.GetComponent<NPCController>();
                if (npcController != null)
                {
                    npcController.OnClick();
                    //npcController.dialogueIndex++;
                    return;
                }
            }
            Attack();
        }
    }
    //-------------------------------------------------------------MOVE
    void Move()
    {
        if (canMove)
        {
            moveAmount.z = 0;
            moveAmount.x = 0;
            SetVelocity(0);
            transform.position += moveAmount.normalized;

            if (Input.GetKey(KeyCode.A))
            {
                SetVelocity(1);
                moveAmount.x = -moveSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                SetVelocity(2);
                moveAmount.x = moveSpeed;
            }
            if (Input.GetKey(KeyCode.W))
            {
                SetVelocity(3);
                moveAmount.z = moveSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                SetVelocity(4);
                moveAmount.z = -moveSpeed;
            }
            transform.position += moveAmount.normalized * moveSpeed * Time.deltaTime;


            //rb.MovePosition(transform.position + Vector3.right * velocity * moveSpeed * Time.deltaTime);
        }
    }
    void SetVelocity(float dirx)
    {
        if (dirx == 1) transform.LookAt(transform.position + Vector3.left);
        if (dirx == 2) transform.LookAt(transform.position + Vector3.right);
        if (dirx == 3) transform.LookAt(transform.position + Vector3.forward);
        if (dirx == 4) transform.LookAt(transform.position + Vector3.back);
        velocity = dirx;//+diry);
        if (velocity == 0)
        {
            anim.SetInteger("Condition", 0);
            return;
        }
        else
        {
            anim.SetInteger("Condition", 1);
        }
    }
    public void ChangeIsMove() //빌드 상태가 아니며 움직일 수 있다면 
    {
        if (canMove)
            canMove = false;
        else
            canMove = true;
    }
    //-------------------------------------------------------------ATTACK
    void Attack()
    {
        if (isAttack) return;

        anim.speed = attackSpeed;
        anim.SetTrigger("Attack");
        StartCoroutine(AttackRoutine());
        //StartCoroutine(AttackCooldown());
    }
    void SetAttackDmg()
    {
        atkDamage = GameLogic.CalculatePlayerBaseAttackDmg(this) + weaponDmg + bonusDmg;
    }
    void DealDamage(GameObject who)
    {
        if (who == this.gameObject)
        {
            Debug.Log("deal damage");
            GetEnemiesInRangeForAttack();
            foreach (Transform enemy in enemiesInRangeAttack)
            {
                EnemyController ec = enemy.GetComponent<EnemyController>();
                if (ec == null) continue;
                ec.getHit(atkDamage);
            }
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
    public void GetEnemiesInRangeForAttack()
    {
        enemiesInRangeAttack.Clear();
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * 1f), 1f))
            if(c.gameObject.CompareTag("Enemy"))
            {
                enemiesInRangeAttack.Add(c.transform);
            }
    }

    public List<Transform> GetEnemiesInRange()
    {
        enemiesInRange_.Clear();
        foreach (Collider c in Physics.OverlapSphere(transform.position, 10f))
            if (c.gameObject.CompareTag("Enemy"))
            {
                enemiesInRange_.Add(c.transform);
            }
        return enemiesInRange_;
    }
    //-------------------------------------------------------------GETHIT
    public void GetHit(float dmg)
    {
        if (dead) return;
        anim.SetTrigger("GetHit");
        curHealth -= dmg;

        hpbar.Find("Fill_bar").GetComponent<Image>().fillAmount = curHealth / totalHealth;
        hpbar.Find("Text").GetComponent<Text>().text = "HP " + curHealth + "/" + totalHealth;


        if (curHealth <= 0)
        {
            Die();
            canMove = false;
            return;
        }

        GetIncapacitated(0.2f);
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
    //-------------------------------------------------------------DIE
    void Die()
    {
        dead = true;
        anim.SetTrigger("IsDead");
    }
    //-------------------------------------------------------------EXP/LEVEL
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

        //Debug.Log("expNeeded: "+ expNeeded + "experience: " +Experience);
    }

    void LevelUp()
    {
        level++;
        levelText.text = "Lv. " + level.ToString("00");
    }
}
