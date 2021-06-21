using System.Collections.Generic;
using UnityEngine;


//상태 이넘!
public enum SteeringState
{
    NONE, SEEK, FLEE, ARRIVE, PURSUIT, EVADE, WANDER, ATTACKMOVE, COHESION, SEPARATION, HIDEBACK, INTERPOS, END
}
public enum Summing_method { none, weighted_average, prioritized, dithered };

public class SteeringBehavior
{
    private bool[] OnState;
    Summing_method summing_Method_;
    Player playerAI; // 나
    GameObject evader; // 적 기체
    Vector3 steeringF; //힘
    Vector3 zero; // 벡터 0,0,0
    Vector3 target;//wwander

    float wanderRadius; //원의 반경
    float wanderDist; //원이 투사되는 거리. 원 중심과의 거리.
    float wanderJitter; // 무작위 변위의 최대 크기
    Vector3 wanderTarget; //무작위변위를 더할 벡터
    float playerAngle;
    float angleGoal;

    float time; //시간
    bool isReady; //공격준비상태인가?
    bool isAttack;//공격상태인가?

    public SteeringBehavior()//, Summing_method summing_Method, SteeringState steeringState)
    {
        OnState = new bool[(int)SteeringState.END];
        time = 0;
        wanderJitter = 0.5f;
        zero = new Vector3(0, 0, 0);
        evader = new GameObject();

        summing_Method_ = Summing_method.none;
    }
    public void SetTargetPlayer(Player Agent)
    {
        playerAI = new Player();
        playerAI = Agent;
    }
    public void SetTargetAgent1(GameObject Agent) { evader = Agent; }
    public void SetWander(float rad, float dist, float jit)
    {
        wanderRadius = rad;
        wanderDist = dist;
        wanderJitter = jit;
    }

    public bool On(SteeringState state) { return OnState[(int)state]; }

    #region ONOFF
    public void FleeOn() { OnState[(int)SteeringState.FLEE] = true; }
    public void SeekOn() { OnState[(int)SteeringState.SEEK] = true; }
    public void PursuitOn() { OnState[(int)SteeringState.PURSUIT] = true; }
    public void EvadeOn() { OnState[(int)SteeringState.EVADE] = true; }
    public void WanderOn() { OnState[(int)SteeringState.WANDER] = true; }
    public void ArriveOn() { OnState[(int)SteeringState.ARRIVE] = true; }
    public void SeparationOn() { OnState[(int)SteeringState.SEPARATION] = true; }
    public void CohetionOn() { OnState[(int)SteeringState.COHESION] = true; }
    public void AttackmoveOn() { OnState[(int)SteeringState.ATTACKMOVE] = true; }
    public void HideBackOn() { OnState[(int)SteeringState.HIDEBACK] = true; }
    public void InterposeOn() { OnState[(int)SteeringState.INTERPOS] = true; }
    public void FleeOff() { OnState[(int)SteeringState.FLEE] = false; }
    public void SeekOff() { OnState[(int)SteeringState.SEEK] = false; }
    public void PursuitOff() { OnState[(int)SteeringState.PURSUIT] = false; }
    public void EvadeOff() { OnState[(int)SteeringState.EVADE] = false; }
    public void WanderOff() { OnState[(int)SteeringState.WANDER] = false; }
    public void ArriveOff() { OnState[(int)SteeringState.ARRIVE] = false; }
    public void SeparationOff() { OnState[(int)SteeringState.SEPARATION] = false; }
    public void CohetionOff() { OnState[(int)SteeringState.COHESION] = false; }
    public void AttackmoveOff() { OnState[(int)SteeringState.ATTACKMOVE] = false; }
    public void HideBackOff() { OnState[(int)SteeringState.HIDEBACK] = false; }
    public void InterposeOff() { OnState[(int)SteeringState.INTERPOS] = false; }
    #endregion

