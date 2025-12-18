using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //--ここでデータのみを管理する--

    //UIのランキング表示用スクリプト

    //級別: 初級のスコアのランキングを管理する配列
    [SerializeField] GameObject[] BeginnerRanking;
    //級別: 中級のスコアのランキングを管理する配列
    [SerializeField] GameObject[] IntermediateRanking;
    //級別: 高級のスコアのランキングを管理する配列
    [SerializeField] GameObject[] AdvancedRanking;



    //級別: 初級のスコアのランキング数値データを管理する配列
    //初級
    [SerializeField] int[] BeginnerScore = new int[MaxRankingNum];
    //中級
    [SerializeField] int[] IntermediateScore = new int[MaxRankingNum];
    //上級
    [SerializeField] int[] AdvancedScore = new int[MaxRankingNum];


    //enumで級別を管理
    public enum Level
    {
        BEGINNER = 0,
        INTERMEDIATE,
        ADVANCED,
        MAX,
    }



    //ランキングUIの表示・非表示を管理する変数
    public bool isRankingView = false;

    //ランキング表示数の最大値
    public const int MaxRankingNum = 5;

    //初級・中級・上級の各ランキングデータを外部に渡すためのgetter

    //初級
    public GameObject[] GetBeginnerRanking() { return BeginnerRanking; }
    //中級
    public GameObject[] GetIntermediateRanking() { return IntermediateRanking; }
    //高級
    public GameObject[] GetAdvancedRanking() { return AdvancedRanking; }

    //ランキング化したいスコアデータを受け取るsetter関数

    //初級
    public void SetBeginnerRanking(GameObject[] ranking_data) { BeginnerRanking = ranking_data; }
    //中級
    public void SetIntermediateRanking(GameObject[] ranking_data) { IntermediateRanking = ranking_data; }
    //高級
    public void SetAdvancedRanking(GameObject[] ranking_data) { AdvancedRanking = ranking_data; }



    //ランキング化したいスコア数値データを受け取るsetter関数
    //初級
    public void SetBeginnerScore(int[] score_data) { BeginnerScore = score_data; }
    //中級
    public void SetIntermediateScore(int[] score_data) { IntermediateScore = score_data; }
    //高級
    public void SetAdvancedScore(int[] score_data) { AdvancedScore = score_data; }

    //getter関数
    //初級
    public int[] GetBeginnerScore() { return BeginnerScore; }
    //中級
    public int[] GetIntermediateScore() { return IntermediateScore; }
    //高級
    public int[] GetAdvancedScore() { return AdvancedScore; }

    //ランキングの最大数を返すgetter関数
    public int GetMaxRankingNum() { return MaxRankingNum; }

    //ランキングデータ表示・非表示切り替え関数
    public void ToggleRankingView()
    {

        //ここで各ランキングUIの表示・非表示を切り替える
        //初級

        //ここのfor文はMaxRankingNum回繰り返す(ランキングは定数分のみ表示する)
        for (int i = 0; i < MaxRankingNum; i++)
        {
            DisplayBeginnerScore();
            BeginnerRanking[i].SetActive(isRankingView);
        }
        //中級
        for (int i = 0; i < MaxRankingNum; i++)
        {
            DisplayIntermediateScore();
            IntermediateRanking[i].SetActive(isRankingView);
        }
        //高級
        for (int i = 0; i < MaxRankingNum; i++)
        {
            DisplayAdvancedScore();
            AdvancedRanking[i].SetActive(isRankingView);
        }



    }
    //ランキングのひ非表示にする関数
    public void HideRankingView()
    {
        //ここで各ランキングUIの非表示を行う
        //初級
        for (int i = 0; i < MaxRankingNum; i++)
        {
            BeginnerRanking[i].SetActive(false);
        }
        //中級
        for (int i = 0; i < MaxRankingNum; i++)
        {
            IntermediateRanking[i].SetActive(false);
        }
        //高級
        for (int i = 0; i < MaxRankingNum; i++)
        {
            AdvancedRanking[i].SetActive(false);
        }
    }

    //級別のenumをint型に変換する関数
    //ここは使うか不明
    public int LevelToInt(Level level)
    {
        return (int)level;
    }
    //階級別のスコアを表示する関数
    //初級
     void DisplayBeginnerScore()
    {
        for (int i = 0; i < MaxRankingNum; i++)
        {
            BeginnerRanking[i].GetComponent<UnityEngine.UI.Text>().text = (i + 1).ToString() + "位: " + BeginnerScore[i].ToString() + "点";
        }
    }
    //中級
     void DisplayIntermediateScore()
    {
        for (int i = 0; i < MaxRankingNum; i++)
        {
            IntermediateRanking[i].GetComponent<UnityEngine.UI.Text>().text = (i + 1).ToString() + "位: " + IntermediateScore[i].ToString() + "点";
        }
    }
    //高級
     void DisplayAdvancedScore()
    {
        for (int i = 0; i < MaxRankingNum; i++)
        {
            AdvancedRanking[i].GetComponent<UnityEngine.UI.Text>().text = (i + 1).ToString() + "位: " + AdvancedScore[i].ToString() + "点";
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
