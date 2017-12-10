using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI of the game
/// </summary>
public class UIManager : MonoBehaviour 
{
    public Text scoreText;
	public Text timeText;
    public static UIManager instance;
	
	
    void Start()
    {
        instance = this;

        // subscribes to the event of the score change on the game Manager, to update the score on the screen
        GameManager.instance.ScoreChanged += UpdateScoreText;
    }

	void Update () 
    {
        UpdateTimeText();
	}

    /// <summary>
    /// Updates the time text.
    /// </summary>
    void UpdateTimeText()
    {
       timeText.text = "Time: " + Mathf.Round(GameManager.instance.currentTime); 
    }

    /// <summary>
    /// Updates the Score text.
    /// </summary>
    /// <param name="aNewScore">The current score</param>
    void UpdateScoreText(int aNewScore)
    {
        scoreText.text = "Score: " + aNewScore;

    }
}
