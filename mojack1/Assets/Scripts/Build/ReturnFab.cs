using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFab : MonoBehaviour
{
    public GameObject fab;
    public float row;
    public float col;
    public void RF()
    {
        Debug.Log("returnfab " + row + " " + col);
        GridMouse.Getprefab(row, col, fab);
    }
}
