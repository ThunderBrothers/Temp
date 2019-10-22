using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Material material;
    private bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 mousePosition = Vector3.zero;
        if (isDragging)
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
        }
        else
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        }
        if (Input.GetMouseButton(0))
        {
            if (material != null)
            {
                material.SetVector("iMouse", mousePosition);
            }
        }

       

        material.SetFloat("iTime", Time.time);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.richText = true;
        if (GUI.Button(new Rect(10, 10, 100, 30),"<color=white>Exit</color>", style))
        {
            Application.Quit();
        }
    }
}
