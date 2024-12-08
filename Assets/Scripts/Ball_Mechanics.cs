using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Mechanics : MonoBehaviour
{
    public static Ball_Mechanics instance;

    public GameObject Ball_Prefab, Board_Holder;
    public float Saturation, Value;
    public int GridSize = 8, maxMatches = 2;
    public float Ball_Spacing = 1f;
    public List<GameObject> _spawnedBalls;
    public List<Color> RandomColors, Colors;
    public bool testClick;


    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (testClick)
        {
            //RandomColorAssigner();
            foreach (GameObject g in _spawnedBalls)
                Destroy(g);
            SpawnBalls();
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
                tempBall.transform.localPosition = new Vector3((i * Ball_Spacing) - (GridSize / 2f) + 0.5f, 0, (j * Ball_Spacing) - (GridSize / 2f) + 0.5f);
                tempBall.GetComponentInChildren<MeshRenderer>().transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f)));
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
        for (int i = 0; i < (GridSize * GridSize) / 2; i++)
        {
            //randomColor = Random.ColorHSV(0,1f,1f,1f,0.5f,1f);
            randomColor = Colors[i];
            RandomColors.Add(randomColor);
            for (int j = 0; j < maxMatches; j++)
            {
                GameObject randomBall = FisherYatesShuffler();
                randomBall.GetComponentInChildren<MeshRenderer>().material.SetColor("_MainColor", randomColor);
            }
        }
    }

    void ColorsGenerator()
    {
        Colors = new List<Color>();
        int maxLength = (GridSize * GridSize) / maxMatches;
        for (int i = 0; i < maxLength; i++)
        {
            Colors.Add(Color.HSVToRGB((1f / maxLength) * i, Saturation, Value));
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
