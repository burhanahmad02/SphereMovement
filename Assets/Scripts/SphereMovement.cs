using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SphereMovement : MonoBehaviour
{
    public float time;
    public float speed;
    public float distance;
    public int sphereId;

    private Vector3 centerPoint;
    private float currentAngle;
    private float currentDistance;
    private float direction;
    private float startTime;
    private bool movingTowardsCenter = true;
    private string logFilePath;

    public List<SphereData> sphereDataList;

    void Start()
    {
        //getting the center point
        centerPoint = transform.parent.position;

        //loading the data from json file here which i added in a folder named Json
        string jsonFilePath = Application.dataPath + "/Json/sphere_properties.json";
        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            sphereDataList = JsonConvert.DeserializeObject<List<SphereData>>(jsonData);
            //List<SphereData> sphereDataList = JsonConvert.DeserializeObject<List<SphereData>>(jsonData);
            foreach (SphereData sphereData in sphereDataList)
            {
                if (sphereData.sphereId == sphereId)
                {
                    time = sphereData.time;
                    speed = sphereData.speed;
                    distance = sphereData.distance;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Json file not found at path: " + jsonFilePath);
        }


        //I set the angle here to make a circlular shape for spheres
        currentAngle = sphereId * 45f;
        direction = (movingTowardsCenter) ? 1f : -1f;

        //initializing the log file here in a text file
        startTime = Time.time;
        logFilePath = Application.dataPath + "/sphere_movement_log.txt";
    }



    void Update()
    {
        // Calculating current distance from center point
        currentDistance = distance * Mathf.Sin(Time.time - startTime) * speed;

        // Calculate current position based on angle, direction, and distance
        Vector3 currentPosition = centerPoint + new Vector3(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * currentDistance,
            transform.position.y,
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * currentDistance);

        //As I mentioned in the interview I was experimenting lerp and slerp so I used Lerp here instead of movetowards
        // Move towards or away from center point
        if (movingTowardsCenter)
        {
            transform.position = Vector3.Lerp(transform.position, currentPosition, Time.deltaTime / time);
        }
        else
        {
            transform.position = Vector3.Lerp(currentPosition, centerPoint, Time.deltaTime / time);
        }

        // Change direction when sphere reaches maximum distance
        if (currentDistance >= distance || currentDistance <= 0f)
        {
            direction = -direction;
            movingTowardsCenter = !movingTowardsCenter;
        }

        // Update current angle based on direction and speed
        currentAngle += direction * speed * Time.deltaTime;

        // Log current position to file
        LogPosition();
    }

    void LogPosition()
    {
        if (logFilePath != null)
        {
            // Create or append to log file
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine("Sphere " + sphereId + " position: " + transform.position.ToString("F3"));
            }
        }
    }

}

[System.Serializable]
public class SphereData
{
    public int sphereId;
    public float time;
    public float speed;
    public float distance;
}
