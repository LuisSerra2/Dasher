using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>, IDataPersistence
{
    public Observer<int> Score;
    public Observer<int> HighScore;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;


    private void Start()
    {
        UpdateScore(0);
    }

    public void UpdateScore(int score)
    {
        Score.Value += score;
        if (Score.Value > HighScore.Value)
        {
            HighScore.Value = Score.Value;
        }
        if (scoreText == null || highScoreText == null ) return;
        scoreText.text = $"Score: {Score.Value}";
        highScoreText.text = $"HighScore: {HighScore.Value}";
    }

    public void LoadData(GameData data)
    {
        HighScore.Value = data.HighScore;
        highScoreText.text = $"HighScore: {HighScore.Value}";
    }

    public void SaveData(ref GameData data)
    {
        data.HighScore = HighScore.Value;
    }

    public string GetUniqueIdentifier()
    {
        return this.gameObject.name + "_" + this.gameObject.GetInstanceID();
    }
}
