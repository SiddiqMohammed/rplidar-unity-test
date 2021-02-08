using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RplidarTest : MonoBehaviour
{

    public string port;

    private LidarData[] data;

    private void Awake()
    {
        data = new LidarData[720];
    }

    // Use this for initialization
    void Start()
    {
        getAllData();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {

        DrawButton("BEGIN", () =>
        {
            if (string.IsNullOrEmpty(port))
            {
                return;
            }

            RplidarBinding.OnConnect(port);
            RplidarBinding.StartMotor();
            RplidarBinding.StartScan();
        });

        DrawButton("Connect", () =>
        {
            if (string.IsNullOrEmpty(port))
            {
                return;
            }

            int result = RplidarBinding.OnConnect(port);

            Debug.Log("Connect on " + port + " result:" + result);
        });

        DrawButton("DisConnect", () =>
        {
            bool r = RplidarBinding.OnDisconnect();
            Debug.Log("Disconnect:" + r);
        });

        DrawButton("StartScan", () =>
        {
            bool r = RplidarBinding.StartScan();
            Debug.Log("StartScan:" + r);
        });

        DrawButton("EndScan", () =>
        {
            bool r = RplidarBinding.EndScan();
            Debug.Log("EndScan:" + r);
        });

        DrawButton("StartMotor", () =>
        {
            bool r = RplidarBinding.StartMotor();
            Debug.Log("StartMotor:" + r);
        });

        DrawButton("EndMotor", () =>
        {
            bool r = RplidarBinding.EndMotor();
            Debug.Log("EndMotor:" + r);
        });


        DrawButton("Release Driver", () =>
        {
            bool r = RplidarBinding.ReleaseDrive();
            Debug.Log("Release Driver:" + r);
        });


        DrawButton("GrabData", () =>
        {
            int count = RplidarBinding.GetData(ref data);

            Debug.Log("GrabData:" + count);

            print("count " + count);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    // Debug.Log("d:" + data[i].distant + " " + data[i].theta);
                    if (data[i].theta > 265 && data[i].theta < 275)
                    {
                        Debug.Log("d:" + data[i].distant + " " + data[i].theta);
                    }
                }
            }
        });
    }

    void DrawButton(string label, Action callback)
    {
        if (GUILayout.Button(label, GUILayout.Width(200), GUILayout.Height(75)))
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }

    void getAllData()
    {
        RplidarBinding.OnConnect(port);
        RplidarBinding.StartMotor();
        RplidarBinding.StartScan();

        int count = RplidarBinding.GetData(ref data);

        Debug.Log("GrabData:" + count);

        print("count " + count);
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                // Debug.Log("d:" + data[i].distant + " " + data[i].theta);
                if (data[i].theta > 265 && data[i].theta < 275)
                {
                    Debug.Log("d:" + data[i].distant + " " + data[i].theta);
                }
            }
        }
    }
}
