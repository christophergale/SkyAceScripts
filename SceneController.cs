using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public static SceneController instance = null;

	// Use this for initialization
	void Awake () {
        #region instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        #endregion
    }
	
	// Update is called once per frame
	public void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        GameManager.instance.pickups = 0;
    }

    public void NewScene(int i)
    {
        SceneManager.LoadScene(i);
        GameManager.instance.pickups = 0;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
