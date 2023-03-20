using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;


    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject startMenuCanvas;
    [SerializeField] private GameObject playerUICanvas;
    [SerializeField] private Renderer background;
    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private TextMeshProUGUI livesLabel;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    
    public bool gameOver = false;
    public bool start = false;

    [SerializeField] GameObject playerPrefab;
    private Player player;
    private int score = 0;
    private int STARTING_LIVES = 2;
    private int lives = 0;

    private void Awake() {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(this.gameObject); }else { Destroy(this.gameObject); }
    }

 

    // Update is called once per frame
    void Update()
    {
        if (background) {
            background.material.mainTextureOffset += new Vector2(0.015f, 0) * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.X) && (gameOver || !start)) {
            LoadOrRestart();
        }

    }

    public void SpawnPlayer() {
        player = Instantiate(playerPrefab, new Vector3(-3, -3, 0), Quaternion.identity).GetComponent<Player>();
        player.OnPlayerDamaged += UpdateStatus;
        DontDestroyOnLoad(player);
    }

    public void LoadOrRestart() {
        Debug.Log("Load or Restart.");
        playerUICanvas.SetActive(true);
        if (start == false) {
            start = true;
            startMenuCanvas.SetActive(false);
            SceneManager.LoadScene("Level00", LoadSceneMode.Single);
            SpawnPlayer();
            lives = STARTING_LIVES;
            score = 0;
            //healthLabel.text = "Health: " + new string('♥',player.HEALTH);
        } else {
            if (gameOver) {                
                gameOverCanvas.SetActive(false);
                gameOver = false;
                SceneManager.LoadScene("Level00", LoadSceneMode.Single);
                SpawnPlayer();
                lives = STARTING_LIVES;
                score = 0;
                //healthLabel.text = "Health: " + new string('♥', player.HEALTH);
            }
        }

        UpdateStatus();

    }

    public void UpdateStatus() {
        healthLabel.text = "Health: " + new string('♥', player.HEALTH);
        livesLabel.text = "Lives: " + new string('l', lives);
        scoreLabel.text = $"Score: {score}";        
        if (player.HEALTH <= 0) {
            Debug.Log("Respawning player");
            StartCoroutine(Respawn());
        }        
    }

    public void GameOver(int score) {
        gameOverCanvas.SetActive(true);
        gameOver = true;
    }

    private IEnumerator Respawn() {
        lives= lives>0?lives-1:0;
        Debug.Log("Respawning. . .");
        yield return new WaitForSeconds(2.5f);
        if (lives > 0) { SpawnPlayer(); UpdateStatus(); } else {
            livesLabel.text = "Lives: x";
            GameOver(score);
        }
    }

    public void AddScore(int val) {
        score += val;
        scoreLabel.text = $"Score: {score}";
        Debug.Log("Current Score is: " + score);
    }

}
