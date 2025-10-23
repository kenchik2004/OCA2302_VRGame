using UnityEngine;

public class ChargeSlashAction : MonoBehaviour
{
    [SerializeField] float charge_time = 2.0f;
    [SerializeField] float capcher_time = 0.1f;
    [SerializeField] Transform sword_end;
    [SerializeField] GameObject slash_prefab;
    public float charge_timer = 0.0f;
    public float capcher_timer = 0.0f;
    Vector3 slash_start;
    Vector3 slash_end;
    public enum SLASH_STATE
    {
        NOT_READY,
        CHARGE,
        READY,
        CAPCHER_START,
        CAPCHER,
        CAPCHER_END,
        SLASH

    }
    public SLASH_STATE slash_state;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charge_timer = charge_time;
        capcher_timer = capcher_time;
        slash_state = SLASH_STATE.NOT_READY;
    }

    // Update is called once per frame
    void Update()
    {
        switch (slash_state)
        {
            case SLASH_STATE.NOT_READY:
                break;

            case SLASH_STATE.CHARGE:

                charge_timer -= Time.deltaTime;
                if (charge_timer <= 0.0f)
                    slash_state = SLASH_STATE.READY;
                break;

            case SLASH_STATE.READY:

                break;

            case SLASH_STATE.CAPCHER_START:
                slash_start = sword_end.position;
                slash_state = SLASH_STATE.CAPCHER;
                break;
            case SLASH_STATE.CAPCHER:
                capcher_timer -= Time.deltaTime;
                if (capcher_timer <= 0.0f)
                {
                    capcher_timer = capcher_time;
                    slash_state = SLASH_STATE.CAPCHER_END;
                }
                break;

            case SLASH_STATE.CAPCHER_END:
                slash_end = sword_end.position;
                slash_state = SLASH_STATE.SLASH;
                break;

            default:
                Debug.DrawRay(slash_start, slash_end - slash_start, Color.green, 1.0f);
                Quaternion instance_rot = Quaternion.LookRotation(transform.forward, Vector3.Cross(slash_end - slash_start, transform.forward));
                Vector3 instance_pos = Vector3.Lerp(slash_start, slash_end, 0.5f);
                Instantiate(slash_prefab, instance_pos, instance_rot);
                slash_state = SLASH_STATE.NOT_READY;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("!?!?");
        if (other.tag == "Hip")
            slash_state = SLASH_STATE.CHARGE;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("!?!?");
        if (other.tag == "Hip")
        {
            if (slash_state == SLASH_STATE.READY)
                slash_state = SLASH_STATE.CAPCHER_START;
            else
                slash_state = SLASH_STATE.NOT_READY;
            charge_timer = charge_time;
        }
    }
}
