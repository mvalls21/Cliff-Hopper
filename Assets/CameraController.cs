using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float correctionStrength = 0.5f;

    private Player _playerScript;

    private readonly Vector3 _baseTranslation = new Vector3(6.0f, 9.0f, 11.0f);

    private readonly Vector3 _movementDirection = new Vector3(1.0f, 0.0f, 1.0f);

    private Vector3 _previousPlayerMovementDirection;

    private Vector3 _middlePositionCache;

    public void Start()
    {
        _previousPlayerMovementDirection = Vector3.zero;
        _playerScript = player.GetComponent<Player>();
    }

    public void Update()
    {
        var middle = FindMiddlePlatformRaycast();
        // var middle = FindMiddlePlatform();

        var position = transform.position;
        var correction = new Vector3(middle.x - position.x, middle.y - position.y,
            middle.z - position.z);

        correction += _baseTranslation;

        var correctionDirection = correction.normalized;

        var playerSpeed = _playerScript.speed / 2.0f;
        var movement = _movementDirection * playerSpeed + correctionDirection * correctionStrength;

        var currentRotation = transform.rotation;
        transform.rotation = Quaternion.identity;

        transform.Translate(movement * Time.deltaTime);
        transform.rotation = currentRotation;
    }

    private Vector3 FindMiddlePlatformRaycast()
    {
        var playerDirection = _playerScript.movementDirection;

        if (playerDirection == _previousPlayerMovementDirection)
            return _middlePositionCache;

        _previousPlayerMovementDirection = playerDirection;

        var origin = player.transform.position + new Vector3(0.0f, 0.0f, 0.0f);
        var currentPosition = new Vector3(origin.x, origin.y, origin.z);

        while (Physics.Raycast(currentPosition + playerDirection, Vector3.down, out RaycastHit info))
        {
            currentPosition = info.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        }

        var middle = (origin + currentPosition) / 2.0f;

        _middlePositionCache = middle;
        return middle;
    }
}