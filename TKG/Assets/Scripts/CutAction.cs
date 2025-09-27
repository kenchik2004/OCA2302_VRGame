using UnityEngine;

public class CutAction : MonoBehaviour
{
    Vector3 dir;
    Vector3 cut_start;
    Vector3 cut_end;
    Vector3 cut_normal;
    public Mesh cutting_mesh;
    public Vector3[] cutting_mesh_vertices;
    [SerializeField] Collider my_col;

    [SerializeField] Transform line_start;
    [SerializeField] Transform line_end;
    [SerializeField] Transform child_queue;
    [SerializeField] float delta_force = 10;
    Vector3 sword_end_delta = new Vector3(0, 0, 0);
    Vector3 sword_end_prev = new Vector3(0, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            transform.parent.localRotation = Quaternion.Euler(90, 0, 0);
            transform.parent.localPosition = new Vector3(0, -0.1f, 0);
        }
        dir = new Vector3(-1, 1, 0);
        dir.Normalize();
        sword_end_prev = line_end.position;
    }

    private void FixedUpdate()
    {
        sword_end_delta = line_end.position - sword_end_prev;
        sword_end_prev = line_end.position;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cut_start - Vector3.up * 0.5f, Vector3.up, Color.red);
        Debug.DrawRay(cut_start - Vector3.right * 0.5f, Vector3.right, Color.red);
        Debug.DrawRay(cut_end - Vector3.up * 0.5f, Vector3.up, Color.green);
        Debug.DrawRay(cut_end - Vector3.right * 0.5f, Vector3.right, Color.green);
        if (sword_end_delta.magnitude > delta_force)
        {
            int child_count = child_queue.childCount;
            for (int i = 0; i < child_count; i++)
            {
                var child_ = child_queue.GetChild(i);
                var child_rb = child_.GetComponent<Rigidbody>();
                if (child_rb)
                {
                    child_rb.constraints = RigidbodyConstraints.None;
                    child_rb.isKinematic = false;
                    child_rb.linearVelocity = transform.up * 10 * sword_end_delta.magnitude;

                }
                child_.gameObject.tag = "CutObject";
            }
            child_queue.DetachChildren();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CutObject" && cutting_mesh == null)
        {
            if (Vector3.Dot(sword_end_delta, transform.up) < 0.2f)
            {
                other.transform.SetParent(child_queue);

                other.gameObject.tag = "Untagged";
                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    rb.isKinematic = true;

                }
                return;
            }
            Mesh mesh = other.GetComponent<MeshFilter>().mesh;
            cutting_mesh = mesh;
            cutting_mesh_vertices = mesh.vertices;
            cut_start = CuttingPointDetector.DetectClosestPointOnLine(cutting_mesh_vertices, line_start.position, line_end.position);
            //cut_start = other.ClosestPointOnBounds(my_col.ClosestPointOnBounds(other.transform.position));

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CutObject" && cutting_mesh)
        {

            cut_end = CuttingPointDetector.DetectClosestPointOnLine(cutting_mesh_vertices, line_start.position, line_end.position);
            //cut_end = other.ClosestPointOnBounds(my_col.ClosestPointOnBounds(other.transform.position));
            dir = cut_end - cut_start;
            cut_normal = Vector3.Cross(dir.normalized, transform.up);
            if (cut_normal.magnitude <= 0.5f)
            {
                other.transform.SetParent(child_queue);
                cutting_mesh = null;
                cutting_mesh_vertices = null;
                other.gameObject.tag = "Untagged";
                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    rb.isKinematic = true;

                }
                return;
            }
            cut_normal.Normalize();
            var result = MeshCut.CutMesh(other.gameObject, Vector3.Lerp(cut_start, cut_end, 0.5f), cut_normal);
            //カット結果が小さすぎてnullが返る場合は、処理スキップ
            if (result.copy_normalside && result.original_anitiNormalside)
            {
                //result.copy_normalside.tag = "Untagged";
                //result.original_anitiNormalside.tag = "Untagged";
                var rb = result.copy_normalside.GetComponent<Rigidbody>();
                if (rb)
                    rb.AddForce(cut_normal * 5, ForceMode.VelocityChange);
                rb = result.original_anitiNormalside.GetComponent<Rigidbody>();
                if (rb)
                    rb.AddForce(-cut_normal * 5, ForceMode.VelocityChange);
            }
            cutting_mesh = null;
            cutting_mesh_vertices = null;
        }
    }
}
