using Unity.VisualScripting;
using UnityEngine;

public class ThrowModelAction : MonoBehaviour
{
    float scaling_timer = 0.0f;
    float scaling_time_border = 0.4f;
    const float scaling_time_max = 0.5f;
    const float max_scale = 1.5f;
    const float min_scale = 1.0f;
    Vector3 base_scale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base_scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float scale_ = 1.0f;
        if (scaling_timer > scaling_time_border)
        {
            float t = (scaling_timer - scaling_time_border) / (scaling_time_max - scaling_time_border);
       
            scale_ = Mathf.Lerp(min_scale, max_scale, t);
        }
        else if (scaling_timer > 0.0f)
        {
            float t = scaling_timer / scaling_time_border;
            scale_ = Mathf.Lerp(min_scale, max_scale, t);
        }
        transform.localScale = new Vector3(base_scale.x * scale_, base_scale.y * scale_, base_scale.z * scale_);
        scaling_timer -= Time.unscaledDeltaTime;
    }
    public void Throw()
    {
        scaling_timer = scaling_time_max;
    }
}
