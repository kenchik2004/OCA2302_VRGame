using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    float timer;
    [SerializeField] float throw_time = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = throw_time;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            gameObject.SendMessage("Throw");
            timer = throw_time;
        }

    }
}
