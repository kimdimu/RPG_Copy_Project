using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : State<Player>
{
    public delegate void BackUpTargetEvent();
    public static BackUpTargetEvent backUpTarget;
    void OnHitEnemyEvent()
    {
        backUpTarget();
    }

    float time;
    static private Alert instance = null;
    static public Alert Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "statebaby_Container";
                instance = container.AddComponent(typeof(Alert)) as Alert;
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    GameObject prevTarget;

    public override void Enter(Player player)
    {
        player.alertState = 2;
        time = 0;
        prevTarget = player.target;
        player.steeringBehavior.SeekOn();
    }
    public override void Execute(Player player)
    {
        time += Time.deltaTime;
        Debug.Log("Alert");
        if (time >= 0.5f || player.target == player.mainPlayer)
        {
            player.GetFSM().ChangeState(BackToPlayer.Instance);
        }
    }
    public override void Exit(Player player)
    {
        player.alertState = 0;
        backUpTarget();
    }
}
