using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    public GameObject GameManagerObject;
    private GameManager _gameManager;
    
    public float emptyProb = 0.1f;
    public float spikesProb = 0.2f;
    public float slopeProb = 0.1f;
    public float coinProb = 0.1f;

    public GameObject normalPrefab;
    public GameObject spikesPrefab;
    public GameObject slopePrefab;
    public GameObject changeDirectionPrefab;
    public GameObject coinPrefab;

    private int _platformsSinceCoin = 0;
    private PlatformType _previousPlatform;

    public void Start()
    {
        _gameManager = GameManagerObject.GetComponent<GameManager>();
        
        GameObject obj;
        float xPos, zPos, yPos;
        xPos = zPos = yPos = 0.0f;

        // Temporal: Runoff area for the rock
        Instantiate(normalPrefab, new Vector3(0.0f, 0.0f, -5.0f), Quaternion.identity);
        Instantiate(normalPrefab, new Vector3(0.0f, 0.0f, -4.0f), Quaternion.identity);
        Instantiate(normalPrefab, new Vector3(0.0f, 0.0f, -3.0f), Quaternion.identity);
        Instantiate(normalPrefab, new Vector3(0.0f, 0.0f, -2.0f), Quaternion.identity);
        Instantiate(normalPrefab, new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity);

        obj = Instantiate(normalPrefab);
        obj.transform.position = new Vector3(xPos, yPos, zPos);
        obj.transform.parent = transform;

        _previousPlatform = PlatformType.Normal;

        for (uint pathNum = 0; pathNum < 30; ++pathNum)
        {
            float dx = pathNum % 2;
            float dz = 1 - pathNum % 2;
            float dy = 0.0f;

            for (uint blockNum = 0; blockNum < Random.Range(1, 8); ++blockNum) // between 1 and 7 blocks
            {
                xPos += dx;
                zPos += dz;
                yPos += dy;

                bool canPlaceCoin = true;
                // TODO: Modify so that two empties in a row are valid
                bool canPlaceEmpty = pathNum != 0 && (_previousPlatform is PlatformType.Normal or PlatformType.Slope);
                bool canPlaceSpikes = pathNum != 0 && (_previousPlatform is PlatformType.Normal or PlatformType.Slope);

                float value = Random.value;
                if (value <= emptyProb && canPlaceEmpty)
                {
                    dy = 0.0f;
                    _previousPlatform = PlatformType.Empty;
                    continue;
                }
                else if (value <= emptyProb + spikesProb && value >= emptyProb && canPlaceSpikes)
                {
                    dy = 0.0f;
                    obj = Instantiate(spikesPrefab);
                    obj.transform.position = new Vector3(xPos, yPos, zPos);

                    _previousPlatform = PlatformType.Spikes;
                    canPlaceCoin = false;
                }
                else if (value <= emptyProb + spikesProb + slopeProb && value >= emptyProb + spikesProb && pathNum != 0)
                {
                    dy = -0.5f;
                    obj = Instantiate(slopePrefab);
                    obj.transform.position = new Vector3(xPos, yPos, zPos);
                    obj.transform.Rotate(new Vector3(0.0f, 90.0f * dx, 0.0f));

                    _previousPlatform = PlatformType.Empty;
                }
                else
                {
                    dy = 0.0f;
                    obj = Instantiate(normalPrefab);
                    obj.transform.position = new Vector3(xPos, yPos, zPos);

                    _previousPlatform = PlatformType.Normal;
                }

                obj.transform.parent = transform;

                // Coin placement
                var probabilityCoin = coinProb * Mathf.Log(_platformsSinceCoin - 5);
                if (Random.value < Mathf.Max(0.0f, probabilityCoin) && canPlaceCoin)
                {
                    obj = Instantiate(coinPrefab);
                    obj.transform.position = new Vector3(xPos, yPos + 1, zPos);

                    var coinController = obj.GetComponent<CoinController>();
                    coinController.GameManager = _gameManager;

                    _platformsSinceCoin = 0;
                }
                else
                {
                    _platformsSinceCoin++;
                }
            }

            xPos += dx;
            zPos += dz;
            yPos += dy;

            obj = Instantiate(changeDirectionPrefab);
            obj.transform.position = new Vector3(xPos, yPos, zPos);
            obj.transform.parent = transform;

            _previousPlatform = PlatformType.DirectionChange;
        }
    }

    private enum PlatformType
    {
        Empty,
        Spikes,
        Slope,
        DirectionChange,
        Normal
    }
}