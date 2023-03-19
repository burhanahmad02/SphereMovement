using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCreator : MonoBehaviour
{
    public GameObject spherePrefab;

    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            Vector3 position = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * 5f,
                0f,
                Mathf.Sin(angle * Mathf.Deg2Rad) * 5f);
            GameObject sphere = Instantiate(spherePrefab, position, Quaternion.identity, transform);
            sphere.GetComponent<SphereMovement>().sphereId = i;
        }
    }
}
