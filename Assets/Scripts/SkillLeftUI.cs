using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkillLeftUI : MonoBehaviour
{
    [SerializeField] GameObject ui_object;
    [SerializeField] int left_count = 3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < left_count; i++)
        {
            float width = ui_object.GetComponent<RectTransform>().sizeDelta.x;
            width *= transform.localScale.x;
            var obj = Instantiate(ui_object, new Vector3(transform.position.x - width * i, transform.position.y, transform.position.z), Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeleteUIObject(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }
}
