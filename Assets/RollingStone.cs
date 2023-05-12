using System;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RollingStone : MonoBehaviour
{
    private Vector3 _movementDirection;

    public float speed = 2.0f;

    private GameObject _previousDirectionChange;

    public void Start()
    {
        _movementDirection = new Vector3(0.0f, 0.0f, 1.0f);
    }

    public void Update()
    {
        transform.Translate(_movementDirection * speed * Time.deltaTime);

        var hit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit info);
        if (hit && info.collider.CompareTag("DirectionChange") && info.collider.GameObject() != _previousDirectionChange
            && IsAtCenter(info.collider.GameObject()))
        {
            ChangeDirection();
            _previousDirectionChange = info.collider.GameObject();
        }
    }

    private bool IsAtCenter(GameObject obj)
    {
        var position = transform.position - new Vector3(0.0f, 1.0f, 0.0f);
        var platformPosition = obj.transform.position;

        // Debug.Log($"{position} {platformPosition}");

        return Math.Abs(position.x - platformPosition.x) < 0.1 && Math.Abs(position.y - platformPosition.y) < 0.1
                                                                && Math.Abs(position.z - platformPosition.z) < 0.1;
    }

    private void ChangeDirection()
    {
        _movementDirection = new Vector3(1.0f, 0.0f, 1.0f) - _movementDirection;
        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y,
            Mathf.Round(transform.position.z));
    }
}