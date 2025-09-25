using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI messageText;
    public Button startButton;
    public Button restartButton;

    [Header("Game Settings")]
    public float gameDuration = 30f;  // editable timer
    public int pointsToWin = 5;       // editable win score

    private float timeLeft;
    private bool isPlaying = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        timeLeft = gameDuration;
        timerText.text = "Time: " + Mathf.Ceil(timeLeft);
        messageText.text = "";
        restartButton.gameObject.SetActive(false);

        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (!isPlaying) return;

        timeLeft -= Time.deltaTime;
        if (timerText != null) timerText.text = "Time: " + Mathf.Ceil(timeLeft);

        if (timeLeft <= 0) EndGame(false);
    }

    public void StartGame()
    {
        isPlaying = true;
        startButton.gameObject.SetActive(false);
        messageText.text = "";
        ScoreManager.Instance?.ResetScore();
        timeLeft = gameDuration;
    }

    void EndGame(bool won)
    {
        isPlaying = false;
        messageText.text = won ? "You Win!" : "Time's Up!";
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CheckWinCondition(int score)
    {
        if (score >= pointsToWin) EndGame(true);
    }

    public bool IsPlaying() => isPlaying;
    public void PlayerMissed() { /* Optional */ }
}
