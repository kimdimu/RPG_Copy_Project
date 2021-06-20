using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMouse : MonoBehaviour
{
    Ray ray;
    RaycastHit groundHit;
    RaycastHit buildHit;
    GridGetNear grid;
    public static GameObject img;
    public static GameObject prefab;
    public static float row, col;
    public static bool isBuild;
    Vector3 realpos;
    public static Vector3 originLocalScale;

    private void Awake()
    {
        grid = FindObjectOfType<GridGetNear>();
        img = GameObject.Find("placelocate");
        originLocalScale = img.transform.localScale;
        isBuild = false;
    }
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out groundHit, 100000, 1 << 16))
        {

            realpos = grid.GetNearGridPoint2up(groundHit.point, row, col);
            img.transform.position = realpos;

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out buildHit, 100000, 1 << 17))
                    Debug.Log("this is BO");
                else
                {
                    Debug.Log("groundHit.point"+groundHit.point);
                    PlaceNear(groundHit.point);
                }
            }
        }
    }
    //void Update()
    //{
    //    RaycastHit hitInfo;
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out hitInfo, 100000, 1 << 16))
    //    {
    //        realpos = grid.GetNearGridPoint2up(hitInfo.point, row, col);
    //        img.transform.position = realpos;

    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            if (Physics.Raycast(ray, out hitInfo))
    //                Debug.Log(hitInfo.collider.tag);

    //            if (hitInfo.collider.tag != "BuildObj")
    //            {
    //                if (Physics.Raycast(ray, out hitInfo, 100000, 1 << 16))
    //                    PlaceNear(hitInfo.point);
    //            }
    //            else
    //                Debug.Log("this is BO");
    //        }
    //    }
    //}
    public static void Getprefab(float r, float c, GameObject fab)
    {
        row = r;
        col = c;
        prefab = fab;
        img.transform.localScale = new Vector3(originLocalScale.x * col, originLocalScale.y, originLocalScale.z * row);
        Debug.Log("getfab " + r +" "+ c);
    }

    void PlaceNear(Vector3 point)
    {
        Vector3 nearpos = grid.GetNearGridPoint2up(point,row,col);
        GameObject fab = Instantiate(prefab);
        fab.transform.position = nearpos;
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = nearpos;
    }
}
