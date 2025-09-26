using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector2 camera_rot;
    [SerializeField] GameObject eye_anchor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            camera_rot.x += Input.GetAxis("Mouse X");
            camera_rot.y += Input.GetAxis("Mouse Y");

            Vector3 mov = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
                mov += transform.forward;
            if (Input.GetKey(KeyCode.S))
                mov -= transform.forward;
            if (Input.GetKey(KeyCode.D))
                mov += transform.right;
            if (Input.GetKey(KeyCode.A))
                mov -= transform.right;
            mov = Vector3.ProjectOnPlane(mov, Vector3.up);
            mov.Normalize();
            transform.position += mov * Time.deltaTime * 10;

            transform.eulerAngles = new Vector3(-camera_rot.y, camera_rot.x, 0.0f);
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            Vector2 input = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
            Vector3 mov = eye_anchor.transform.forward * input.y;
            mov += eye_anchor.transform.right * input.x;
            mov = Vector3.ProjectOnPlane(mov, Vector3.up);
            mov.Normalize();
            transform.position += mov * Time.deltaTime * 10;
        }
    }
}