    public Vector3 Calculate()
    {
        steeringF = zero;

        switch (summing_Method_)
        {
            case Summing_method.none:
                {
                    if (On(SteeringState.SEEK))
                        steeringF += Seek(playerAI.target.transform.position);
                    if (On(SteeringState.FLEE))
                        steeringF += Flee(playerAI.target.transform.position);
                    if (On(SteeringState.ARRIVE))
                        steeringF += Arrive(playerAI.target.transform.position, 2);
                    if (On(SteeringState.PURSUIT))
                        steeringF += Pursuit(playerAI.target.GetComponent<Player>());
                    if (On(SteeringState.EVADE))
                        steeringF += Evade(playerAI.target.GetComponent<Player>());
                    if (On(SteeringState.WANDER))
                        steeringF += Wander();
                    if (On(SteeringState.ATTACKMOVE))
                        steeringF += AttackMove(playerAI.target);
                    if (On(SteeringState.COHESION))
                        steeringF += Cohesion(playerAI.GetTargets(10f));
                    if (On(SteeringState.SEPARATION))
                        steeringF += Separation(playerAI.GetTargets(1f));
                    if (On(SteeringState.HIDEBACK))
                        steeringF += HideBack(playerAI.mainPlayer.GetComponent<PlayerController>().GetEnemiesInRange());
                    if (On(SteeringState.INTERPOS))
                        steeringF += Interpose(playerAI.mainPlayer.GetComponent<PlayerController>().GetEnemiesInRange());
                }
                break;
            case Summing_method.prioritized:
                {

                }
                break;
        }
        //Debug.Log(steeringF);

        return steeringF;
    }

