using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public delegate void AnimationEvent();
    //public static event AnimationEvent OnSlashAnimationHit;
    public static AnimationEvent OnSlashAnimationHit;
    //public static event EventHandler a;
    void OnExecuteSlashAniEvent()
    {
        OnSlashAnimationHit();
    }

    public delegate void CanMoveEvent();
    public static CanMoveEvent OnOffMove;

   public void OnExecuteOnOffMove()
    {
        OnOffMove();
    }

}
