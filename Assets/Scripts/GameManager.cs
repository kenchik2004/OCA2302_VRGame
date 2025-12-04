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
    [SerializeField] GameObject[] play_level_ui = new GameObject[3];

    [System.Serializable]
    struct Data
    {
        [SerializeField] public GameObject prefab;
        [SerializeField] public GameObject spawn_pos;
        [ReadOnly] public GameObject obj;
    }

    [SerializeField] Data title_object;
    [SerializeField] Data[] level_object = new Data[3];

    [SerializeField] AudioSource audio_player;
    [SerializeField] AudioClip clear_audio;
    GameObject on_cut_object;


    int chose_level;
    bool is_next_level = false;
    bool is_play_se = false;
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


    public void OnCutObject(GameObject cut_object)
    {
        on_cut_object = cut_object;
    }

    void Update()
    {
        switch (now_game_type)
        {
            case GameType.TITLE:
                if (on_cut_object == title_object.obj && !is_next_level)
                {
                    audio_player.PlayOneShot(clear_audio);
                    on_cut_object = null;
                    is_next_level = true;
                }
                if (is_next_level)
                {
                    StartMoveScene();
                }
                break;
            case GameType.CHOSE_LEVEL:
                if (is_next_level)
                {
                    for (int i = 0; i < (int)Level.MAX; i++)
                    {
                        if (level_object[i].obj)
                        {
                            level_object[i].obj.tag = "Untagged";
                        }
                    }
                    StartMoveScene();
                    break;
                }
                for (int i = 0; i < (int)Level.MAX; i++)
                {
                    if (on_cut_object == level_object[i].obj)
                    {
                        chose_level = i;
                        is_next_level = true;
                        audio_player.PlayOneShot(clear_audio);
                        on_cut_object = null;
                    }
                }
                break;
            case GameType.GAME_TUTORIAL:

                break;
            case GameType.GAME:

                break;
            case GameType.END:
                StartMoveScene();
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
        is_next_level = false;
        switch (type)
        {
            case GameType.TITLE:
                {
                    TitleUI.SetActive(true);
                    TutorialUI.SetActive(true);
                    Vector3 pos = title_object.spawn_pos.transform.position;
                    GameObject prefab = title_object.prefab;
                    title_object.obj = Instantiate(prefab, pos, Quaternion.identity);
                }
                break;

            case GameType.CHOSE_LEVEL:
                {
                    LevelUI.SetActive(true);
                    for (int i = 0; i < (int)Level.MAX; i++)
                    {
                        Vector3 pos = level_object[i].spawn_pos.transform.position;
                        GameObject prefab = level_object[i].prefab;
                        level_object[i].obj = Instantiate(prefab, pos, Quaternion.identity);
                    }
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
        is_play_se = false;
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
                    if (level_object[i].obj)
                    {
                        Destroy(level_object[i].obj);
                        level_object[i].obj = null;
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
                play_level_ui[i].SetActive(true);
            }
            else
            {
                play_level_ui[i].SetActive(false);
            }
        }
    }

    public void StartMoveScene()
    {
        next_scene_timer -= Time.deltaTime;
        if (next_scene_timer <= 0)
        {
            MoveNextScene();
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
