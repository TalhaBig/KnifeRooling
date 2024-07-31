using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour 
{
    private static int score = 0;
    private static int highScore = 0;

	// Use this for initialization
	void Start () 
    {
        LoadHighScore();     
	}
	

    public static void SetScore(int newScore)
    {
        score += newScore;
        if (score > highScore)
            highScore = score;
    }

    public static void ResetScore()
    {
        score = 0;
    }

    public static int GetScore()
    {
        return score;
    }

    public static int GetHighScore()
    {
        return highScore;
    }

    public static void SaveHighScore()
    {
        if(score == highScore)
        {
            PlayerPrefs.SetInt("HighScore",highScore);
        }
    }

    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore",0);
    }

    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
