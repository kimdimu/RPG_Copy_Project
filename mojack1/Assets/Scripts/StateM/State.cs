using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<entity_type> : MonoBehaviour
{
    //public SteeringBehavior steeringBehavior;
    public Vector3 zero = new Vector3(0, 0, 0);

    public abstract void Enter(entity_type entity_Type);
    public abstract void Execute(entity_type entity_Type);
    public abstract void Exit(entity_type entity_Type);

}
