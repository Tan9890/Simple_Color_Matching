using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager instance;
    public GameObject lifePrefab;
    public int maxLives = 10, currentLives = 3;
    public List<GameObject> lifeList;
    
    public bool AddL, RemL;
    public float animSpeed, initAnimMul, endAnimMul, maxPopVal;
    public AnimationCurve animcurve;


    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        StartCoroutine(RegisterEvent());
        InitializeLives();
    }

    // Update is called once per frame
    void Update()
    {
        if (AddL)
        {
            AddLife();
            AddL = false;
        }
        if (RemL)
        {
            RemoveLife();
            RemL = false;
        }
    }

    IEnumerator RegisterEvent()
    {
        while (Interaction_behav.onMisMatch == null)
        {
            Debug.Log("isnull");
            yield return null;
        }
        Interaction_behav.onMisMatch.AddListener(RemoveLife);
    }

    public static void InitializeLives()
    {
        instance.lifeList = new List<GameObject>();
        for (int i = 0; i < instance.maxLives; i++)
        {
            GameObject life = Instantiate(instance.lifePrefab, instance.transform);
            instance.lifeList.Add(life);
            life.SetActive(false);
        }
        instance.InitializeLevel();
    }

    void InitializeLevel()
    {
        for (int i = 0; i < currentLives; i++)
        {
            lifeList[i].SetActive(true);
        }
    }

    public void AddLife()
    {
        if (instance.lifeList.Count > 0)
        {
            for (int i = 0; i < instance.lifeList.Count; i++)
            {
                if (!instance.lifeList[i].activeInHierarchy)
                {
                    //instance.lifeList[i].SetActive(true);
                    StartCoroutine(AnimateLife(instance.lifeList[i], 0, maxPopVal, 1f, true));
                    instance.currentLives++;
                    return;
                }
            }
        }
    }

    public void RemoveLife()
    {
        if (instance.lifeList.Count > 0)
        {
            for (int i = instance.lifeList.Count - 1; i >= 0; i--)
            {
                if (instance.lifeList[i].activeInHierarchy)
                {
                    //instance.lifeList[i].SetActive(false);
                    StartCoroutine(AnimateLife(instance.lifeList[i], 1f, maxPopVal, 0, false));
                    instance.currentLives--;
                    instance.CheckGameOver();
                    return;
                }
            }
        }
    }

    IEnumerator AnimateLife(GameObject _life, float initVal, float maxVal, float finalVal, bool isActive)
    {
        if (isActive)
            _life.SetActive(true);
        float k = 0;
        while (k < 1f)
        {
            AnimationOps(_life, k, initVal, maxVal);
            k += animSpeed * initAnimMul;
            yield return null;
        }
        k = 1f;
        while (k > 0)
        {
            AnimationOps(_life, k, finalVal, maxVal);
            k -= animSpeed * endAnimMul;
            yield return null;
        }
        k = 0;
        AnimationOps(_life, k, finalVal, maxVal);
        if (!isActive)
            _life.SetActive(false);

    }
    void AnimationOps(GameObject g, float k, float min, float max)
    {
        g.transform.localScale = Vector3.one * Utils.Map(animcurve.Evaluate(k), min, max);
    }

    void CheckGameOver()
    {
        if (currentLives <= 0)
        {
            Debug.Log("GameOver");
        }
    }
}
