using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneState : State<Player>
{
    static private NoneState instance = null;
    static public NoneState Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject();
                container.name = "statebaby_Container";
                instance = container.AddComponent(typeof(NoneState)) as NoneState;
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
        Debug.Log("Enter NoneState");
    }
    public override void Execute(Player player)
    {
    }
    public override void Exit(Player player)
    {
    }
}
