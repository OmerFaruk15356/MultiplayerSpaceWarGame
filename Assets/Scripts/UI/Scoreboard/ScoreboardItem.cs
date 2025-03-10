using Photon.Realtime;
using TMPro;
using UnityEngine;


public class ScoreboardItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI usernameText;
    [SerializeField] TextMeshProUGUI scoreText;
    private int score = 0;

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        scoreText.text = score.ToString(); 
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        scoreText.text = score.ToString();
    }
}