    Vector3 Seek(Vector3 targetPos)
    {
        Vector3 playerVelo = (targetPos - playerAI.transform.position).normalized;

        //속도 리턴. 정규화된 방향 
        return playerVelo;
    }
    Vector3 Flee(Vector3 targetPos)
    {
        Vector3 playerVelo = (playerAI.transform.position - targetPos).normalized;

        //속도 리턴
        return playerVelo;
    }
    Vector3 Arrive(Vector3 targetPos, float deceleration)
    {
        Vector3 Totarget = (targetPos - playerAI.transform.position);

        float dist = Totarget.magnitude;

        if (dist > 0)
        {
            float DecelerationTweaker = 0.3f;

            float speed = dist / deceleration * DecelerationTweaker;

            Vector3 DesireV;
            DesireV.x = Totarget.x * speed / dist;
            DesireV.y = Totarget.y * speed / dist;
            DesireV.z = Totarget.z * speed / dist;

            return DesireV;
        }

        //속도 리턴
        return Vector3.zero;
    }
    Vector3 Pursuit(Player evader)
    {
        //도피자를 향한 벡터
        Vector3 toEv = evader.transform.position - playerAI.transform.position;
        //상대적인 앞위치? 플레이어 방향과 도피자 방향의 내적값.
        float RelativeHeading = Vector3.Dot(playerAI.transform.forward, evader.transform.forward);
        //도피자로 가는 벡터와 플레이어방향의 내적이 0보다 크다. 자신 앞에 있다면.
        //두 방향의 내적값이 0.95보다 작다.
        if (Vector3.Dot(toEv, playerAI.transform.forward) > 0 && RelativeHeading < -0.95f)
        {
            return Seek(evader.transform.position);
        }
        float LookAheadTime = toEv.magnitude / (playerAI.movingEntity.m_dMaxSpeed + evader.speed);
        return Seek(evader.transform.position + evader.movingEntity.m_vVelocity * LookAheadTime);
    }
    Vector3 Evade(Player pursuer)
    {
        Vector3 toPs = pursuer.transform.position - playerAI.transform.position;
        float LookAheadTime = toPs.magnitude / (playerAI.movingEntity.m_dMaxSpeed + pursuer.speed);
        return Flee(pursuer.transform.position + pursuer.movingEntity.m_vVelocity * LookAheadTime);
    }
    Vector3 Wander()
    {
        float difference;
        time -= Time.deltaTime;
        if (time < 0f)
        {
            //Debug.Log("초기화~ 단번에 늒껴");

            wanderTarget = zero; //초기화
                                 //소량의 무작위 벡터를 목표물 위치에 더한다.
            wanderTarget.x += Random.Range(-1f, 1f) * wanderJitter;
            wanderTarget.z += Random.Range(-1f, 1f) * wanderJitter;
            //정규화 후 원의 반지름에 맞춘다.
            wanderTarget.Normalize();
            wanderTarget *= wanderRadius;

            playerAngle = playerAI.transform.eulerAngles.y;
            // Debug.Log(playerAngle);
            angleGoal = Mathf.Pow(Random.Range(0.0f, 90f), 2);
            time = 2;
        }
        difference = playerAI.transform.eulerAngles.y - playerAngle;
        if (difference > 180)
        {
            difference = 360 - difference;
        }
        if (Mathf.Pow(difference, 2) >= angleGoal)
        {
            return zero;
        }

        //반지름에 맞춘 위치에 현재 보고있는 방향 * 투사 거리만큼 더해준다. + 현재 위치 더해서 월드 좌표로 옮기기
        target.x = wanderTarget.x + playerAI.transform.position.x + playerAI.transform.forward.x * wanderDist;
        target.y = playerAI.transform.position.y;
        target.z = wanderTarget.z + playerAI.transform.position.z + playerAI.transform.forward.z * wanderDist;
        //그 쪽으로 간다.
        Vector3 Velo = (target - playerAI.transform.position).normalized;
        return Velo;
    }
    Vector3 AttackMove(GameObject target)
    {
        if (isReady) // 공격 준비상태인가?
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                isReady = false;
                isAttack = true;
                time = 0;
            }
            return zero; // 움직이지 않는다. 회전은 한다.
        }
        else if (isAttack) // 공격중인가?
        {
            time += Time.deltaTime;
            if (time > 0.3f)
            {
                isReady = false;
                isAttack = false;
                time = 0;
            }
            Vector3 jumppower = target.transform.position - playerAI.transform.position;
            jumppower.Normalize();
            jumppower *= 10; //빠른 속도로 전진한다.

            return jumppower;
        }
        else
        {
            //거리가 10보다 작다면
            if (Vector3.Distance(target.transform.position, playerAI.transform.position) < 10)
            {
                isReady = true; // 공격 준비 상태로 전환
                return zero; // 0 움직이지 않는다.
            }
            //거리가 10 이상이라면 
            else
            {
                isReady = false;
                isAttack = false;
                return Seek(target.transform.position);

            }

        }
    }
    Vector3 Cohesion(List<GameObject> target)
    {
        Vector3 mass = new Vector3();
        Vector3 sforce = new Vector3();
        int targetsCount = 0;

        for (int i = 0; i < target.Count; ++i)
        {
            if (target[i].gameObject.CompareTag("Player") && target[i] != playerAI.gameObject)
            {
                mass += target[i].transform.position;
                ++targetsCount;
            }
        }
        if (targetsCount > 0)
        {
            //mass /= targetsCount;
            //sforce = Seek(mass);
            mass /= targetsCount;
            Vector3 toTarget = mass - playerAI.transform.position;
            sforce += toTarget.normalized / (toTarget.magnitude);//거리가 길어질수록 힘이 적게 추가됨
            //sforce = Seek(mass);
        }

        return sforce;
    }
    Vector3 Separation(List<GameObject> target)
    {
        Vector3 sforce = new Vector3();
        for (int i = 0; i < target.Count; ++i)
        {
            if (target[i].gameObject.CompareTag("Player") && target[i] != playerAI.gameObject)
            {
                //타겟(다른놈)이 플레이어에게 가는 방향. 즉 도망침
                Vector3 toPlayer = playerAI.transform.position - target[i].transform.position;
                sforce += toPlayer.normalized / (toPlayer.magnitude);//거리가 길어질수록 힘이 적게 추가됨
                                                                     //Debug.Log(sforce);
            }
        }
        return sforce;
    }

    Vector3 HideBack(List<Transform> target)//인자로 장애물의 위치를 받는다.
    {
        float DistanceFromBoundary = 1;
        float DistAway = 1 + DistanceFromBoundary; // 5 = 플레이어 둘레 + 플레이어와 떨어질 거리
        Vector3 ToTarget = zero;
        int count = 0;
        for (int i = 0; i < target.Count; ++i)
        {
            ToTarget += (playerAI.mainPlayer.transform.position - target[i].position).normalized;
            count++;
        }
        Debug.Log(count);
        if (count == 0)
            return zero;
        Vector3 hidingSpot = (ToTarget.normalized) * DistAway + playerAI.mainPlayer.transform.position ;
            Debug.Log(hidingSpot);
        return Arrive(hidingSpot, 1);
    }

    //호출할 때 가까운 적이 존재 할때만 호출해야 할듯..
    Vector3 Interpose(List<Transform> target)
    {
        Vector3 midPoint = zero; //가장 가까운 적과 메인플레이어의 위치.
        Vector3 nearest = zero; //가장 가까운 적의 위치를 담는다.
        int count = 0;
        for (int i = 0; i < target.Count; ++i)
        {
            if (Vector3.Distance(target[i].position, playerAI.mainPlayer.transform.position)< Vector3.Distance(nearest, playerAI.mainPlayer.transform.position))
            {
                nearest = target[i].position;
                ++count;
            }
        }
        midPoint = (nearest + playerAI.mainPlayer.transform.position) /2;
        if (count==0)
            return zero;

        return Arrive(midPoint,1);
    }
}