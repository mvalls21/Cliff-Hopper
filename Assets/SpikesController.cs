using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SpikesController : MonoBehaviour
{
    private bool _moved = false;

    private Vector3 _basePosition;

    private MovementDirection _movement = MovementDirection.None;

    private Vector3 _objectivePosition;

    public void Update()
    {
        if (_movement == MovementDirection.None) return;

        if (_movement == MovementDirection.Up)
        {
            transform.Translate(new Vector3(0.0f, 5.0f, 0.0f) * Time.deltaTime);

            if (transform.position.y > _objectivePosition.y)
            {
                _movement = MovementDirection.Down;
            }
        }
        else if (_movement == MovementDirection.Down)
        {
            transform.Translate(new Vector3(0.0f, -1.0f, 0.0f) * Time.deltaTime);

            if (transform.position.y < _basePosition.y)
            {
                transform.position = _basePosition;
                _moved = true;
                _movement = MovementDirection.None;
            }
        }
    }

    public void CollisionPlayer()
    {
        if (_moved || _movement != MovementDirection.None) return;

        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        _basePosition = transform.position;
        _objectivePosition = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        _movement = MovementDirection.Up;
    }

    private enum MovementDirection
    {
        None,
        Up,
        Down
    }
}