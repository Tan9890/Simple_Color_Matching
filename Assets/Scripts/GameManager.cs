using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Ball_Mechanics _ball_mechanics;

    public static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        _ball_mechanics = GetComponent<Ball_Mechanics>();
        _ball_mechanics.SpawnBalls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
