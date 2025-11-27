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
    [SerializeField] GameObject[] level = new GameObject[3];
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
        if(Input.GetKeyDown(KeyCode.Return))
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
                LevelUI.SetActive(true);
                break;

            case GameType.GAME_TUTORIAL:
                GameTutorialUI.SetActive(true);
                break;

            case GameType.GAME:
                PlayUI.SetActive(true);
                break;

            case GameType.END:
                break;
        }
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
                level[i].SetActive(true);
            }
            else
            {
                level[i].SetActive(false);
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
