using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : State<Player>
{
    static private AttackEnemy instance = null;
    static public AttackEnemy Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "statebaby_Container";
                instance = container.AddComponent(typeof(AttackEnemy)) as AttackEnemy;
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
        player.attackState = 2;
        //player.steeringBehavior.AttackmoveOn();
    }
    public override void Execute(Player player)
    {
        player.Attack();
        if (Vector3.Distance(player.transform.position, player.mainPlayer.transform.position) >= 5
            ||player.attackState ==0)
        {
            player.GetFSM().ChangeState(BackToPlayer.Instance);
        }
    }
    public override void Exit(Player player)
    {
        player.attackState = 0;
        //Alert.backUpTarget();
        //player.steeringBehavior.AttackmoveOn();
    }


}
