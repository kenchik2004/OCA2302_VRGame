using UnityEngine;

public class SlashAction : MonoBehaviour
{
    [SerializeField] float life_time = 5.0f;
    [SerializeField] float mov_speed = 1.0f;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.forward * mov_speed;
        life_time -= Time.deltaTime;
        if (life_time <= 0.0f)
            Destroy(gameObject);
    }
}
