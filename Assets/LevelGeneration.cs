using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    public GameObject gameManagerObject;
    private GameManager _gameManager;

    #region Probabilities

    public float emptyProb = 0.1f;
    public float spikesProb = 0.2f;
    public float slopeProb = 0.1f;
    public float slowdownProb = 0.1f;
    public float fireLauncherProb = 0.05f;
    public float volcanoProb = 0.05f;

    public float coinProb = 0.1f;

    #endregion

    #region Prefabs

    public GameObject normalPrefab;
    public GameObject spikesPrefab;
    public GameObject slopePrefab;
    public GameObject changeDirectionPrefab;
    public GameObject slowdownPrefab;
    public GameObject fireLauncherPrefab;
    public GameObject fireballPrefab;
    public GameObject volcanoPrefab;

    public GameObject coinPrefab;

    #endregion

    private int _platformsSinceCoin = 0;

    private PlatformType _previousPlatform;
    private int _numberSamePlatformPrevious;

    public static bool InfiniteGeneration = true;
    public int minimumPathsInstantiated = 5;
    private readonly Queue<Path> _placedPaths = new Queue<Path>();

    private float xPos, yPos, zPos;
    private int _numberPaths;

    public void Start()
    {
        _gameManager = gameManagerObject.GetComponent<GameManager>();
        if (InfiniteGeneration)
            _gameManager.ScoreChanged += OnScoreChanged;

        GameObject obj;
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
        _numberSamePlatformPrevious = 0;

        var numberPaths = InfiniteGeneration ? minimumPathsInstantiated : 30;
        for (var pathNum = 0; pathNum < numberPaths; ++pathNum)
        {
            var path = GeneratePath(pathNum);

            if (InfiniteGeneration)
                _placedPaths.Enqueue(path);
        }

        _numberPaths += numberPaths;
    }

    private Path GeneratePath(int pathNum)
    {
        var obj = new GameObject();

        float dx = pathNum % 2;
        float dz = 1 - pathNum % 2;
        float dy = 0.0f;

        bool stairsInPath = false;

        var path = new Path() { platforms = new List<GameObject>() };

        var numberBlocks = Random.Range(1, 6); // between 1 and 5 blocks
        for (int blockNum = 0; blockNum < numberBlocks; ++blockNum)
        {
            xPos += dx;
            zPos += dz;
            yPos += dy;

            bool canPlaceCoin = pathNum != 0;

            bool canPlaceEmpty =
                pathNum != 0
                && _previousPlatform != PlatformType.DirectionChange
                && _previousPlatform != PlatformType.Volcano
                && (_previousPlatform is PlatformType.Normal ||
                    (_previousPlatform != PlatformType.Empty && _numberSamePlatformPrevious < 2));

            bool canPlaceSpikes =
                pathNum != 0
                && (_previousPlatform is PlatformType.Normal or PlatformType.Slope ||
                    (_previousPlatform == PlatformType.Spikes && _numberSamePlatformPrevious < 2));

            bool canPlaceSlope =
                pathNum != 0
                && (_previousPlatform == PlatformType.Normal || _previousPlatform == PlatformType.Volcano ||
                    (_previousPlatform == PlatformType.Slope && _numberSamePlatformPrevious < 3));

            bool canPlaceSlowdown =
                pathNum != 0
                && (_previousPlatform is PlatformType.Normal or PlatformType.Slope ||
                    (_previousPlatform == PlatformType.Slowdown && _numberSamePlatformPrevious < 2));

            bool canPlaceVolcano =
                pathNum != 0
                && (_previousPlatform is PlatformType.Normal);

            PlatformType placedPlatform;

            float value = Random.value;
            if (value <= emptyProb && canPlaceEmpty)
            {
                // Empty
                dy = 0.0f;
                placedPlatform = PlatformType.Empty;
            }
            else if (value <= emptyProb + spikesProb && canPlaceSpikes)
            {
                // Spikes
                dy = 0.0f;
                obj = Instantiate(spikesPrefab);
                obj.transform.position = new Vector3(xPos, yPos, zPos);

                placedPlatform = PlatformType.Spikes;
            }
            else if (value <= emptyProb + spikesProb + slopeProb && canPlaceSlope)
            {
                // Stairs
                dy = -0.5f;
                obj = Instantiate(slopePrefab);
                obj.transform.position = new Vector3(xPos, yPos, zPos);
                obj.transform.Rotate(new Vector3(0.0f, 90.0f * dx, 0.0f));

                stairsInPath = true;

                placedPlatform = PlatformType.Slope;
            }
            else if (value <= emptyProb + spikesProb + slopeProb + slowdownProb && canPlaceSlowdown)
            {
                // Slowdown
                dy = 0.0f;
                obj = Instantiate(slowdownPrefab);
                obj.transform.position = new Vector3(xPos, yPos, zPos);

                placedPlatform = PlatformType.Slowdown;
            }
            else if (value <= emptyProb + spikesProb + slopeProb + slowdownProb + volcanoProb && canPlaceVolcano)
            {
                // Volcano
                dy = 0.0f;
                obj = Instantiate(volcanoPrefab);
                obj.transform.position = new Vector3(xPos, yPos, zPos);

                var controller = obj.GetComponent<VolcanoController>();
                controller.fireballPrefab = fireballPrefab;

                placedPlatform = PlatformType.Volcano;
            }
            else
            {
                // Normal
                dy = 0.0f;
                obj = Instantiate(normalPrefab);
                obj.transform.position = new Vector3(xPos, yPos, zPos);

                placedPlatform = PlatformType.Normal;
            }

            obj.transform.parent = transform;

            if (placedPlatform == _previousPlatform)
                _numberSamePlatformPrevious++;
            else
                _numberSamePlatformPrevious = 0;

            _previousPlatform = placedPlatform;

            if (InfiniteGeneration && placedPlatform != PlatformType.Empty)
                path.platforms.Add(obj);

            // Coin placement
            var probabilityCoin = coinProb * Mathf.Log(_platformsSinceCoin - 5);
            if (Random.value <= Mathf.Max(0.0f, probabilityCoin) && canPlaceCoin)
            {
                obj = Instantiate(coinPrefab);
                obj.transform.position = new Vector3(xPos, yPos + 1.5f, zPos);

                var coinController = obj.GetComponent<CoinController>();
                coinController.GameManager = _gameManager;

                _platformsSinceCoin = 0;
            }
            else
            {
                _platformsSinceCoin++;
            }

            path.platforms.Add(obj);
        }

        xPos += dx;
        zPos += dz;
        yPos += dy;

        obj = Instantiate(changeDirectionPrefab);
        obj.transform.position = new Vector3(xPos, yPos, zPos);
        obj.transform.parent = transform;

        for (int i = 1; i < 5; ++i)
        {
            obj = Instantiate(normalPrefab);
            obj.transform.position = new Vector3(xPos, yPos - i, zPos);
            obj.transform.parent = transform;
        }

        path.platforms.Add(obj);

        // Fire launcher placement
        bool canPlaceFireLauncher = pathNum % 2 != 0 && !stairsInPath && numberBlocks >= 2;
        if (Random.value <= fireLauncherProb && canPlaceFireLauncher)
        {
            obj = Instantiate(fireLauncherPrefab);
            obj.transform.position = new Vector3(xPos + 0.75f, yPos + 1.0f, zPos);
            obj.transform.parent = transform;

            var controller = obj.GetComponent<FireLauncherController>();
            controller.fireballPrefab = fireballPrefab;

            path.platforms.Add(obj);
        }

        _previousPlatform = PlatformType.DirectionChange;

        return path;
    }

    private void OnScoreChanged(object _, int score)
    {
        if (score >= minimumPathsInstantiated)
        {
            // Destroy path
            var p = _placedPaths.Dequeue();
            foreach (var platform in p.platforms)
                Destroy(platform);
        }

        var path = GeneratePath(_numberPaths++);
        _placedPaths.Enqueue(path);
        
        Debug.Log($"Number placed paths = {_placedPaths.Count}");
    }

    private struct Path
    {
        public List<GameObject> platforms;
    }

    private enum PlatformType
    {
        Empty,
        Spikes,
        Slope,
        Slowdown,
        Volcano,
        DirectionChange,
        Normal
    }
}