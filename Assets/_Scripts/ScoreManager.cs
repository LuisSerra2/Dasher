using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{

    public Observer<int> Score;
    public Observer<int> HighScore;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public bool resetScore;

    private void Start()
    {
        if (resetScore)
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();
        }

        Load();
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
        highScoreText.text = $"HighScore\n{HighScore.Value}";
    }

    public void Save()
    {
        PlayerPrefs.SetInt("HighScore", HighScore.Value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("HighScore"))
            HighScore.Value = PlayerPrefs.GetInt("HighScore");
    }
}
