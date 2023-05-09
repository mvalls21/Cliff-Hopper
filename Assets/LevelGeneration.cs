using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    public float emptyProb = 0.1f;
    public float spikesProb = 0.2f;
    public float slopeProb = 0.1f;

    public GameObject normalPrefab, spikesPrefab, slopePrefab, changeDirectionPrefab;

    public void Start()
    {
        GameObject obj;
        float xPos, zPos, yPos;
        xPos = zPos = yPos = 0.0f;

        obj = Instantiate(changeDirectionPrefab);
        obj.transform.position = new Vector3(xPos, yPos, zPos);
        obj.transform.parent = transform;

        for (uint pathNum = 0; pathNum < 30; ++pathNum)
        {
            float dx = pathNum % 2;
            float dz = 1 - pathNum % 2;
            float dy = 0.0f;

            for (uint blockNum = 0; blockNum < Random.Range(4, 8); ++blockNum) // between 4 and 7 blocks
            {
                xPos += dx;
                zPos += dz;
                yPos += dy;

                float value = Random.value;
                if (value <= emptyProb)
                {
                    dy = 0.0f;
                    continue;
                }
                else if (value <= emptyProb + spikesProb && value >= emptyProb)
                {
                    dy = 0.0f;
                    obj = Instantiate(spikesPrefab);
                    obj.transform.position = new Vector3(xPos, yPos, zPos);
                }
                else if (value <= emptyProb + spikesProb + slopeProb && value >= emptyProb + spikesProb)
                {
                    dy = -0.5f;
                    obj = Instantiate(slopePrefab);
                    obj.transform.position = new Vector3(xPos, yPos, zPos);
                    obj.transform.Rotate(new Vector3(0.0f, 90.0f * dx, 0.0f));
                }
                else
                {
                    dy = 0.0f;
                    obj = Instantiate(normalPrefab);
                    obj.transform.position = new Vector3(xPos, yPos, zPos);
                }

                obj.transform.parent = transform;
            }

            xPos += dx;
            zPos += dz;
            yPos += dy;

            obj = Instantiate(changeDirectionPrefab);
            obj.transform.position = new Vector3(xPos, yPos, zPos);
            obj.transform.parent = transform;
        }
    }

    // Update is called once per frame
    public void Update()
    {
    }
}