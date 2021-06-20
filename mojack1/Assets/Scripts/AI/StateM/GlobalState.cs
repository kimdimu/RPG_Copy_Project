using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : State<Player>
{
    static private GlobalState instance = null;
    static public GlobalState Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "statebaby_Container";
                instance = container.AddComponent(typeof(GlobalState)) as GlobalState;
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
        //if (Vector3.Distance(player.transform.position, player.target.transform.position) > 15)
        if (player.attackState == 1)
        {
            player.GetFSM().ChangeState(AttackEnemy.Instance);
        }
        else if(player.hideState == 0 && player.stats.HP <=20 && player.mainPlayer.GetComponent<PlayerController>().curHealth>80)
        {
            player.GetFSM().ChangeState(HideBack.Instance);

        }
        else if (player.alertState == 1 && player.attackState == 0)
            player.GetFSM().ChangeState(Alert.Instance);
    }
    public override void Exit(Player player)
    {
    }
}

