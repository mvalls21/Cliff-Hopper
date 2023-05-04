using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _movementDirection;

    private GameObject _previousDirectionChangeObject;

    public void Start()
    {
        _movementDirection = new Vector3(0.0f, 0.0f, 1.0f);
    }

    public void Update()
    {
        var newPosition = transform.position + _movementDirection * Time.deltaTime;
        var centerPosition = new Vector3(Mathf.Round(newPosition.x), newPosition.y, Mathf.Round(newPosition.z));

        // Only apply correction to the axis where the player is not moving (to prevent stutters while moving)
        var movement = Vector3.Scale((centerPosition - transform.position),
            (Vector3.one - _movementDirection)) + _movementDirection;

        transform.Translate(movement * Time.deltaTime);

        var hit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo);
        if (!hit)
        {
            // TODO: Player fell of the map = dead
            Destroy(this.GameObject());
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (!hit) throw new InvalidOperationException();

            var objectTag = hitInfo.collider.GameObject().tag;
            if (objectTag == "DirectionChange" && _previousDirectionChangeObject != hitInfo.collider.GameObject())
            {
                _previousDirectionChangeObject = hitInfo.collider.GameObject();
                ChangeDirection();
            }
            else
            {
                // TODO: Jump
            }
        }
    }

    private void ChangeDirection()
    {
        _movementDirection = new Vector3(1.0f, 0.0f, 1.0f) - _movementDirection;
    }
}