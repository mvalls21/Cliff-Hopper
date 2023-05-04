using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;

    private readonly Vector3 _baseTranslation = new Vector3(10.0f, 9.0f, 7.0f);

    private readonly Vector3 _movementDirection = new Vector3(1.0f, 0.0f, 1.0f);

    public void Start()
    {
    }

    public void Update()
    {
        var movement = _movementDirection;

        var middle = FindMiddlePlatform();
        if (middle != null)
        {
            var position = transform.position;
            var correction = new Vector3(middle.Value.x - position.x, 0.0f,
                middle.Value.z - position.z);

            correction.x += _baseTranslation.x;
            correction.z += _baseTranslation.z;

            const float strengthCorrection = 0.15f;
            movement = _movementDirection * (1.0f - strengthCorrection) + correction * strengthCorrection;
        }
        
        var currentRotation = transform.rotation;
        transform.rotation = Quaternion.identity;

        transform.Translate(movement * Time.deltaTime);
        transform.rotation = currentRotation;
    }

    private Vector3? FindMiddlePlatform()
    {
        var hit = Physics.Raycast(Player.transform.position, Vector3.down, out RaycastHit hitInfo);
        if (!hit) return null;

        var parent = hitInfo.collider.transform.parent;
        if (parent == null) return null;

        var min = parent.transform.GetChild(0).position;
        var max = parent.transform.GetChild(0).position;

        for (int i = 1; i < parent.transform.childCount; ++i)
        {
            var child = parent.transform.GetChild(i);
            if (child.position.x < min.x || child.position.z < min.z)
                min = child.position;

            if (child.position.x > max.x || child.position.z > max.z)
                max = child.position;
        }

        var middle = new Vector3((min.x + max.x) / 2.0f, 0.0f, (min.z + max.z) / 2.0f);
        return middle;
    }
}