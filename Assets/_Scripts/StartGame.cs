using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private float speed = 2f;

    void Update()
    {
        // Fade in - fade out
        float alpha = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        alpha = Mathf.Lerp(0.3f, 1f, alpha); // không mờ hẳn
        Color c = startText.color;
        c.a = alpha;
        startText.color = c;

        // Touch to Start
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Start Game!");
                int level = PlayerPrefs.GetInt("CurrentLevel", 1);
                SceneManager.LoadScene(level);
            }
        }
    }
}
