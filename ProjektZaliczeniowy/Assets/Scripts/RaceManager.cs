using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("Race Settings")]
    public int lapsToComplete = 3;
    public int checkpointCount = 3; // liczba CP na torze (bez startu)

    [Header("UI")]
    public TMP_Text timeText;
    public TMP_Text bestText;
    public TMP_Text lapText;

    private float currentLapTime = 0f;
    private float bestTime = 9999f;

    private bool raceFinished = false;
    private int currentLap = 1;
    private int nextCheckpoint = 1;
    private bool raceStarted = false;

    public Vector3 lastCheckpointPosition;
    public Transform[] checkpoints;
    public Quaternion lastCheckpointRotation;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        if (Instance == null) Instance = this;

        bestTime = PlayerPrefs.GetFloat("BestTime_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 9999f);
        UpdateBestUI();
    }

    private void Update()
    {
        if (raceStarted)
        {
            currentLapTime += Time.deltaTime;
            UpdateTimeUI();
        }
    }

    public void PlayerHitCheckpoint(int index)
    {
        if (raceFinished) return;
        if (!raceStarted && index == 0)
        {
            StartLap();
            lastCheckpointPosition = checkpoints[index].position; // start
            lastCheckpointRotation = checkpoints[index].rotation;
            return;
        }

        if (index == nextCheckpoint)
        {
            lastCheckpointPosition = checkpoints[index].position; // zapamiętaj CP
            lastCheckpointRotation = checkpoints[index].rotation;
            nextCheckpoint++;

            if (nextCheckpoint > checkpointCount)
            {
                LapCompleted();
                nextCheckpoint = 1;
            }
        }
    }

    private void StartLap()
    {
        raceStarted = true;
        currentLapTime = 0f;
        UpdateTimeUI();
    }

    private void LapCompleted()
    {
        raceStarted = false;

        if (currentLapTime < bestTime)
        {
            bestTime = currentLapTime;
            PlayerPrefs.SetFloat(
                "BestTime_" + SceneManager.GetActiveScene().name,
                bestTime
            );
            UpdateBestUI();
        }

        // ===== KONIEC WYŚCIGU =====
        if (currentLap >= lapsToComplete)
        {
             raceFinished = true;
             raceStarted = false;

             var car = FindObjectOfType<CarController>();
             if (car != null)
                 car.enabled = false;

            // osiągnięcia
            GameProgress.Track1Finished = true;

            if (bestTime < 60f)
                GameProgress.Track1FastTime = true;

            GameProgress.CheckUnlocks();

            Debug.Log("Race Finished!");

            // wróć do menu po 2 sekundach
            Invoke(nameof(ReturnToMainMenu), 2f);
            return;
        }

        // kolejne okrążenie
        currentLap++;
        lapText.text = "Lap: " + currentLap + "/" + lapsToComplete;
    }


    private void UpdateTimeUI()
    {
        timeText.text = " Time:  " + currentLapTime.ToString("F3");
    }

    private void UpdateBestUI()
    {
        bestText.text = " Best: " + bestTime.ToString("F3");
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
