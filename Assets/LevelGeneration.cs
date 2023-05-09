using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{

    public float empty_prob = 0.1f;
    public float spikes_prob = 0.2f;
    public float slope_prob = 0.1f;

    public GameObject normalPrefab, spikesPrefab, slopePrefab, changeDirectionPrefab;

    public void Start()
    {
        GameObject obj;
        float xPos, zPos, yPos;
        xPos = zPos = yPos = 0.0f;

        obj = (GameObject)Instantiate(changeDirectionPrefab);
        obj.transform.position = new Vector3(xPos, yPos, zPos);
        obj.transform.parent = transform;

        for (uint pathNum = 0; pathNum < 30; ++pathNum)
        {
            float dx = (float)(pathNum%2);
            float dz = (float)(1 - pathNum%2);
            float dy = 0.0f;
            
            for (uint blockNum = 0; blockNum < Random.Range(4, 8); ++blockNum) // between 4 and 7 blocks
            {
                xPos += dx;
                zPos += dz;
                yPos += dy;

                float value = UnityEngine.Random.value;
                if (value <= empty_prob) 
                {
                    dy = 0.0f;
                    continue;
                }
                else if (value <= empty_prob + spikes_prob)
                {
                    dy = 0.0f;
                    obj = (GameObject)Instantiate(spikesPrefab);
                }
                else if (value <= empty_prob + spikes_prob + slope_prob)
                {
                    dy = -1.0f;
                    obj = (GameObject)Instantiate(slopePrefab);
                }
                else 
                {
                    dy = 0.0f;
                    obj = (GameObject)Instantiate(normalPrefab);
                }
                
                obj.transform.position = new Vector3(xPos, yPos, zPos);
                obj.transform.parent = transform;
            }

            xPos += dx;
            zPos += dz;
            yPos += dy;
            obj = (GameObject)Instantiate(changeDirectionPrefab);
            obj.transform.position = new Vector3(xPos, yPos, zPos);
            obj.transform.parent = transform;
        }
    }

    // Update is called once per frame
    public void Update()
    {
    }
}