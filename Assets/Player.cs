using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public Vector3 movementDirection { get; private set; }

    public float speed = 2.0f;

    private GameObject _previousDirectionChangeObject;

    public GameObject gameManagerObject;

    private GameManager _gameManager;

    public AudioClip deadAudioClip;

    public AudioClip jumpAudioClip;

    private AudioSource _audioSource;

    public GameObject bloodParticle;

    private int _jumpCounter = 0;

    private bool _spacePressed = false;

    public void Start()
    {
        movementDirection = new Vector3(0.0f, 0.0f, 1.0f);
        _gameManager = gameManagerObject.GetComponent<GameManager>();
        transform.position = new Vector3(0.0f, 1.0f, 0.0f);
        _audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (!GameManager.IsPlayerAlive)
            return;

        var hit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo);

        //
        // Spike collision
        //
        if (hit && hitInfo.collider.CompareTag("Spike") && _jumpCounter == 0)
        {
            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.AddForce(new Vector3(0.0f, 10.0f, 5.0f), ForceMode.Impulse);

            var spikesController = hitInfo.collider.GetComponent<SpikesController>();
            spikesController.CollisionPlayer();

            PlayerDied();
            return;
        }

        //
        // Movement
        //
        var movementSpeed = speed;
        if (hit && hitInfo.collider.GameObject().CompareTag("Slowdown") && _jumpCounter == 0)
            movementSpeed = speed / 2.0f;

        var newPosition = transform.position + movementDirection * Time.deltaTime;
        var centerPosition = new Vector3(Mathf.Round(newPosition.x), newPosition.y, Mathf.Round(newPosition.z));

        // Only apply correction to the axis where the player is not moving (to prevent stutters while moving)
        var movement = Vector3.Scale((centerPosition - transform.position),
            (Vector3.one - movementDirection)) + movementDirection;

        transform.Translate(movement * movementSpeed * Time.deltaTime);

        // 
        // Jump
        //
        if (Input.GetKey(KeyCode.Space) && !_spacePressed)
        {
            if (hit && hitInfo.collider.GameObject().CompareTag("DirectionChange") &&
                _previousDirectionChangeObject != hitInfo.collider.GameObject() &&
                _jumpCounter == 0)
            {
                _previousDirectionChangeObject = hitInfo.collider.GameObject();
                ChangeDirection();
            }
            else if (_jumpCounter < 2)
            {
                var rigidbody = GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.up * 4.2f;
                _jumpCounter++;
            }
        }

        // 
        // Check fell off map 
        // 
        if (_previousDirectionChangeObject == null && transform.position.y < -2.0f)
        {
            PlayerDied();
        }
        else if (_previousDirectionChangeObject != null &&
                 _previousDirectionChangeObject.transform.position.y - transform.position.y > 4.0f)
        {
            PlayerDied();
        }

        _spacePressed = Input.GetKey(KeyCode.Space);
    }

    private void ChangeDirection()
    {
        movementDirection = new Vector3(1.0f, 0.0f, 1.0f) - movementDirection;
        _gameManager.IncreaseScore();
    }

    private void PlayerDied()
    {
        if (!GameManager.IsPlayerAlive) return;

        var mesh = GetComponent<MeshRenderer>();
        Destroy(mesh);

        for (int i = 0; i < 8; ++i)
        {
            var blood = Instantiate(bloodParticle);
            blood.transform.position = transform.position;

            var rigidBody = blood.GetComponent<Rigidbody>();

            var horizontalStrength = 1.0f;
            var direction = i % 2 == 0 ? 1 : -1;
            var force = new Vector3(horizontalStrength * movementDirection.z * direction, 1.0f,
                horizontalStrength * movementDirection.x * direction);
            rigidBody.AddForce(force, ForceMode.Impulse);
        }

        _audioSource.PlayOneShot(deadAudioClip);
        _gameManager.PlayerDied();
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "RollingStone")
        {
            PlayerDied();
        }
        else
        {
            _jumpCounter = 0;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fireball"))
        {
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(new Vector3(-10.0f, 7.0f, 0.0f), ForceMode.Impulse);

            PlayerDied();
        }
    }
}