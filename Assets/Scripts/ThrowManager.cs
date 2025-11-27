using UnityEngine;
using NaughtyAttributes;

public class ThrowManager : MonoBehaviour
{
    [Header("スポナー")]
    [SerializeField] GameObject[] spawners;
    [Header("投げられるオブジェクト")]
    [SerializeField] GameObject[] throw_objects;
    [SerializeField] float throw_time = 3.0f;
    [ReadOnly] public int chose_spawner;
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = throw_time;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            GameObject spawner = ChoseRandomSpawner();
            GameObject obj = ChoseRandomObject();
            DoThrow(spawner, obj);
            timer = throw_time;
        }

    }

    GameObject ChoseRandomObject()
    {
        if (throw_objects.Length <= 0)
        {
            Debug.LogWarning("投げられるオブジェクトがいない");
            return null;
        }
        int rand = Random.Range(0, throw_objects.Length);
        GameObject obj = throw_objects[rand];
        return obj;
    }


    GameObject ChoseRandomSpawner()
    {
        if (spawners.Length <= 0)
        {
            Debug.LogWarning("スポナーがいない");
            return null;
        }
        int rand = Random.Range(0, spawners.Length);
        GameObject obj = spawners[rand];
        return obj;
    }

    void DoThrow(GameObject spawner, GameObject obj)
    {
        var throw_action = spawner.GetComponent<ThrowAction>();
        if (throw_action)
        {
            throw_action.SetThrowObject(obj);
            throw_action.Throw();
        }
        else
        {
            throw_action = spawner.AddComponent<ThrowAction>();
            throw_action.SetThrowObject(obj);
            throw_action.Throw();
        }
    }

}
