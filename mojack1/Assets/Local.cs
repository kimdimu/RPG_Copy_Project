using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Local : MonoBehaviour
{
    Vector3 localforward;
    void Update()
    {
        localforward = transform.forward;
        //Debug.Log(transform.InverseTransformVector(transform.forward));
    }
}
