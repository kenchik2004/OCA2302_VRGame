using UnityEngine;
public class VRAction : MonoBehaviour
{
    Vector2 Rot = Vector2.zero;
    [SerializeField] Vector2 Bias = new Vector2(5.0f, 5.0f); //��]�W��
    [SerializeField] float maxPitch = 60.0f; //�p����
    [SerializeField] float minPitch = -80.0f; //��p����
    static public bool isVR; //VR�����ǂ����̐^�U�l
    void Awake()
    {
        isVR = (Application.platform == RuntimePlatform.Android); //VR���̔�����s��
    }
    void Update()
    {
        if (!isVR)
        {
            Rot.x = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Bias.x;
            Rot.y += Input.GetAxis("Mouse Y") * Bias.y;
            Rot.y = Mathf.Clamp(Rot.y, minPitch, maxPitch);
            transform.localEulerAngles = new Vector3(-Rot.y, Rot.x, 0);
        }
    }
}
