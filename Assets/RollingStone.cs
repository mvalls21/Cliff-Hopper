using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RollingStone : MonoBehaviour
{
    private Vector3 _movementDirection;

    public GameObject playerGameObject;

    private Player _playerScript;

    public float speed = 2.0f;

    private GameObject _previousDirectionChange;

    private Rigidbody _rigidbody;

    public void Start()
    {
        _movementDirection = new Vector3(0.0f, 0.0f, 1.0f);
        _playerScript = playerGameObject.GetComponent<Player>();
        speed = _playerScript.speed;

        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (GameManager.Instance.IsGamePaused)
            return;

        _rigidbody.useGravity = true;
        transform.Translate(_movementDirection * speed * Time.deltaTime);

        var hit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit info);
        if (hit && info.collider.CompareTag("DirectionChange") && info.collider.GameObject() != _previousDirectionChange
            && IsAtCenter(info.collider.GameObject()))
        {
            ChangeDirection();
            _previousDirectionChange = info.collider.GameObject();
        }
        else if (!hit || info.collider.name == "stairs(Clone)")
        {
            _rigidbody.useGravity = false;
        }
    }

    private bool IsAtCenter(GameObject obj)
    {
        var position = transform.position - Vector3.up;
        var platformPosition = obj.transform.position;

        const float epsilon = 0.05f;
        return position.x - platformPosition.x >= epsilon || position.z - platformPosition.z >= epsilon;
    }

    private void ChangeDirection()
    {
        _movementDirection = new Vector3(1.0f, 0.0f, 1.0f) - _movementDirection;
        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y,
            Mathf.Round(transform.position.z));
    }
}