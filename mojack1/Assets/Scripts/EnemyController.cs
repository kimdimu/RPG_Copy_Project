using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator anim;
    public float totalHealth;
    public float curHealth;
    public float expGranted;
    public float atkDamage;
    public float atkSpeed;
    public float moveSpeed;
    bool dead;

    private GameObject[] players;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        curHealth = totalHealth;
    }

    void Update()
    {
    }
    public void getHit(float dmg)
    {
        if (dead) return;

        anim.SetTrigger("GetHit");
        anim.SetInteger("Condition", 100);
        curHealth -= dmg;
        //Debug.Log(curHealth);

        if(curHealth<=0)
        {
            Die();
            return;
        }

        StartCoroutine(RecoverFromHit());
    }

    void Die()
    {
        dead = true;
        DropLoot();

        foreach(GameObject go in players)
        {
            Debug.Log("foreach");
            go.GetComponent<PlayerController>().SetExp(expGranted / players.Length);
        }

        anim.SetTrigger("IsDead");
        Destroy(this.gameObject, 3);
    }

    void DropLoot()
    {
        print("You Get the bounty");
    }

    IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetInteger("Condition", 0);
    }
}
