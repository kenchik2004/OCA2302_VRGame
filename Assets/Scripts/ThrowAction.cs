using UnityEngine;

public class ThrowAction : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject throw_prefab;
    [SerializeField] bool is_aim_player;
    [SerializeField]
    [Range(0, 100)]
    float radius = 0.0f;
    [SerializeField] Vector3 target_pos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(is_aim_player)
        {
            target_pos = player.transform.position;
        }


    }

    void SetPosition(Vector3 position)
    {
        target_pos = position;
        is_aim_player = false;
    }

    void SetRadius(float r)
    {
        radius = r;
    }
}
