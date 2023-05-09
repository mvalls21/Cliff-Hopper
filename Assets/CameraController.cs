using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;

    private readonly Vector3 _baseTranslation = new Vector3(7.0f, 9.0f, 11.0f);

    private readonly Vector3 _movementDirection = new Vector3(0.5f, 0.0f, 0.5f);

    private Vector3 _previousPlayerMovementDirection;

    private Vector3 _middlePositionCache;

    public void Start()
    {
        _previousPlayerMovementDirection = Vector3.zero;
    }

    public void Update()
    {
        var middle = FindMiddlePlatformRaycast();
        // var middle = FindMiddlePlatform();

        var position = transform.position;
        var correction = new Vector3(middle.x - position.x, 0.0f,
            middle.z - position.z);

        correction.x += _baseTranslation.x;
        correction.z += _baseTranslation.z;

        var correctionDirection = correction.normalized;

        // const float strengthCorrection = 0.05f;
        // var movement = _movementDirection * (1.0f - strengthCorrection) + correction * strengthCorrection;
        var movement = _movementDirection + correctionDirection * 0.1f;

        var currentRotation = transform.rotation;
        transform.rotation = Quaternion.identity;

        transform.Translate(movement * Time.deltaTime);
        transform.rotation = currentRotation;
    }

    private Vector3 FindMiddlePlatformRaycast()
    {
        var playerDirection = Player.GetComponent<Player>().movementDirection;

        if (playerDirection == _previousPlayerMovementDirection)
            return _middlePositionCache;
        
        _previousPlayerMovementDirection = playerDirection;
        
        var origin = Player.transform.position + new Vector3(0.0f, 0.0f, 0.0f);
        var currentPosition = new Vector3(origin.x, origin.y, origin.z);

        while (Physics.Raycast(currentPosition + playerDirection, Vector3.down, out RaycastHit info))
        {
            currentPosition = info.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        }

        var middle = (origin + currentPosition) / 2.0f;

        // FindMinMaxPlatformsRaycast(origin, direction, 100.0f, out Vector3 min, out Vector3 max);
        // var middle = new Vector3((min.x + max.x) / 2.0f, 0.0f, (min.z + max.z) / 2.0f);
        
        _middlePositionCache = middle;
        return middle;
    }
}