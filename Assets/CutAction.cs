using UnityEngine;

public class CutAction : MonoBehaviour
{
    Vector3 dir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dir=new Vector3(-1,1,0);
        dir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CutObject")
        {
            MeshFilter meshFilter = other.gameObject.GetComponent<MeshFilter>();
            var result = MeshCut.CutMesh(other.gameObject,other.gameObject.transform.position, dir);
            //result.copy_normalside.tag = "Untagged";
            result.original_anitiNormalside.tag = "Untagged";
            result.copy_normalside.GetComponent<Rigidbody>().AddForce(new Vector3(0,1,1));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
