using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject title_ui;
    [SerializeField] GameObject level_ui;
    [SerializeField] GameObject tutorial_ui;
    [SerializeField] GameObject game_tutorial_ui;
    [SerializeField] GameObject play_ui;
    [SerializeField] GameObject[] play_level_ui = new GameObject[3];
    [SerializeField] GameObject over_ui;
    [SerializeField] GameObject throw_manager;

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
    [SerializeField] AudioClip[] score_se = new AudioClip[5];
    [SerializeField] AudioClip[] scoring_se = new AudioClip[2];
    GameObject on_cut_object;


    [Header("難易度")]
    [SerializeField] int chose_level;
    [Header("難易度別投擲クールタイム")]
    [SerializeField] float[] cool_times = new float[3];
    bool is_next_level = false;
    float next_scene_timer = 0.0f;
    [SerializeField] float next_scene_time = 3.0f;
    [SerializeField] Text game_score_text;
    int game_score;
    [SerializeField] Text game_time_text;
    [SerializeField] ChargeSlashAction charge_slash;
    float game_time;
    [Header("ゲーム遊べる時間")]
    [SerializeField] float play_time = 100.0f;
    [SerializeField] Text over_score_text;
    [SerializeField] Text over_score_level;

    [Header("スコアランク区分(C~SS)")]
    [SerializeField] int[] rank_borders = new int[5];
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
        Debug.Log("on_cut_object" + cut_object);
        Debug.Log("title_object" + title_object.obj);
    }

    void Update()
    {
        switch (now_game_type)
        {
            case GameType.TITLE:
                if (!title_object.obj && !is_next_level)
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
                if (Input.GetKeyDown(KeyCode.A) || OVRInput.Get(OVRInput.RawButton.A))
                {
                    MoveNextScene();
                }
                break;
            case GameType.GAME:
                game_score_text.text = game_score.ToString();
                game_time -= Time.deltaTime;
                game_time = Mathf.Max(game_time, 0.0f);
                game_time_text.text = game_time.ToString("F2");


                if (game_time <= 0.0f)
                {
                    var manager = throw_manager.GetComponent<ThrowManager>();
                    if (manager)
                    {
                        manager.StopSpawn(true);
                    }
                    StartMoveScene();
                }
                break;
            case GameType.END:
                if (game_time > 0.0f)
                {
                    float game_time_prev = game_time;
                    game_time -= Time.deltaTime;
                    if (game_time > 7.0f)
                    {
                        over_score_text.text = ((int)(Random.value * 99999)).ToString();
                    }
                    else if (game_time_prev > 7.0f)
                    {
                        over_score_text.text = game_score.ToString();
                        audio_player.PlayOneShot(scoring_se[1]);
                    }
                    if (game_time < 5.0f && game_time_prev >= 5.0f)
                    {
                        string[] ranks = new string[5] { "C", "B", "A", "S", "SS" };
                        string current_rank = ranks[0];
                        for (int i = 1; i < 5; i++)
                        {
                            if (game_score < rank_borders[i])
                            {
                                audio_player.PlayOneShot(score_se[i - 1]);
                                break;
                            }
                            current_rank = ranks[i];
                        }
                        if (current_rank == ranks[4])
                            audio_player.PlayOneShot(score_se[4]);

                        over_score_level.gameObject.SetActive(true);
                        over_score_level.text = current_rank;
                    }
                }
                else
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
                    title_ui.SetActive(true);
                    tutorial_ui.SetActive(true);
                    Vector3 pos = title_object.spawn_pos.transform.position;
                    GameObject prefab = title_object.prefab;
                    title_object.obj = Instantiate(prefab, pos, Quaternion.identity);
                }
                break;

            case GameType.CHOSE_LEVEL:
                {
                    level_ui.SetActive(true);
                    for (int i = 0; i < (int)Level.MAX; i++)
                    {
                        Vector3 pos = level_object[i].spawn_pos.transform.position;
                        GameObject prefab = level_object[i].prefab;
                        level_object[i].obj = Instantiate(prefab, pos, Quaternion.identity);
                    }
                }
                break;

            case GameType.GAME_TUTORIAL:
                game_tutorial_ui.SetActive(true);
                break;

            case GameType.GAME:
                play_ui.SetActive(true);
                SetLevel((Level)chose_level);
                var manager = throw_manager.GetComponent<ThrowManager>();
                if (manager)
                {
                    manager.StopSpawn(false);
                }
                throw_manager.SetActive(true);
                game_time = play_time;
                game_score = 0;
                break;
            case GameType.END:
                game_time = 10.0f;
                over_ui.SetActive(true);
                over_score_level.gameObject.SetActive(false);
                audio_player.PlayOneShot(scoring_se[0]);
                break;
        }
        next_scene_timer = next_scene_time;
    }

    void Exit(GameType type)
    {
        switch (type)
        {
            case GameType.TITLE:
                title_ui.SetActive(false);
                tutorial_ui.SetActive(false);
                break;
            case GameType.CHOSE_LEVEL:
                level_ui.SetActive(false);
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
                game_tutorial_ui.SetActive(false);
                break;
            case GameType.GAME:
                play_ui.SetActive(false);
                throw_manager.SetActive(false);
                break;
            case GameType.END:
                over_ui.SetActive(false);
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
                var throw_manager_comp = throw_manager.GetComponent<ThrowManager>();
                throw_manager_comp.SetThrowInterval(cool_times[i]);
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

    public void AddScore(int score)
    {
        game_score += score;
        if (game_score < 0)
            game_score = 0;
    }

    public int GetScore()
    {
        return game_score;
    }
}
