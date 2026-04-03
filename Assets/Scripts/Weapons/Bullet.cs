using UnityEngine;

public class Bullet : MonoBehaviour {
    public float damage = 25f;
    public float lifetime = 5f;
    public GameObject impactPrefab;
    public AudioClip impactSound;

    private float spawnTime;

    void Start() {
        spawnTime = Time.time;
    }

    void Update() {
        if (Time.time - spawnTime > lifetime)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);
            CreateImpact(collision.transform.position);
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Environment") || collision.CompareTag("Wall")) {
            CreateImpact(collision.transform.position);
            Destroy(gameObject);
        }
    }

    private void CreateImpact(Vector3 hitPoint) {
        if (impactPrefab != null)
            Instantiate(impactPrefab, hitPoint, Quaternion.identity);
        if (impactSound != null)
            AudioSource.PlayClipAtPoint(impactSound, hitPoint);
    }
}