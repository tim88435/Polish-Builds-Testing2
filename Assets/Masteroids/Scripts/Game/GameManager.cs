using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using UnityEditor;
using System;
using System.Text;

public class GameManager : MonoBehaviour
{
    public AsteroidManager asteroidManager;
    public Text scoreText;
    public Text highScoreHUDText;
    public Gradient highScoreTextGradient;
    public float highScoreTextGradientValue;
    public GameObject PauseCanvas;
    public int instanceHighScoreMax;
    public float timeScoreWorth;
    public static float score;
    public static float gameTime;
    public GameObject playerprefab;
    public static PlayerMovement player;
    public static GameState currentGameState;
    public List<HighScore> HighScoreList = new List<HighScore>();
    public Text highScoreScoreText;
    public Text highScoreNameText;
    public GameObject NewScoreObject;
    [System.Serializable]
    public struct HighScore
    {
        [SerializeField] public string name;
        [SerializeField] public int score;
        
        [SerializeField] public HighScore(string newName, int newScore)
        {
            name = newName;
            score = newScore;
        }
    }
    public enum GameState
    {
        Menu,
        Pause,
        Game,
        EndGame
    }
    void Start()
    {
        Cursor.visible = false;
        if (playerprefab == null)
        {
            Debug.LogWarning("Player Prefab not assigned!");
        }
        else
        {
            player = Instantiate(playerprefab).GetComponent<PlayerMovement>();
        }
        if (File.Exists($"{Application.dataPath}/HighScores.txt"))
        {
            LoadScores();
        }
        score = 0;
        gameTime = 0;
        currentGameState = GameState.Game;
        timeScoreWorth = 1;
    }
    void Update()
    {
        PauseMenu();
        if (currentGameState == GameState.Game)
        {
            gameTime += Time.deltaTime;
            if (gameTime > 12)
            {
                timeScoreWorth = 4 + (float)asteroidManager.difficulty;
            }
            ScoreChange(Time.deltaTime * timeScoreWorth);
        }
        if (currentGameState == GameState.Pause && PauseCanvas.activeInHierarchy)
        {
            UpdateText(HighScoreList);
        }
        else if (currentGameState == GameState.EndGame)
        {
            currentGameState = GameState.Pause;
            if (CheckIfNewHighScore(instanceHighScoreMax-1) || HighScoreList.Count < instanceHighScoreMax)
            {
                NewScoreObject.SetActive(true);
            }
            else
            {
                ChangeScene(0);
            }
        }
        if (CheckIfNewHighScore(0))
        {
            highScoreHUDText.enabled =true;
            FlashText(highScoreHUDText);
            FlashText(scoreText);
        }
        else
        {
            highScoreHUDText.enabled = false;
            scoreText.color = Color.cyan;
        }
    }
    public void ScoreChange(float change)
    {
        score += change;
        scoreText.text = $"Score: {(int)score}";
    }
    public void ResumeGame()
    {
        currentGameState = GameState.Game;
        PauseCanvas.SetActive(false);
    }
    public void ChangeScene(int scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
    public void ClearHighScores()
    {
        HighScoreList.Clear();
    }
    public void NewHighScore(string name)
    {
        HighScoreList.Add(new HighScore(name, (int)score));
        HighScoreList = HighScoreList.OrderBy(x => x.score).ToList();
        HighScoreList.Reverse();
        if (HighScoreList.Count() > instanceHighScoreMax) HighScoreList.RemoveAt(instanceHighScoreMax);
        SaveScores();
    }
    public void NewHighScore(string name, int localScore)
    {
        HighScoreList.Add(new HighScore(name, (int)localScore));
        HighScoreList = HighScoreList.OrderBy(x => x.score).ToList();
        HighScoreList.Reverse();
        if (HighScoreList.Count() > instanceHighScoreMax) HighScoreList.RemoveAt(instanceHighScoreMax);
        SaveScores();
    }
    bool CheckIfNewHighScore(int position)
    {
        if (HighScoreList.Count == 0)
        {
            return true;
        }
        else if (HighScoreList.Count-1 < position)
        {
            return score > HighScoreList[HighScoreList.Count-1].score ? true : false;
        }
        return score > HighScoreList[position].score ? true : false;
    }
    public void LoadScores()
    {
        HighScoreList = JsonUtility.FromJson<ScoreSave>(Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText($"{Application.dataPath}/HighScores.txt")))).SavedScoreList;
    }
    public void SaveScores()
    {
        ScoreSave scoreSave = new ScoreSave();
        scoreSave.SavedScoreList = HighScoreList;
        File.WriteAllText($"{Application.dataPath}/HighScores.txt", Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(scoreSave))));
    }
    void PauseMenu()
    {
        if (Input.GetKey(KeyCode.Escape) || Input.GetButton("P2 Start"))
        {
            QuitGame();
        }
        if (Input.GetButtonDown("P1 Start") || Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentGameState)
            {
                case GameState.Pause:
                    currentGameState = GameState.Game;
                    PauseCanvas.SetActive(false);
                    break;
                case GameState.Game:
                    currentGameState = GameState.Pause;
                    PauseCanvas.SetActive(true);
                    break;
                default:
                    PauseCanvas.SetActive(false);
                    break;
            }
        }
    }
    private void UpdateText(List<HighScore> scores)
    {
        string nametext = "";
        string scoretext = "";
        foreach (HighScore score in scores)
        {
            nametext = $"{nametext}\n{score.name}";
            scoretext = $"{scoretext}\n{score.score}";
        }
        highScoreScoreText.text = scoretext;
        highScoreNameText.text = nametext;
    }
    private void FlashText(Text text)
    {
        highScoreTextGradientValue += Time.deltaTime;
        if (highScoreTextGradientValue >= 1)
        {
            highScoreTextGradientValue = 0;
        }
        text.color = highScoreTextGradient.Evaluate(highScoreTextGradientValue);
    }
}
