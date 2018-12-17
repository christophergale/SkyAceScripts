using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Vector3 spawnPoint;

    // Values ////////////////////////
    public float score = 200;
    [HideInInspector] public float quickTime;
    private float orbitMultiplier;
    public int pickups = 0;

    // UI ////////////////////////
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject mainCanvas;
    public Text pickupsText;
    public Slider backgroundOpacity;
    public Text questLog;

    // Bools ////////////////////////
    public bool explorerAchievement = false;
    // public bool speedAchievement = true;
    public bool exterminatorAchievement = false;
    public bool gameOver;
    public bool gamePaused = false;

    // Planet array ////////////////////////
    public LevelInfo levelInfo;
    public int planetCount;
    public Planet[] visited;

    // Itinerary ////////////////////////
    public Quest[] quests;
    public bool allQuestsComplete = false;
    public Quest activeQuest;

    // public GameObject blackHole;
    // public GameObject compass;

    private void Awake()
    {
        #region instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        #endregion

        GetLevelInfo();
    }

    private void Start()
    {
        score = 200;
        GetLevelInfo();

        PopulateQuestLog();
    }

    // Update is called once per frame
    void Update () {
        //quickTime = Time.timeSinceLevelLoad;
        //if (quickTime >= levelInfo.quickTime)
        //{
        //    speedAchievement = false;
        //}

        // RESET:
        if (Input.GetKeyDown("r"))
        {
            RestartFunction();
        }

        if (Input.GetKeyDown("n"))
        {
            SceneController.instance.NewScene(2);
        }

        // PAUSE:
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            PauseUnpause();
        }

        if (Ball.instance.visitedIndex >= visited.Length)
        {
            explorerAchievement = true;
        }

        if (Ball.instance != null)
            gameOver = !Ball.instance.gameObject.activeInHierarchy;

        pauseMenu.SetActive(gamePaused);
        gameOverMenu.SetActive(gameOver);

        if (Ball.instance != null)
            mainCanvas.SetActive(Ball.instance.gameObject.activeInHierarchy);

        Cursor.visible = gamePaused || gameOver;

        pickupsText.text = pickups.ToString();

        // blackHole.SetActive(allQuestsComplete);
        // compass.SetActive(allQuestsComplete);

        if (allQuestsComplete)
        {
            // compass.transform.position = Ball.instance.transform.position + Vector3.Normalize(blackHole.transform.position - Ball.instance.transform.position) * 8f;
        }
    }

    void GetLevelInfo()
    {
        quickTime = 0f;
        planetCount = levelInfo.planetCount;
        visited = new Planet[planetCount];
    }

    void PopulateQuestLog()
    {
        questLog.text = null;

        for (int i = 0; i < quests.Length; i++)
        {
            if (!quests[i].questComplete)
            {
                questLog.text += quests[i].gameObject.GetComponent<Planet>().planetStats.name + " : " + quests[i].questLogText + "\n";
            }
        }
    }

    public void PauseUnpause()
    {
        PopulateQuestLog();
        gamePaused = !gamePaused;
    }

    public void RestartFunction()
    {
        StartCoroutine(Restart());
    }

    public IEnumerator Restart()
    {
        Ball.instance.GetComponent<TrailRenderer>().enabled = false;
        gameOver = false;
        gamePaused = false;
        Ball.instance.gameObject.SetActive(true);
        Ball.instance.transform.position = spawnPoint;
        AudioManager.instance.respawn.Play();
        Ball.instance.rb.velocity = new Vector3(10f, 0f, 0f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<EnemyReset>() != null)
            {
                enemies[i].GetComponent<EnemyReset>().ResetEnemy();
            }
        }
        BackgroundScroll.instance.ResetBackground();
        yield return new WaitForSeconds(Ball.instance.GetComponent<TrailRenderer>().time);
        Ball.instance.GetComponent<TrailRenderer>().enabled = true;
    }

    public void CheckQuests()
    {
        bool questIncomplete = false;

        for (int i = 0; i < quests.Length; i++)
        {
            if (!quests[i].questComplete)
            {
                questIncomplete = true;
                break;
            }
        }

        allQuestsComplete = !questIncomplete;
    }
}
