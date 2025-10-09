using UnityEngine;

public class PreCutAction : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("!!");
        if (other.CompareTag("Sword"))
        {
            var instance = Instantiate(prefab, transform.position, transform.rotation );
            instance.GetComponent<Rigidbody>().AddForce(Vector3.up * 10);
            transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
