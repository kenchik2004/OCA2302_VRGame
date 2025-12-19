using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CutAction : MonoBehaviour
{
    Vector3 dir;
    Vector3 cut_start;
    Vector3 cut_end;
    Vector3 cut_normal;
    public Mesh cutting_mesh;
    public Vector3[] cutting_mesh_vertices;

    [SerializeField] Transform line_start;
    [SerializeField] Transform line_end;
    [SerializeField] float delta_force = 10;
    [SerializeField] bool for_debugger = false;
    Vector3 sword_end_delta = new Vector3(0, 0, 0);
    Vector3 sword_end_prev = new Vector3(0, 0, 0);
    [SerializeField] GameObject game_manager;
    [SerializeField] GameObject score_effect;
    [SerializeField] GameObject player;


    CutMaterialManager cut_mat_manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor && for_debugger)
        {
            transform.parent.localRotation = Quaternion.Euler(90, 0, 0);
            transform.parent.localPosition = new Vector3(0, -0.1f, 0);
        }
        dir = new Vector3(-1, 1, 0);
        dir.Normalize();
        sword_end_prev = line_end.position;
        if (!game_manager || !cut_mat_manager)
        {
            game_manager = GameObject.FindGameObjectWithTag("GameController");
            cut_mat_manager = game_manager.GetComponent<CutMaterialManager>();
        }
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


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CutObject" && cutting_mesh == null)
        {

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
            //Debug.Log(cut_normal.magnitude);
            if (cut_normal.magnitude <= 0.1f)
            {
                float null_rot;
                Random.rotation.ToAngleAxis(out null_rot, out dir);
                cut_normal = Vector3.Cross(dir.normalized, transform.up);
                //return;
            }
            Debug.DrawLine(transform.position - dir * 0.5f, transform.position + dir * 0.5f, Color.black);
            cut_normal.Normalize();
            MeshRenderer renderer = other.gameObject.GetComponent<MeshRenderer>();
            Material cut_mat = null;
            if (renderer)
                cut_mat = cut_mat_manager.FindPairMaterial(renderer.sharedMaterial);
            var result = MeshCut.CutMesh(other.gameObject, Vector3.Lerp(cut_start, cut_end, 0.5f), cut_normal, true, cut_mat);
            //カット結果が小さすぎてnullが返る場合は、処理スキップ
            if (result.copy_normalside && result.original_anitiNormalside)
            {
                var rb = result.copy_normalside.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddForce(cut_normal * 5, ForceMode.VelocityChange);
                    rb.isKinematic = true;
                }
                rb = result.original_anitiNormalside.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddForce(-cut_normal * 5, ForceMode.VelocityChange);

                }

                result.original_anitiNormalside.tag = "Untagged";
                result.copy_normalside.tag = "Untagged";
                Destroy(result.original_anitiNormalside, 2.0f);
                Destroy(result.copy_normalside, 2.0f);
            }
            cutting_mesh = null;
            cutting_mesh_vertices = null;
        }
        else if (other.gameObject.tag == "CutObject")
        {

            cut_end = transform.position;
            dir = cut_end - cut_start;
            cut_normal = Vector3.Cross(dir.normalized, transform.up);
            // Debug.Log(cut_normal.magnitude);
            if (cut_normal.magnitude <= 0.1f)
            {
                float null_rot;
                Random.rotation.ToAngleAxis(out null_rot, out dir);
                cut_normal = Vector3.Cross(dir.normalized, transform.up);
                //return;
            }
            Debug.DrawLine(transform.position - dir * 0.5f, transform.position + dir * 0.5f, Color.black);
            cut_normal.Normalize();
            MeshRenderer renderer = other.gameObject.GetComponent<MeshRenderer>();
            Material cut_mat = null;
            if (renderer)
                cut_mat = cut_mat_manager.FindPairMaterial(renderer.sharedMaterial);
            var result = MeshCut.CutMesh(other.gameObject, Vector3.Lerp(cut_start, cut_end, 0.5f), cut_normal, true, cut_mat);
            //カット結果が小さすぎてnullが返る場合は、処理スキップ
            if (result.copy_normalside && result.original_anitiNormalside)
            {
                var rb = result.copy_normalside.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddForce(cut_normal * 5, ForceMode.VelocityChange);
                    rb.isKinematic = true;
                }
                rb = result.original_anitiNormalside.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddForce(-cut_normal * 5, ForceMode.VelocityChange);

                }

                result.original_anitiNormalside.tag = "Untagged";
                result.copy_normalside.tag = "Untagged";
                Destroy(result.original_anitiNormalside, 2.0f);
                Destroy(result.copy_normalside, 2.0f);
            }
        }
        //Debug.Log(other.gameObject);
        var score = other.gameObject.GetComponent<MyScore>();
        if (score)
        {
            game_manager.BroadcastMessage("AddScore", score.GetScore(), SendMessageOptions.DontRequireReceiver);

            Vector3 direction = (cut_end - player.transform.position).normalized;

            var effect = Instantiate(score_effect, cut_end, Quaternion.LookRotation(direction));
            var text = effect.GetComponent<TextMeshPro>();
            text.text = score.GetScore().ToString();
        }
        game_manager.BroadcastMessage("OnCutObject", other.gameObject, SendMessageOptions.DontRequireReceiver);
    }
}
