using UnityEditor;
using UnityEngine;

public class ScoreRanking : MonoBehaviour
{
    GameObject ScoreManager;
    private ScoreManager RankingScript;

    //ランキング表示数の最大値
    private int max_ranking_num;

    //級毎のenum
    public enum Level
    {
        BEGINNER = 0,
        INTERMEDIATE,
        ADVANCED,
        MAX,
    }
    Level test_Level;


    //RankingScriptを取得する関数
    void ManargerGet()
    {
        //ScoreManagerタグを持つオブジェクトを取得し、RankingScriptにScoreManagerスクリプトを代入する
        ScoreManager = GameObject.FindWithTag("ScoreManager");
        //もし、このオブジェクトが存在しなければゲームコントローラータグを持つオブジェクトを取得
        if (ScoreManager == null)
        {
            ScoreManager = GameObject.FindWithTag("GameController");
        }


        //ScoreManagerスクリプトを取得
        RankingScript = ScoreManager.GetComponent<ScoreManager>();
        max_ranking_num = RankingScript.GetMaxRankingNum();
        //この時点で階級のenumはScoreManagerのものを使用する
       

    }


    //ランキングUIの表示・非表示を切り替える関数
    void ToggleRankingView()
    {

        //RankingScriptがnullの場合、ManargerGet関数を呼び出して取得する
        if (RankingScript == null)
        {
            ManargerGet();
        }
        //もし、RankingScriptが取得できていれば
        if (RankingScript != null)
        {
            //ランキングUIの表示・非表示を切り替える
            RankingScript.isRankingView = !RankingScript.isRankingView;
            //現状ランキングはすべて同時に表示するそうで、個別の表示・非表示は行わない
            //そのため表示非表示を切り替えたっ瞬間に、すべてのランキングUIの表示・非表示を切り替える
            //関数を呼び出す
            RankingScript.ToggleRankingView();

        }


    }
    //試運転でランダムでスコアを取得する関数
    int GetRandomScore()
    {
        //0から1000までのランダムな整数を返す
        return Random.Range(0, 1001);
    }
    //取得した値と級をランキングデータを比較して、ランキングに入っているかどうかを判定する関数
    void CheckAndSetRanking(int score, Level level)
    {
        //もし対応した級のランキング上位5位以内に入っていれば、ランキングデータを更新する
        switch (level)
        {
            //階級別で場合分け
            //初級    
            case Level.BEGINNER:

                {
            int[] beginner_scores = RankingScript.GetBeginnerScore();
                    for (int i = 0; i < max_ranking_num; i++)
                    {
                        Debug.Log("レベルの引数が渡されていません" + beginner_scores[i]);
                        if (score > beginner_scores[i])
                        {
                            //ランキングに入っている場合、ランキングデータを更新する
                            beginner_scores[i] = score;
                            RankingScript.SetBeginnerScore(beginner_scores);
                            break;
                        }
                    }
                    break;
                }
            case Level.INTERMEDIATE:
                {
                    int[] intermediate_scores = RankingScript.GetIntermediateScore();
                    for (int i = 0; i < max_ranking_num; i++)
                    {
                        if (score > intermediate_scores[i])
                        {
                            //ランキングに入っている場合、ランキングデータを更新する
                            intermediate_scores[i] = score;
                            RankingScript.SetIntermediateScore(intermediate_scores);
                            break;
                        }
                    }
                    break;
                }
            case Level.ADVANCED:
                {
                    int[] advanced_scores = RankingScript.GetAdvancedScore();
                    for (int i = 0; i < max_ranking_num; i++)
                    {
                        if (score > advanced_scores[i])
                        {
                            //ランキングに入っている場合、ランキングデータを更新する
                            advanced_scores[i] = score;
                            RankingScript.SetAdvancedScore(advanced_scores);
                            break;
                        }
                    }
                    break;
                }
        }


    }
    //試運転での呼び出されたら、ランダムでスコアを取得し、ランキングに反映させる関数
    public void TestSetRandomScoreToRanking(Level level)
    {
        int random_score = GetRandomScore();
        CheckAndSetRanking(random_score, level);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //キーボードのRキーが押されたらランキングUIの表示・非表示を切り替える   
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRankingView();
        }
        //キーボードのTキーが押されたら試運転でランダムでスコアを取得し、ランキングに反映させる
        if (Input.GetKeyDown(KeyCode.T))
        {
      //試運転でランダムでスコアを取得し、ランキングに反映させる
            TestSetRandomScoreToRanking(0);

        }

    }
}
