using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VolcanoController : MonoBehaviour
{
    public GameObject fireballPrefab;

    private bool _fireballsLaunched = false;

    private const int NumberFireballs = 4;

    private const float LookBack = 1.4f;

    private readonly List<GameObject> _fireballs = new List<GameObject>();

    public void Update()
    {
        var xHit = Physics.Raycast(transform.position - new Vector3(LookBack, 0.0f, 0.0f), Vector3.up,
            out RaycastHit infoX);
        var zHit = Physics.Raycast(transform.position - new Vector3(0.0f, 0.0f, LookBack), Vector3.up,
            out RaycastHit infoZ);

        var playerX = xHit && infoX.collider.gameObject.CompareTag("Player");
        var playerZ = zHit && infoZ.collider.gameObject.CompareTag("Player");

        if ((playerX || playerZ) && !_fireballsLaunched)
        {
            // Launch fireballs up
            for (int i = 0; i < NumberFireballs; ++i)
            {
                var offset = new Vector3(Random.Range(-0.3f, 0.3f), 0.0f, Random.Range(-0.3f, 0.3f));

                var fireball = Instantiate(fireballPrefab);
                fireball.transform.position = transform.position + offset;
               
                if (i != 0)
                {
                    var audioSource = fireball.GetComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                    audioSource.Stop();
                }

                var rigidbody = fireball.GetComponent<Rigidbody>();
                rigidbody.useGravity = true;
                rigidbody.AddForce(Vector3.up * 7.0f, ForceMode.Impulse);

                _fireballs.Add(fireball);
            }

            _fireballsLaunched = true;
        }

        if (_fireballsLaunched &&
            _fireballs.Any(fireball => fireball.transform.position.y < transform.position.y - 1.0f))
        {
            foreach (var fireball in _fireballs)
                Destroy(fireball);
            _fireballs.Clear();
        }
    }
}