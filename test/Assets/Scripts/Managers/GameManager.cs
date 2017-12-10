using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the main states of the game 
/// </summary>
public class GameManager : MonoBehaviour 
{
    public static GameManager instance;
    [HideInInspector] public float currentTime = 0f;
    [HideInInspector] public int score;

    /// <summary>
    /// Defines the delay before restarting the game
    /// </summary>
    public float gameoverDelay=2f;

    /// <summary>
    /// Score change event.
    /// </summary>
    public delegate void ScoreChangedEvent(int amount);
    public ScoreChangedEvent ScoreChanged;

    public bool gameover = false;
	// Use this for initialization
	void Awake () 
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateTime();
	}

    /// <summary>
    /// Updates the time if the game is not over.
    /// </summary>
    void UpdateTime()
    {
		if (gameover == false)
		{
			currentTime += Time.deltaTime;
		}
    }

    /// <summary>
    /// Sets the restart of game. Flags that the game is over
    /// </summary>
    public void  SetRestartOfGame(){
        gameover = true;
        StartCoroutine(RestartGame());
    }

    /// <summary>
    /// Restarts the game after the delay specified.
    /// </summary>
    /// <returns>The game.</returns>
    IEnumerator RestartGame(){

        yield return new WaitForSeconds(gameoverDelay);
		SceneManager.LoadScene("TestScene");

    }

    /// <summary>
    /// Increments the score and fires the Score Change event
    /// </summary>
    public void IncrementScore()
    {
        score++;
        if(ScoreChanged!=null){

            ScoreChanged(score);
        }
    }
}
