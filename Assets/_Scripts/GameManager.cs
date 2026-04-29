using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] long score;
    [SerializeField] int currentLevel;
    [SerializeField] int playerHealthPoint = 5;

    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(long amount)
    {
        score += amount;
        if (score > int.MaxValue) score = int.MaxValue;
        GameEvents.OnScoreChanged?.Invoke(score);
    }

    public void SetHealthPoint(int value)
    {
        playerHealthPoint = value;
        if (playerHealthPoint <= 0)
        {
            Gameover();
        }

        GameEvents.OnPlayerHealthPointChanged?.Invoke(playerHealthPoint);
    }

    public void Gameover()
    {
        StartCoroutine(LoseGameCoroutine());
        GameEvents.OnGameOver?.Invoke();
    }
    public void WinLevel()
    {
        StartCoroutine(WinCoroutine());
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        currentLevel += 1;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.Save();
        SceneManager.LoadScene(currentLevel);
    }

    private IEnumerator LoseGameCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(currentLevel);
    }
    public long GetScore() => score;
    public int GetLevel() => currentLevel;
    public int GetPlayerHealthPoint() => playerHealthPoint;
}

public static class GameEvents
{
    public static Action<long> OnScoreChanged;
    public static Action<int> OnLevelChanged;
    public static Action<int> OnPlayerHealthPointChanged;
    public static Action OnGameOver;
}