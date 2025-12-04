using UnityEngine;

public class CutMaterialManager : MonoBehaviour
{
    [SerializeField] Material[] materials;
    [SerializeField] Material[] pair_materials;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public Material FindPairMaterial(Material mat)
    {
        int index = 0;
        foreach (var mat_ in materials)
        {
            if (mat_ == mat && index < pair_materials.Length)
                return pair_materials[index];
            index++;
        }
        return mat;
    }
}
