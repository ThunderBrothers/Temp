using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LineDraw : MonoBehaviour
{
    public LineSet myLineSet;
    public bool isDrag = false;
    public float checkDragTime = 0f;
    public Vector3 curDrawPos;
    private Vector3 lastDrawPos;
    private float dis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (isDrag)
            {
                dis = Vector3.Distance(curDrawPos, lastDrawPos);
                Debug.Log(dis);
                if (dis > 20f)
                {
                    dis = 0;
                    lastDrawPos = curDrawPos;
                    if (!myLineSet.nodes.Contains(lastDrawPos))
                    {
                        myLineSet.nodes.Add(lastDrawPos);
                    }
                }
            }else {

            }
        }
    }
    private void OnMouseDrag()
    {
        curDrawPos = Input.mousePosition;
    }

    void OnMouseDown()
    {
        myLineSet = new LineSet();
        isDrag = true;
        curDrawPos = Input.mousePosition;
        myLineSet.nodes.Add(curDrawPos);
        lastDrawPos = curDrawPos;
        myLineSet.startPos = curDrawPos;
    }

    void OnMouseUp()
    {
        isDrag = false;
        myLineSet.endPos = curDrawPos;
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && isDrag)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(myLineSet.startPos, 0.05f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(myLineSet.endPos, 0.05f);

            Gizmos.color = Color.green;
            if (myLineSet.nodes.Count > 1)
            {
                for (int i = 0;i < myLineSet.nodes.Count - 1;i++)
                {
                    Gizmos.DrawLine(myLineSet.nodes[i], myLineSet.nodes[i + 1]);
                }
            }
        }
    }
}
[Serializable]
public class LineSet
{
    public string owner;
    public Color color;
    public float width;
    public Vector3 startPos;
    public Vector3 endPos;
    public List<Vector3> nodes = new List<Vector3>();
}
