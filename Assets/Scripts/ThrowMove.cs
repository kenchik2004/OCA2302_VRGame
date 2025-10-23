using NaughtyAttributes;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ThrowMove : MonoBehaviour
{
    [Header("ターゲット")]
    [SerializeField] GameObject target;
    [Header("目標にかかる時間")]
    [SerializeField]
    [Range(0.1f, 50)]
    float time = 5.0f;
    [Header("上に投げる速度")]
    [SerializeField, EnableIf(nameof(use_custom_speed))]
    [Range(0.1f, 50)]
    float speed = 3.0f;
    [Header("速度を調整できるオプション")]
    [SerializeField] bool use_custom_speed = false;

    Vector3 target_pos;
    float initial_y_speed;
    float y;
    Vector3 start_pos;
    float dt;
    Vector3 total_vec;
    void Start()
    {
        var rigid = GetComponent<Rigidbody>();
        if (rigid)
        {
            rigid.useGravity = false;
        }
        start_pos = transform.position;
        if (target)
        {
            target_pos = target.transform.position;
        }
        Vector3 vec = target_pos - start_pos;

        initial_y_speed = (vec.y + 0.5f * 9.8f * time * time) / time;

        vec.y = 0.0f;
        total_vec = vec;
    }

    void Update()
    {
        dt += Time.deltaTime;

        // y軸の移動
        //y = speed * Time - 0.5f * 9.8f * Time.deltaTime * Time.deltaTime;
        float final_y = 0.0f;
        if (use_custom_speed)
        {
            y = (speed * dt - (0.5f * 9.8f * dt * dt));
            float vec_y = target_pos.y - start_pos.y;
            float move_y = vec_y * dt;
            final_y = start_pos.y + y + move_y;
        }
        else
        {
            y = initial_y_speed * dt - 0.5f * 9.8f * dt * dt;
            final_y = start_pos.y + y;
        }


        Vector3 xz = start_pos + total_vec * dt / time;
        transform.position = new Vector3(xz.x, final_y, xz.z);
    }

    void SetTargetPos(Vector3 pos)
    {
        target_pos = pos;
    }
    void SetTarget(GameObject gameObject)
    {
        target = gameObject;
        target.transform.position = target_pos;
    }
}
