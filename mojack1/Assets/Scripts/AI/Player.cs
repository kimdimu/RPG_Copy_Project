﻿using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MovingEntity movingEntity;
    SteeringBehavior steeringBehavior;
    public Stats stats;

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
        steeringBehavior = new SteeringBehavior(this);
        steeringBehavior.SeparationOn();
        movingEntity = new MovingEntity();
        steeringBehavior.SetTargetAgent1(target);
        steeringBehavior.SetWander(wanderRadius, wanderDist, wanderJitter);
    }

    void Update()
    {
        transform.LookAt(transform.position + movingEntity.m_vVelocity * Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, target.transform.position) < 2.5f)
        {
            Debug.Log("CohetionOff, dis < 2.5");
            steeringBehavior.CohetionOff();
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < 5)
        {
            Debug.Log("SeekOff, dis < 5");
            steeringBehavior.SeekOff();
            steeringBehavior.WanderOn();
        }
        else
        {
            Debug.Log("CohetionOn, SeekOn");
            steeringBehavior.CohetionOn();
            steeringBehavior.SeekOn();
            steeringBehavior.WanderOff();
        }


        steeringForce = steeringBehavior.Calculate();

        acceleration = steeringForce;// /mass;
        movingEntity.m_vVelocity = acceleration * Time.deltaTime; //가 = 속도변화량 / 시
        transform.position += movingEntity.m_vVelocity * Time.deltaTime * speed; // 거 = 속 * 시
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

    public void ChangeState(SteeringState steeringState)
    {

    }
}