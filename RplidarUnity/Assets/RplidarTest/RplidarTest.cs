using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RplidarTest : MonoBehaviour
{

    List<int> angles_list_0 = new List<int>();
    List<int> angles_list_1 = new List<int>();
    List<int> angles_list_2 = new List<int>();
    List<int> angles_list_3 = new List<int>();
    List<int> angles_list_4 = new List<int>();
    List<int> angles_list_5 = new List<int>();
    List<int> angles_list_6 = new List<int>();
    List<int> angles_list_7 = new List<int>();
    List<int> angles_list_8 = new List<int>();
    List<int> angles_list_9 = new List<int>();


    // distances of each touchpoint (in mm)
    // Left to Right
    // int[] tp_distances = { 1720, 1560, 1200, 900, 1210, 1410, 1520, 1710, 1450, 1160 };
    // int[] tp_angles = { 54, 40, 27, 27, 9, 352, 334, 324, 315, 300 };
    int[] tp_distances = { 900, 500, 1200, 900, 1210, 1410, 1520, 1710, 1450, 1160 };
    int[] tp_angles = { 270, 270, 270, 250, 9, 352, 334, 324, 315, 300 };

    double[] filtered_val = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    int[] filter_counter = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // redundant true values to avoid spikes being registered as true
    int rdc_val = 2;

    // number of times the value has to be repeated to be registered as precise
    int dist_precision = 5;

    // tolerances for the distance and angle
    int tp_dist_tolerance = 80;
    int tp_angle_tolerance = 2;

    public string port = "COM8";

    private LidarData[] data;

    public int button_clicked = -1;

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
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("Begin");
            if (string.IsNullOrEmpty(port))
            {
                return;
            }

            RplidarBinding.OnConnect(port);
            RplidarBinding.StartMotor();
            RplidarBinding.StartScan();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            print("Disconnect");
            RplidarBinding.OnDisconnect();
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

                // for (int j = 0; j < count; j++)
                // {

                // }

                if ((data[i].theta > tp_angles[0] - tp_angle_tolerance) && (data[i].theta < tp_angles[0] + tp_angle_tolerance))
                {
                    angles_list_0.Add((int)data[i].distant);
                    // print("data 0: " + data[i].distant);


                    if (angles_list_0.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_0);
                        filtered_val[0] = (0.8 * filtered_val[0]) + (0.2 * avg_val);

                        if ((filtered_val[0] > tp_distances[0] - tp_dist_tolerance) && (filtered_val[0] < tp_distances[0] + tp_dist_tolerance))
                        {
                            filter_counter[0]++;

                            if (filter_counter[0] > rdc_val)
                            {
                                button_clicked = 0;
                                print("0 at " + filtered_val[0]);
                            }
                        }
                        else
                        {
                            filter_counter[0] = 0;
                        }
                        angles_list_0.Clear();
                    }
                }

                if ((data[i].theta > tp_angles[1] - tp_angle_tolerance) && (data[i].theta < tp_angles[1] + tp_angle_tolerance))
                {
                    angles_list_1.Add((int)data[i].distant);
                    // print("data 1: " + data[i].distant);

                    if (angles_list_1.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_1);
                        filtered_val[1] = (0.8 * filtered_val[1]) + (0.2 * avg_val);

                        if ((filtered_val[1] > tp_distances[1] - tp_dist_tolerance) && (filtered_val[1] < tp_distances[1] + tp_dist_tolerance))
                        {
                            filter_counter[1]++;

                            if (filter_counter[1] > rdc_val)
                            {
                                button_clicked = 1;
                                print("1 at " + filtered_val[1]);
                            }
                        }
                        else
                        {
                            filter_counter[1] = 0;
                        }
                        angles_list_1.Clear();
                    }
                }

                if ((data[i].theta > tp_angles[2] - tp_angle_tolerance) && (data[i].theta < tp_angles[2] + tp_angle_tolerance))
                {
                    angles_list_2.Add((int)data[i].distant);
                    // print("data 2: " + data[i].distant);

                    if (angles_list_2.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_2);
                        filtered_val[2] = (0.8 * filtered_val[2]) + (0.2 * avg_val);

                        // print(filtered_val[2]);
                        if ((filtered_val[2] > tp_distances[2] - tp_dist_tolerance) && (filtered_val[2] < tp_distances[2] + tp_dist_tolerance))
                        {
                            filter_counter[2]++;

                            if (filter_counter[2] > rdc_val)
                            {
                                button_clicked = 2;
                                print("2 at " + filtered_val[2]);
                            }
                        }
                        else
                        {
                            filter_counter[2] = 0;
                        }
                        angles_list_2.Clear();
                    }
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

// RplidarBinding.OnConnect(port);
// RplidarBinding.StartMotor();
// RplidarBinding.StartScan();
// RplidarBinding.OnDisconnect();
// RplidarBinding.StartScan();
// RplidarBinding.EndScan();
// RplidarBinding.StartMotor();
// RplidarBinding.EndMotor();
// RplidarBinding.ReleaseDrive();

