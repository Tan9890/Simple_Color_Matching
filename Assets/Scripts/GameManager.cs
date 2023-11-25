using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Ball_Mechanics _ball_mechanics;
    // Start is called before the first frame update
    void Start()
    {
        _ball_mechanics = GetComponent<Ball_Mechanics>();
        _ball_mechanics.SpawnBalls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
