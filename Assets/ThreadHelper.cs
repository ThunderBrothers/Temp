using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadHelper : MonoBehaviour
{
    public static ThreadHelper Instance;
    public static List<Action> actionlist = new List<Action>();

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (actionlist.Count > 0)
        {
            for (int i = 0;i < actionlist.Count;i++)
            {
                actionlist[i].Invoke();
            }
            actionlist.Clear();
        }
       

    }
}
