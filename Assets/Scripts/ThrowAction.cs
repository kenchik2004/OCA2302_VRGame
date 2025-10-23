using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
public class ThrowAction : MonoBehaviour
{
    [Header("�^�[�Q�b�g")]
    [SerializeField] GameObject target;
    [Header("������I�u�W�F�N�g")]
    [SerializeField] GameObject throw_prefab;
    [Header("�v���C���[��_��")]
    [SerializeField] bool is_aim_player = true;
    [Header("�_�����a(�����_���Ō��߂�)")]
    [SerializeField]
    [Range(0, 50)]
    float radius = 0.0f;
    [Header("�^�[�Q�b�g���W")]
    [SerializeField, DisableIf(nameof(is_aim_player))] 
    Vector3 target_pos;
    [Header("�_���ŏI���W")]
    [ReadOnly] public Vector3 final_target_pos;
    [Header("�ŏI���a")]
    [ReadOnly] public float final_radius = 0.0f;


    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Throw();
        }
        Debug.DrawLine(transform.position, final_target_pos, Color.red);

    }

    void SetPosition(Vector3 position,bool aim_player = false)
    {
        target_pos = position;
        is_aim_player = aim_player;
    }

    void SetRadius(float r)
    {
        radius = r;
    }

    void SetThrowObject(GameObject gameObject)
    {
        throw_prefab = gameObject;
    }

    void Throw()
    {
        if (is_aim_player)
        {
            if(!target)
            {
                Debug.LogWarning("�^�[�Q�b�g�����Ȃ�");
            }
            target_pos = target.transform.position;
        }
        float range = Random.Range(0.0f, 360.0f);
        float r = Random.Range(0.0f, radius);
        target_pos.x += r * Mathf.Cos(range * Mathf.Deg2Rad);
        target_pos.z += r * Mathf.Sin(range * Mathf.Deg2Rad);
        GameObject throw_object = Instantiate(throw_prefab, transform.position, Quaternion.identity);
        throw_object.AddComponent<ThrowMove>().SendMessage("SetTargetPos", target_pos);
        Destroy(throw_object, 5.0f);
        final_target_pos = target_pos;
        final_radius = r;
    }
}
