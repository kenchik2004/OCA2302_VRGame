using UnityEngine;

public class MyScore : MonoBehaviour
{
    [SerializeField] int score = 100;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetScore()
    {
        return score;
    }
}
