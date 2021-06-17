using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToPlayer : State<Player>
{
    static private BackToPlayer instance = null;
    static public BackToPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "statebaby_Container";
                instance = container.AddComponent(typeof(BackToPlayer)) as BackToPlayer;
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public override void Enter(Player player)
    {
        player.steeringBehavior.SeekOn();
    }
    public override void Execute(Player player)
    {
        player.steeringBehavior.SeekOn();
        Debug.Log("BACKTOPLAYER");
        if (Vector3.Distance(player.transform.position, player.target.transform.position) < 5)
        {
            player.GetFSM().ChangeState(LookAround.Instance);
        }
    }
    public override void Exit(Player player)
    {
        player.steeringBehavior.SeekOff();
    }
}
