using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ThrowAction : MonoBehaviour
{
    [Header("ターゲット")]
    [SerializeField] GameObject target;
    [Header("投げるオブジェクト")]
    [SerializeField] GameObject throw_prefab;
    [Header("プレイヤーを狙う")]
    [SerializeField] bool is_aim_player = true;
    [Header("狙う半径(ランダムで決める)")]
    [SerializeField]
    [Range(0, 50)]
    float radius = 0.0f;
    [Header("ターゲット座標")]
    [SerializeField, DisableIf(nameof(is_aim_player))]
    Vector3 target_pos;
    [Header("デバック表示")]
    [SerializeField]
    bool draw_debug_line = false;
    [Header("狙う最終座標")]
    [ReadOnly] public Vector3 final_target_pos;
    [Header("最終半径")]
    [ReadOnly] public float final_radius = 0.0f;
    [SerializeField] GameObject model_obj;

    ThrowModelAction model_action;


    // debug
    [SerializeField] Text text;
    [SerializeField] float speed;


    void Start()
    {
        if (model_obj)
        {
            model_action = model_obj.AddComponent<ThrowModelAction>();
        }
    }

    void Update()
    {
        if (draw_debug_line)
        {
            Debug.DrawLine(transform.position, final_target_pos, Color.red);
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (OVRInput.Get(OVRInput.RawButton.Y))
            {
                speed += 0.1f;
            }
            if (OVRInput.Get(OVRInput.RawButton.X))
            {
                speed -= 0.1f;
            }
        }
        text.text = "Speed " + speed.ToString();
        transform.LookAt(target.transform.position, Vector3.up);
    }

    public void SetPosition(Vector3 position, bool aim_player = false)
    {
        target_pos = position;
        is_aim_player = aim_player;
    }

    public void SetRadius(float r)
    {
        radius = r;
    }

    public void SetThrowObject(GameObject obj)
    {
        Debug.Log(obj);
        throw_prefab = obj;
        Debug.Log("throw_prefab" + throw_prefab);
    }

    public float GetSpeed()
    {
        return speed;
    }


    public void Throw()
    {
        if (is_aim_player)
        {
            if (!target)
            {
                Debug.LogWarning("ターゲットがいない");
            }
            target_pos = target.transform.position;
        }
        float range = Random.Range(0.0f, 360.0f);
        float r = Random.Range(0.0f, radius);
        target_pos.x += r * Mathf.Cos(range * Mathf.Deg2Rad);
        target_pos.z += r * Mathf.Sin(range * Mathf.Deg2Rad);
        GameObject throw_object = Instantiate(throw_prefab, transform.position, Quaternion.identity);
        throw_object.GetComponent<ThrowMove>().SetTargetPos(target_pos);
        throw_object.GetComponent<ThrowMove>().SetSpeed(GetSpeed());
        Destroy(throw_object, 5.0f);
        final_target_pos = target_pos;
        final_radius = r;
        if (model_action)
            model_action.Throw();
    }
}
