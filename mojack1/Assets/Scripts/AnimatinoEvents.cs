using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatinoEvents : MonoBehaviour
{
    public delegate void AnimationEvent();
    public static event AnimationEvent OnSlashAnimationHit;

    void SlashaniHitEvent()
    {
        Debug.Log("she");
        OnSlashAnimationHit();
    }
}
