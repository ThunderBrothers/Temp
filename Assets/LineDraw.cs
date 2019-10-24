using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class LineDraw : MonoBehaviour
{
    public LineSet myLineSet;
    public bool isDrag = false;
    public float checkDragTime = 0f;
    public LineRenderer lineRender;
    private List<Vector3> points = new List<Vector3>();
    public Vector3 curDrawPos;
    private Vector3 lastDrawPos;
    private float dis;
    private Vector3 tempVec3;
    private Ray ray;
    private RaycastHit hit;

    private Dictionary<LineSet, LineRenderer> allDrawnLines = new Dictionary<LineSet, LineRenderer>();

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
                        ray = Camera.main.ScreenPointToRay(lastDrawPos);
                        if (Physics.Raycast(ray,out hit))
                        {
                            if (hit.collider != null)
                            {
                                points.Add(hit.point - Vector3.forward);
                            }
                        }
                        myLineSet.nodes.Add(lastDrawPos);
                        lineRender.positionCount = points.Count;
                        lineRender.SetPositions(points.ToArray());
                    }
                }
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
        myLineSet.Reset();
        myLineSet.owner = SocketConnect.Instance.identityMark;
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
        GameObject tempObj = new GameObject("Line" + myLineSet.owner);
        tempObj.transform.parent = lineRender.transform;
        tempObj.transform.localPosition = Vector3.zero;
        tempObj.transform.localRotation = Quaternion.identity;
        LineRenderer tempLine = tempObj.AddComponent<LineRenderer>();
        tempLine.endColor = myLineSet.color;
        tempLine.startColor = myLineSet.color;
        tempLine.startWidth = myLineSet.width;
        tempLine.endWidth = myLineSet.width;
        tempLine.positionCount = points.Count;
        tempLine.SetPositions(points.ToArray());


        allDrawnLines.Add(myLineSet, tempLine);

        
        myLineSet = new LineSet();
        points.Clear();
        lineRender.SetPositions(points.ToArray());
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

    private string PackLineData()
    {
        JsonConvert.DeserializeObject<LineSet>();
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

    public void Reset()
    {
        owner = "";
        color = Color.red;
        width = 100;
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        nodes = new List<Vector3>();
    }
}
