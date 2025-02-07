using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    // private GameObject[] wayPointList = new GameObject[10];
    private List<Transform> wayPointList = new List<Transform>();
    //private Component[] wayPointList;
    public Transform waypointMasterPrefab;
    private int waypointsLoaded = 0;

    // Use this for initialization
    void Start()
    {


    }

    void Awake()
    {
        //GenerateWaypoints ();

        DetectWayPoints();
        ShowWayPoints();
    }

    //if no waypoints present then generate some
    public void GenerateWaypoints()
    {
        for (int i = 0; i < 5; i++)
        {
            Transform ob = Instantiate(waypointMasterPrefab, this.transform.position, Quaternion.identity) as Transform;
            ob.Translate(i * 0.5f, 0, 0);
            wayPointList.Add(ob);
            waypointsLoaded++;
            //Debug.Log("Path:Path has added waypoint#" + waypointsLoaded);
        }
    }

    //if waypoints are added manually as child objects then find them
    public void DetectWayPoints()
    {
        Component[] tempList;
        tempList = GetComponentsInChildren<WayPoint>();
        waypointsLoaded = 0;
        foreach (WayPoint cmp in tempList)
        {
            Debug.Log("//component found:" + cmp);
            wayPointList.Add(cmp.gameObject.transform);
            waypointsLoaded++;
        }
        Debug.Log("//Path:waypoints found: " + waypointsLoaded);
    }

    private void ShowWayPoints()
    {
        for (int i = 0; i < waypointsLoaded; i++)
        {
            Debug.Log("%%" + wayPointList[i].position);
        }
    }
    public Vector3 GetPositionOfWP(int index)
    {
        return wayPointList[index].position;
    }

    public int GetNumWaypoints()
    {
        return waypointsLoaded;
    }


    private void VisualisePath()
    {
        //draw lines between the waypoints
        for (int i = 0; i < waypointsLoaded; i++)
        {
            Vector3 startpos = wayPointList[i].transform.position;
            int endIndex;
            if (i == (waypointsLoaded - 1))
            {
                endIndex = 0;
            }
            else
            {
                endIndex = i + 1;
            }
            //Debug.Log("draw line from item" + i + ",to item " + endIndex);
            Vector3 endpos = wayPointList[endIndex].transform.position;
            Debug.DrawLine(startpos, endpos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        VisualisePath();

    }//end update
}//end class


