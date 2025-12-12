using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject startPanel;
    public GameObject trackSelectPanel;
    public GameObject carSelectPanel;

    [Header("Track Select")]
    public Button track2Button;
    public GameObject track2LockIcon;

    void Start()
    {
        ShowMainPanel();

        // blokada Track 2
        bool unlocked = GameProgress.Track2Unlocked;
        track2Button.interactable = unlocked;
        track2LockIcon.SetActive(!unlocked);
    }

    // =========================
    // PANEL SWITCHING
    // =========================

    void DisableAllPanels()
    {
        mainPanel.SetActive(false);
        startPanel.SetActive(false);
        trackSelectPanel.SetActive(false);
        carSelectPanel.SetActive(false);
    }

    public void ShowMainPanel()
    {
        DisableAllPanels();
        mainPanel.SetActive(true);
    }

    public void ShowStartPanel()
    {
        DisableAllPanels();
        startPanel.SetActive(true);
    }

    public void ShowTrackSelectPanel()
    {
        DisableAllPanels();
        trackSelectPanel.SetActive(true);
    }

    public void ShowCarSelectPanel()
    {
        DisableAllPanels();
        carSelectPanel.SetActive(true);
    }

    // =========================
    // TRACK LOADING
    // =========================

    public void LoadTrack1()
    {
        SceneManager.LoadScene("Track1");
    }

    public void LoadTrack2()
    {
        if (GameProgress.Track2Unlocked)
            SceneManager.LoadScene("Track2");
    }

    // =========================
    // EXIT (na później)
    // =========================

    public void ExitGame()
    {
        Application.Quit();
    }
}