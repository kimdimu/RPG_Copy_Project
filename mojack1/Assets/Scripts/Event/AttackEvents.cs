using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvents : MonoBehaviour
{
    public delegate void AttackEvent(GameObject me);
    public static AttackEvent HitEnemyEvent;
    void OnHitEnemyEvent()
    {
        HitEnemyEvent(this.transform.parent.gameObject);
    }
}
