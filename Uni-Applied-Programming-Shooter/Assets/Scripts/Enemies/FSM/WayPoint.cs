using UnityEngine;
//this is part of the NodePrefab

public class WayPoint : MonoBehaviour
{


    private bool bDragging;
    private Vector3 screenPoint;


    void Start()
    {
        bDragging = false;
    }

    // Update is called once per frame
    void Update()
    {

    }




    void OnMouseDown()
    {
        bDragging = true;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }




    void OnMouseDrag()
    {
        if (bDragging)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            transform.position = new Vector3(curPosition.x, 0, curPosition.z);
        }


    }

    void OnMouseUp()
    {
        bDragging = false;
    }


}
