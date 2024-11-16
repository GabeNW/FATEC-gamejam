using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Gamejuice")]
    [SerializeField] private GameObject fallParticles;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(1, 0.5f);
    [SerializeField] private Vector2 offSet = new Vector2(0, -0.5f);

    private void Start()
    {
        GetComponent<BoxCollider2D>().size = groundCheckSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            SpawnSmokeParticles();
        }
    }

    private void SpawnSmokeParticles()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x - offSet.x, transform.position.y - offSet.y, transform.position.z);
        GameObject particles = Instantiate(fallParticles, spawnPosition, Quaternion.identity, transform);
        Destroy(particles, 3.5f);
    }
}
