using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RplidarTest : MonoBehaviour
{

    List<int> angles_list_0 = new List<int>();
    List<int> angles_list_1 = new List<int>();


    // distances of each touchpoint (in mm)
    int[] tp_distances = { 360, 240 };
    int[] tp_angles = { 260, 300 };

    double[] filtered_val = { 0, 0 };

    // number of times the value has to be repeated to be registered as precise
    int dist_precision = 5;

    // tolerances for the distance gives
    int tp_dist_tolerance = 20;
    int tp_angle_tolerance = 5;

    public string port = "COM8";

    private LidarData[] data;

    private void Awake()
    {
        data = new LidarData[720];
    }

    // Use this for initialization
    void Start()
    {
        RplidarBinding.OnConnect(port);
        RplidarBinding.StartMotor();
        RplidarBinding.StartScan();
        getAllData();

    }

    // Update is called once per frame
    void Update()
    {
        getAllData();
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
            getAllData();
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
        int count = RplidarBinding.GetData(ref data);

        // Debug.Log("GrabData:" + count);
        // print("count " + count);
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if ((data[i].theta > tp_angles[0] - tp_angle_tolerance) && (data[i].theta < tp_angles[0] + tp_angle_tolerance))
                {
                    angles_list_0.Add((int)data[i].distant);

                    if (angles_list_0.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_0);
                        filtered_val[0] = (0.8 * filtered_val[0]) + (0.2 * avg_val);


                        if ((filtered_val[0] > tp_distances[0] - tp_dist_tolerance) && (filtered_val[0] < tp_distances[0] + tp_dist_tolerance))
                        {

                            print("filtered_val " + filtered_val[0]);
                            // Debug.Log("d:" + data[i].distant);
                            print("Click at 1");
                        }
                        angles_list_0.Clear();
                    }

                    // average(da)
                    // print(angles_list_0.Count);
                }
            }
        }
    }

    float average(List<int> angle_lists)
    {
        var sum = 0;

        for (var i = 0; i < angle_lists.Count; i++)
        {
            sum += angle_lists[i];
        }
        return sum / angle_lists.Count;
    }
}
