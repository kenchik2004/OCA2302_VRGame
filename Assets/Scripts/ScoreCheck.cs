using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCheck : MonoBehaviour
{
    [SerializeField] GameObject score_effect;
    TextMeshPro score_effect_text;
    [SerializeField] Text score_text;
    [SerializeField] int score = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score_effect_text = score_effect.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 当たった座標にエフェクトを生成
        Instantiate(score_effect, collision.contacts[0].point, Quaternion.identity);
        score_effect_text.text = "+" + score.ToString();
        if (score_text)
        {
            // スコアを加算
            int current_score = int.Parse(score_text.text);
            current_score += score;
            score_text.text = current_score.ToString();
        }
    }
}

