using TMPro;
using UnityEngine;

public class ScoreEffectUI : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    [SerializeField] float fade_time = 1.0f;
    [SerializeField] float y_speed = 0.5f;
    float elasped_time = 0.0f;
    Color text_color;
    Color color;
    RectTransform rect_transform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rect_transform = GetComponent<RectTransform>();
        text_color = text.color;
        color = text_color;
        color.a = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        elasped_time += fade_time * Time.deltaTime;
        if(elasped_time > 1.0f)
        {
            Destroy(gameObject);
        }
        rect_transform.position += new Vector3(0, y_speed, 0) * Time.deltaTime;
        text.alpha = Mathf.Lerp(text_color.a, color.a, elasped_time);

    }
}
