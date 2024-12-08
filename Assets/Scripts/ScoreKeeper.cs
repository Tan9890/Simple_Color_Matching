using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;
    public TMPro.TMP_Text scoreText;

    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateScore(int points)
    {
        instance.score += points;
        instance.scoreText.text = "Score: " + instance.score.ToString();
    }
}
