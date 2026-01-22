using UnityEngine;

public class InstantiateOnDestroy : MonoBehaviour
{

    [SerializeField] GameObject instantiate_prefab;
    [SerializeField] float instance_life_time;
    bool has_life_time = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        has_life_time = instance_life_time >= 0.0f;
    }

    private void OnCut()
    {
        if (!instantiate_prefab)
            return;
        //プレイヤーカメラ(カレントカメラ)の方に向けて生成
        Transform cam_trns = Camera.main.transform;
        Vector3 look_pos = cam_trns.position;
        Vector3 look_vec = look_pos - transform.position;
        look_vec.Normalize();
        GameObject instance = Instantiate(instantiate_prefab, transform.position + cam_trns.forward * 2, Quaternion.LookRotation(look_vec, Vector3.up));
        if (has_life_time)
            Destroy(instance, instance_life_time);
    }
}
