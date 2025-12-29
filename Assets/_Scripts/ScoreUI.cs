using System.Collections;
using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private long currentScore;
    private long targetScore;
    private Coroutine scoreCoroutine;

    private void OnEnable()
    {
        GameEvents.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreChanged -= UpdateScore;
    }

    private void Start()
    {
        currentScore = GameManager.Instance.GetScore();
        targetScore = currentScore;
        scoreText.text = currentScore.ToString();
    }

    private void UpdateScore(long newScore)
    {
        targetScore = newScore;

        if (scoreCoroutine != null)
            StopCoroutine(scoreCoroutine);

        scoreCoroutine = StartCoroutine(UpdateScoreCoroutine());
    }

    private IEnumerator UpdateScoreCoroutine()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        long startScore = currentScore;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            currentScore = (long)Mathf.Lerp(startScore, targetScore, t);
            scoreText.text = currentScore.ToString();
            yield return null;
        }

        currentScore = targetScore;
        scoreText.text = currentScore.ToString();
    }
}