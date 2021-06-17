using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State<Player>
{
    static private Idle instance = null;
    static public Idle Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "statebaby_Container";
                instance = container.AddComponent(typeof(Idle)) as Idle;
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
    }
    public override void Execute(Player player)
    {
        if (Vector3.Distance(player.transform.position, player.target.transform.position) >= 5)
        {
            player.GetFSM().ChangeState(BackToPlayer.Instance);
        }
        else
        {
            player.GetFSM().ChangeState(LookAround.Instance);
        }
    }
    public override void Exit(Player player)
    {
    }
}
