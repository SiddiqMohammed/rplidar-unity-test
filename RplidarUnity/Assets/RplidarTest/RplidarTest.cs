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
    int[] tp_distances = { 1500, 1760, 1600, 700, 920, 1410, 1700, 1710, 1550, 4000 };
    int[] tp_angles = { 60, 42, 28, 27, 9, 345, 329, 320, 309, 292 };
    // int[] tp_distances = { 300, 600, 1200, 900, 1210, 1410, 1520, 1710, 1450, 1160 };
    // int[] tp_angles = { 54, 40, 24, 27, 9, 352, 334, 324, 315, 300 };

    double[] filtered_val = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    int[] filter_counter = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // redundant true values to avoid spikes being registered as true
    int rdc_val = 2;

    // number of times the value has to be repeated to be registered as precise
    int dist_precision = 2;

    // tolerances for the distance and angle
    int tp_dist_tolerance = 300;
    int tp_angle_tolerance = 6;

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

        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {

                if ((data[i].theta > tp_angles[0] - 1) && (data[i].theta < tp_angles[0] + 1))
                {
                    angles_list_0.Add((int)data[i].distant);
                    // print("data 0: " + data[i].distant);


                    if (angles_list_0.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_0);
                        filtered_val[0] = (0.8 * filtered_val[0]) + (0.2 * avg_val);

                        // print(filtered_val[0]);
                        if ((filtered_val[0] > tp_distances[0] - 100) && (filtered_val[0] < tp_distances[0] + 100))
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

                if ((data[i].theta > tp_angles[1] - 4) && (data[i].theta < tp_angles[1] + 4))
                {
                    angles_list_1.Add((int)data[i].distant);
                    // print("data 1: " + data[i].distant);

                    if (angles_list_1.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_1);
                        filtered_val[1] = (0.8 * filtered_val[1]) + (0.2 * avg_val);
                        // print(filtered_val[1]);

                        if ((filtered_val[1] > tp_distances[1] - 200) && (filtered_val[1] < tp_distances[1] + 200))
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

                            if (filter_counter[2] > rdc_val + 2)
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
                if ((data[i].theta > tp_angles[3] - 2) && (data[i].theta < tp_angles[3] + 2))
                {
                    angles_list_3.Add((int)data[i].distant);
                    // print("data 2: " + data[i].distant);

                    if (angles_list_3.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_3);
                        filtered_val[3] = (0.8 * filtered_val[3]) + (0.2 * avg_val);

                        // print(filtered_val[3]);
                        if ((filtered_val[3] > tp_distances[3] - tp_dist_tolerance) && (filtered_val[3] < tp_distances[3] + tp_dist_tolerance))
                        // if ((filtered_val[3] > 700) && (filtered_val[3] < 900))
                        {
                            filter_counter[3]++;

                            if (filter_counter[3] > rdc_val)
                            {
                                button_clicked = 3;
                                print("3 at " + filtered_val[3]);
                            }
                        }
                        else
                        {
                            filter_counter[3] = 0;
                        }
                        angles_list_3.Clear();
                    }
                }
                if ((data[i].theta > tp_angles[4] - tp_angle_tolerance) && (data[i].theta < tp_angles[4] + tp_angle_tolerance))
                {
                    angles_list_4.Add((int)data[i].distant);
                    // print("data 2: " + data[i].distant);

                    if (angles_list_4.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_4);
                        filtered_val[4] = (0.8 * filtered_val[4]) + (0.2 * avg_val);

                        // print(filtered_val[4]);
                        if ((filtered_val[4] > tp_distances[4] - tp_dist_tolerance) && (filtered_val[4] < tp_distances[4] + tp_dist_tolerance))
                        {
                            filter_counter[4]++;

                            if (filter_counter[4] > rdc_val)
                            {
                                button_clicked = 4;
                                print("4 at " + filtered_val[4]);
                            }
                        }
                        else
                        {
                            filter_counter[4] = 0;
                        }
                        angles_list_4.Clear();
                    }
                }
                if ((data[i].theta > tp_angles[5] - tp_angle_tolerance) && (data[i].theta < tp_angles[5] + tp_angle_tolerance))
                {
                    angles_list_5.Add((int)data[i].distant);
                    // print("data 2: " + data[i].distant);

                    if (angles_list_5.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_5);
                        filtered_val[5] = (0.8 * filtered_val[5]) + (0.2 * avg_val);

                        // print(filtered_val[5]);
                        if ((filtered_val[5] > 1600) && (filtered_val[5] < 1750))
                        {
                            filter_counter[5]++;

                            // if (filter_counter[5] > rdc_val)
                            // {
                            button_clicked = 5;
                            print("5 at " + filtered_val[5]);
                            // }
                        }
                        else
                        {
                            filter_counter[5] = 0;
                        }
                        angles_list_5.Clear();
                    }
                }
                if ((data[i].theta > tp_angles[6] - tp_angle_tolerance) && (data[i].theta < tp_angles[6] + tp_angle_tolerance))
                {
                    angles_list_6.Add((int)data[i].distant);
                    // print("data 6: " + data[i].distant);

                    if (angles_list_6.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_6);
                        filtered_val[6] = (0.8 * filtered_val[6]) + (0.2 * avg_val);

                        // print(filtered_val[6]);
                        if ((filtered_val[6] > tp_distances[6] - tp_dist_tolerance) && (filtered_val[6] < tp_distances[6] + tp_dist_tolerance))
                        {
                            filter_counter[6]++;

                            if (filter_counter[6] > rdc_val)
                            {
                                button_clicked = 6;
                                print("6 at " + filtered_val[6]);
                            }
                        }
                        else
                        {
                            filter_counter[6] = 0;
                        }
                        angles_list_6.Clear();
                    }
                }
                if ((data[i].theta > tp_angles[7] - tp_angle_tolerance) && (data[i].theta < tp_angles[7] + tp_angle_tolerance))
                {
                    angles_list_7.Add((int)data[i].distant);
                    // print("data 7: " + data[i].distant);

                    if (angles_list_7.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_7);
                        filtered_val[7] = (0.8 * filtered_val[7]) + (0.2 * avg_val);

                        // print(filtered_val[7]);
                        if ((filtered_val[7] > tp_distances[7] - tp_dist_tolerance) && (filtered_val[7] < tp_distances[7] + tp_dist_tolerance))
                        {
                            filter_counter[7]++;

                            // if (filter_counter[7] > rdc_val)
                            // {
                            button_clicked = 7;
                            print("7 at " + filtered_val[7]);
                            // }
                        }
                        else
                        {
                            filter_counter[7] = 0;
                        }
                        angles_list_7.Clear();
                    }
                }
                if ((data[i].theta > tp_angles[8] - tp_angle_tolerance) && (data[i].theta < tp_angles[8] + tp_angle_tolerance))
                {
                    angles_list_8.Add((int)data[i].distant);
                    // print("data 2: " + data[i].distant);

                    if (angles_list_8.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_8);
                        filtered_val[8] = (0.8 * filtered_val[8]) + (0.2 * avg_val);

                        // print(filtered_val[8]);
                        if ((filtered_val[8] > tp_distances[8] - tp_dist_tolerance) && (filtered_val[8] < tp_distances[8] + tp_dist_tolerance))
                        {
                            filter_counter[8]++;

                            // if (filter_counter[8] > rdc_val)
                            // {
                            button_clicked = 8;
                            print("8 at " + filtered_val[8]);
                            // }
                        }
                        else
                        {
                            filter_counter[8] = 0;
                        }
                        angles_list_8.Clear();
                    }
                }
                if ((data[i].theta > tp_angles[9] - tp_angle_tolerance) && (data[i].theta < tp_angles[9] + tp_angle_tolerance))
                {
                    angles_list_9.Add((int)data[i].distant);
                    // print("data 9: " + data[i].distant);

                    if (angles_list_9.Count > dist_precision)
                    {
                        float avg_val = average(angles_list_9);
                        filtered_val[9] = (0.9 * filtered_val[9]) + (0.2 * avg_val);

                        // print(filtered_val[9]);
                        if ((filtered_val[9] > tp_distances[9] - tp_dist_tolerance) && (filtered_val[9] < tp_distances[9] + tp_dist_tolerance))
                        // if ((filtered_val[9] > 4000) && (filtered_val[9] < 5000))
                        {
                            // filter_counter[9]++;

                            // if (filter_counter[9] > rdc_val)
                            // {
                            button_clicked = 9;
                            print("9 at " + filtered_val[9]);
                            // }
                        }
                        else
                        {
                            filter_counter[9] = 0;
                        }
                        angles_list_9.Clear();
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
