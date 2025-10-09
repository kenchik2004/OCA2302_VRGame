using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float cool_time = 2.0f;
    float timer = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = cool_time;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            GameObject spawned = Instantiate(prefab);
            Rigidbody spawned_rb = spawned.GetComponent<Rigidbody>();

            spawned_rb.AddForce(new Vector3(0, 5, -1), ForceMode.Impulse);
            Destroy(spawned, 5);
            timer = cool_time;
        }
    }
}
