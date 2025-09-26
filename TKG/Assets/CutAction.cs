using UnityEngine;

public class CutAction : MonoBehaviour
{
    Vector3 dir;
    Vector3 cut_start;
    Vector3 cut_end;
    Vector3 cut_normal;
    public GameObject cutting_object;
    [SerializeField] Collider my_col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dir = new Vector3(-1, 1, 0);
        dir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cut_start, Vector3.up, Color.red);
        Debug.DrawRay(cut_end, Vector3.up, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CutObject" && cutting_object == null)
        {
            cut_start = other.ClosestPointOnBounds(my_col.ClosestPointOnBounds(other.transform.position));
            cutting_object = other.gameObject;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CutObject")
        {
            cut_end = other.ClosestPointOnBounds(my_col.ClosestPointOnBounds(other.transform.position));
            dir = cut_end - cut_start;
            cut_normal = Vector3.Cross(dir.normalized, transform.up);
            MeshFilter meshFilter = other.gameObject.GetComponent<MeshFilter>();
            if (cut_normal.sqrMagnitude <= 0.1f)
            {
                other.transform.SetParent(transform);
                cutting_object = null;
                other.gameObject.tag = "Untagged";
                return;
            }
            cut_normal.Normalize();
            Debug.Log(cut_normal);
            var result = MeshCut.CutMesh(other.gameObject, Vector3.Lerp(cut_start, cut_end, 0.5f), cut_normal);
            //result.copy_normalside.tag = "Untagged";
            //result.original_anitiNormalside.tag = "Untagged";
            result.copy_normalside.GetComponent<Rigidbody>().AddForce(dir, ForceMode.VelocityChange);
            result.original_anitiNormalside.GetComponent<Rigidbody>().AddForce(dir, ForceMode.VelocityChange);
            cutting_object = null;
        }
    }
}
