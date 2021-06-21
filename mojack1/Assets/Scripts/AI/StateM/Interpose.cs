using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpose : State<Player>
{
    static private Interpose instance = null;
    static public Interpose Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "statebaby_Container";
                instance = container.AddComponent(typeof(Interpose)) as Interpose;
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
        Debug.Log("Enter Interpose");
        player.interposeState = 1;
        player.steeringBehavior.InterposeOn();
    }
    public override void Execute(Player player)
    {
        if (player.mainPlayer.GetComponent<PlayerController>().GetEnemiesInRange().Count <=0)
        {
            player.GetFSM2().ChangeState(NoneState.Instance);
        }
    }
    public override void Exit(Player player)
    {
        player.steeringBehavior.InterposeOff();
        player.interposeState = 0;
    }
}