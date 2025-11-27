using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject TitleUI;
    [SerializeField] GameObject LevelUI;
    [SerializeField] GameObject TutorialUI;
    [SerializeField] GameObject GameTutorialUI;
    [SerializeField] GameObject PlayUI;
    [SerializeField] GameObject[] game_level_ui = new GameObject[3];
    [SerializeField] GameObject[] level_object_prefab = new GameObject[3];
    [SerializeField] GameObject[] level_spawn_pos = new GameObject[3];
    GameObject[] level_object = new GameObject[3];
    int chose_level;
    bool is_chose_level = false;
    float next_scene_timer = 0.0f;
    [SerializeField] float next_scene_time = 3.0f;
    public enum Level
    {
        EASY,
        NORMAL,
        HARD,

        MAX,
    }
    float target_score;

    public enum GameType
    {
        TITLE,
        CHOSE_LEVEL,
        GAME_TUTORIAL,
        GAME,
        END,

        MAX,
    }
    [ReadOnly] GameType now_game_type = GameType.TITLE;

    void Start()
    {
        Init(now_game_type);
    }


    void Update()
    {
        switch (now_game_type)
        {
            case GameType.TITLE:

                break;
            case GameType.CHOSE_LEVEL:             
                for (int i = 0; i < (int)Level.MAX; i++)
                {
                    if (!level_object[i])
                    {
                        chose_level = i;
                        is_chose_level = true;
                    }
                }
                if (is_chose_level)
                {
                    for (int i = 0; i < (int)Level.MAX; i++)
                    {
                        if (level_object[i])
                        {
                            level_object[i].tag = "Untagged";
                        }
                    }
                    next_scene_timer -= Time.deltaTime;
                    if(next_scene_timer<= 0 )
                    {
                        MoveNextScene();
                    }
                }
                break;
            case GameType.GAME_TUTORIAL:

                break;
            case GameType.GAME:

                break;
            case GameType.END:

                break;
            default:
                Debug.LogWarning("予想外のgame_typeにアクセス");
                break;

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MoveNextScene();
        }
    }

    void Init(GameType type)
    {
        switch (type)
        {
            case GameType.TITLE:
                TitleUI.SetActive(true);
                TutorialUI.SetActive(true);
                break;

            case GameType.CHOSE_LEVEL:
                is_chose_level = false;
                LevelUI.SetActive(true);
                for (int i = 0; i < (int)Level.MAX; i++)
                {
                    level_object[i] = Instantiate(level_object_prefab[i], level_spawn_pos[i].transform.position,Quaternion.identity);
                }
                break;

            case GameType.GAME_TUTORIAL:
                GameTutorialUI.SetActive(true);
                break;

            case GameType.GAME:
                PlayUI.SetActive(true);
                SetLevel((Level)chose_level);
                break;

            case GameType.END:
                break;
        }
        next_scene_timer = next_scene_time;
    }

    void Exit(GameType type)
    {
        switch (type)
        {
            case GameType.TITLE:
                TitleUI.SetActive(false);
                TutorialUI.SetActive(false);
                break;
            case GameType.CHOSE_LEVEL:
                LevelUI.SetActive(false);
                for (int i = 0; i < (int)Level.MAX; i++)
                {
                    if(level_object[i])
                    { 
                        Destroy(level_object[i]);
                        level_object[i] = null;
                    }
                }
                break;
            case GameType.GAME_TUTORIAL:
                GameTutorialUI.SetActive(false);
                break;
            case GameType.GAME:
                PlayUI.SetActive(false);
                break;
            case GameType.END:
                break;
        }
    }

    public void ChangeState(GameType nextType)
    {
        if (now_game_type == nextType)
        {
            return;
        }

        Exit(now_game_type);

        now_game_type = nextType;

        Init(now_game_type);
    }



    public void SetTargetScore(float score)
    {
        target_score = score;
    }

    public void SetLevel(Level type)
    {
        int max_level = (int)Level.MAX;
        for (int i = 0; i < max_level; i++)
        {
            if (i == (int)type)
            {
                game_level_ui[i].SetActive(true);
            }
            else
            {
                game_level_ui[i].SetActive(false);
            }
        }
    }

    public void MoveNextScene()
    {
        GameType next = now_game_type + 1;
        if (next >= GameType.MAX)
        {
            next = GameType.TITLE;
        }
        ChangeState(next);
    }
}
