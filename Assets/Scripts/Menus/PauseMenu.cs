using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public bool isPause = false;
    public static PauseMenu instance;
    private AudioManager audioManager;
    private Audio lastAu;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start() { Time.timeScale = 1f; audioManager = FindObjectOfType<AudioManager>(); }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
                Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPause = true;
        Time.timeScale = 0f;
        menu.SetActive(true);
        lastAu = audioManager.mainAudio;
        audioManager.SetToMain("PauseMenu", false);
    }
    public void Resume()
    {
        isPause = false;
        Time.timeScale = 1f;
        menu.SetActive(false);
        audioManager.SetToMain(lastAu.name, false);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
