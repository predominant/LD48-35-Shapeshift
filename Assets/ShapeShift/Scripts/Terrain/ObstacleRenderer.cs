using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleRenderer : MonoBehaviour
{
    public List<GameObject> ObstacleList;
    public float MinDistance = 30f;

    public void Start()
    {
        int obsCount = Random.Range(10, 20);
        float spread = 190f / (float)obsCount;
        // Generate?
        for (int i = 0; i < obsCount; i++)
        {
            // 190 is the default map width
            float posX = Random.Range(spread / 2.5f, spread);

        }
    }
}
