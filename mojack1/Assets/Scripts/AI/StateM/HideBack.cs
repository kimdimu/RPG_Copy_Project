using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBack : State<Player>
{
    static private HideBack instance = null;
    static public HideBack Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "statebaby_Container";
                instance = container.AddComponent(typeof(HideBack)) as HideBack;
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
        Debug.Log("Enter HideBack");
        player.steeringBehavior.HideBackOn();
        player.hideState = 1;
    }
    public override void Execute(Player player)
    {

        if(player.stats.HP>20)
        {
            player.GetFSM().ChangeState(LookAround.Instance);
        }
    }
    public override void Exit(Player player)
    {
        player.steeringBehavior.HideBackOff();
        player.hideState = 0;
    }
}
