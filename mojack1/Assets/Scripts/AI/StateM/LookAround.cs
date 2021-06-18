using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : State<Player>
{
    static private LookAround instance = null;
    static public LookAround Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "statebaby_Container";
                instance = container.AddComponent(typeof(LookAround)) as LookAround;
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
            Debug.Log("Enter LOOKAROUND");
        player.steeringBehavior.WanderOn();
    }
    public override void Execute(Player player)
    {
        if (Vector3.Distance(player.transform.position, player.target.transform.position) >= 5)
        {
            player.GetFSM().ChangeState(BackToPlayer.Instance);
        }
    }
    public override void Exit(Player player)
    {
        player.steeringBehavior.WanderOff();
    }
}
