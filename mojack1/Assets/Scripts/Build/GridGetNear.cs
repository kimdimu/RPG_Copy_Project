using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGetNear : MonoBehaviour
{
    public float size = 1f;

    public Vector3 GetNearGridPoint(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x / size);
        int y = Mathf.RoundToInt(pos.y / size);
        int z = Mathf.RoundToInt(pos.z / size);

        Vector3 result = new Vector3((float)x * size, (float)y * size, (float)z * size);

        return result;
    }

    public Vector3 GetNearGridPoint2up(Vector3 pos,float row, float col)
    {
        float x = Mathf.RoundToInt(pos.x / size);
        //float y = Mathf.RoundToInt(pos.y / size);
        float y = pos.y;
        float z = Mathf.RoundToInt(pos.z / size);

        if (col % 2 == 0)
        {
            if (x - pos.x > 0)
                x -= size / 2;
            else
                x += size / 2;

        }
        if (row % 2 == 0)
        {
            if (z - pos.z > 0)
                z -= size / 2;
            else
                z += size / 2;
        }

        Vector3 result = new Vector3(x * size, y+0.001f/* * size*/, z * size);
        //Debug.Log("result: " + result);

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = 0; x < 40; x += size)
        {
            for (float z = 0; z < 40; z += size)
            {
                //Vector3 point = GetNearGridPoint(new Vector3(x, 0f, z));
                //Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}
