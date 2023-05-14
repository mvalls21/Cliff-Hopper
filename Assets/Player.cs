using System;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 movementDirection;

    public float speed = 2.0f;

    private GameObject _previousDirectionChangeObject;

    private int _jumpCounter = 0;

    private bool _spacePressed = false;

    public void Start()
    {
        movementDirection = new Vector3(0.0f, 0.0f, 1.0f);
    }

    public void Update()
    {
        var newPosition = transform.position + movementDirection * Time.deltaTime;
        var centerPosition = new Vector3(Mathf.Round(newPosition.x), newPosition.y, Mathf.Round(newPosition.z));

        // Only apply correction to the axis where the player is not moving (to prevent stutters while moving)
        var movement = Vector3.Scale((centerPosition - transform.position),
            (Vector3.one - movementDirection)) + movementDirection;

        transform.Translate(movement * speed * Time.deltaTime);

        var hit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo);
        if (!hit)
        {
            // TODO: Player fell of the map = dead
            // TODO: Actually, not true, maybe jumping over empty platform
            // Destroy(this.GameObject());
            return;
        }

        if (hitInfo.collider.GameObject().CompareTag("Spike") && _jumpCounter == 0)
        {
            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.AddForce(new Vector3(0.0f, 1.0f, 0.5f), ForceMode.Impulse);
            
            return;
        }

        if (Input.GetKey(KeyCode.Space) && !_spacePressed)
        {
            if (!hit) throw new InvalidOperationException();

            if (hitInfo.collider.GameObject().CompareTag("DirectionChange") &&
                _previousDirectionChangeObject != hitInfo.collider.GameObject() &&
                _jumpCounter == 0)
            {
                _previousDirectionChangeObject = hitInfo.collider.GameObject();
                ChangeDirection();
            }
            else if (_jumpCounter < 2)
            {
                var rigidbody = GetComponent<Rigidbody>();
                // rigidbody.AddForce(new Vector3(0.0f, 4.0f, 0.0f), ForceMode.Impulse);
                rigidbody.velocity = Vector3.up * 4.0f;
                _jumpCounter++;
            }
        }

        _spacePressed = Input.GetKey(KeyCode.Space);
    }

    private void ChangeDirection()
    {
        movementDirection = new Vector3(1.0f, 0.0f, 1.0f) - movementDirection;
    }

    public void OnCollisionEnter(Collision _)
    {
        _jumpCounter = 0;
    }
}