using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] Canvas PauseCanvas;
    [SerializeField] int instanceHighScoreMax;
    [SerializeField] float timeScoreWorth;
    public static float score;
    public static GameObject player;
    public static GameState currentGameState;
    [SerializeField] private List<HighScore> HighScoreList;
    public struct HighScore
    {
        public string name;
        public int score;
        public HighScore(string newName, int newScore)
        {
            name = newName;
            score = newScore;
        }
    }
    public enum GameState
    {
        Menu,
        Pause,
        Game
    }
    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned");
        }
        score = 0;
    }

    void Update()
    {
        //ScoreChange(Time.deltaTime * timeScoreWorth);
        TestingStuff();
    }
    void ScoreChange(float change)
    {
        score += change;
        highScoreText.enabled = NewScore(0) ? true : false;
        scoreText.text = $"Score: {(int)score}";
    }
    void PauseGame()//defunct?
    {
        currentGameState = GameState.Pause;
    }
    void ResumeGame()//defunct?
    {
        currentGameState = GameState.Game;
        PauseCanvas.enabled = false;
    }
    void ClearHighScore()
    {
        HighScoreList.Clear();
    }
    void NewHighScore(string name)
    {
        HighScoreList.Add(new HighScore(name, (int)score));
        HighScoreList.OrderBy(x => x.score);
        if (HighScoreList.Count() > instanceHighScoreMax) HighScoreList.RemoveAt(instanceHighScoreMax);
    }
    bool NewScore(int position)
    {
        return score > HighScoreList[position].score ? true : false;
    }
    void LoadScores()
    {

    }
    void SaveScores()
    {
        string jsonFile = JsonUtility.ToJson(HighScoreList);
        Debug.Log(jsonFile);
    }
    void TestingStuff()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Scrores Cleared");
            ClearHighScore();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Added Tim's score");
            NewHighScore("Tim");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Saves Scores");
            SaveScores();
        }
    }
}
