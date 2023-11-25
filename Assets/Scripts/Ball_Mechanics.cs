using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Mechanics : MonoBehaviour
{
    public GameObject Ball_Prefab, Board_Holder;
    public int GridSize = 8, maxMatches = 2;
    public float Ball_Spacing = 1f;
    public List<GameObject> _spawnedBalls;
    public List<Color> RandomColors, Colors;
    public bool testClick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (testClick)
        {
            RandomColorAssigner();
            testClick = false;
        }
    }

    public void SpawnBalls()
    {
        _spawnedBalls = new List<GameObject>();
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                GameObject tempBall = Instantiate(Ball_Prefab, Board_Holder.transform);
                tempBall.transform.localPosition = new Vector3((i * Ball_Spacing) - (GridSize/2f) + 0.5f, 0, (j * Ball_Spacing) - (GridSize / 2f) + 0.5f);
                _spawnedBalls.Add(tempBall);
            }
        }
        ColorsGenerator();
        RandomColorAssigner();
    }

    void RandomColorAssigner()
    {
        Color randomColor;
        RandomColors = new List<Color>();
        for (int i = 0; i < (GridSize*GridSize) / 2; i++)
        {
            //randomColor = Random.ColorHSV(0,1f,1f,1f,0.5f,1f);
            randomColor = Colors[i];
            RandomColors.Add(randomColor);
            for (int j = 0; j < maxMatches; j++)
            {
                GameObject randomBall = FisherYatesShuffler();
                randomBall.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", randomColor);
            }
        }
    }

    void ColorsGenerator()
    {
        Colors = new List<Color>();
        int maxLength = (GridSize * GridSize) / 2;
        for (int i = 0; i < maxLength; i++)
        {
            Colors.Add(Color.HSVToRGB((1f / maxLength) * i, 1f, 1f));
        }
    }

    GameObject FisherYatesShuffler()
    {
        int _randomIndex = Random.Range(0, _spawnedBalls.Count);
        GameObject tempRandomBall = _spawnedBalls[_randomIndex];
        _spawnedBalls.RemoveAt(_randomIndex);
        return tempRandomBall;    
    }
}
