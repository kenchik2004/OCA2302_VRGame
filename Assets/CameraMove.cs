using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector2 camera_rot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        camera_rot.x += Input.GetAxis("Mouse X");
        camera_rot.y += Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(-camera_rot.y, camera_rot.x,0.0f);
    }
}
