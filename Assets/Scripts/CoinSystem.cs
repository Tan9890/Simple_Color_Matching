using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public static CoinSystem instance;

    public GameObject coinPrefab, coinHolder, scoreTrigger;
    public int maxCoins;

    List<GameObject> coins;

    public bool testCoin = false;

    int coinValue = 10;
    public int CoinValue { get { return coinValue; } }

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        InitializeCoins();
    }

    // Update is called once per frame
    void Update()
    {
        if (testCoin)
        {
            ActivateCoin(Vector3.zero);
            testCoin = false;
        }
    }

    void InitializeCoins()
    {
        coins = new List<GameObject>();
        for (int i = 0; i < maxCoins; i++)
        {
            GameObject coin = Instantiate(coinPrefab, coinHolder.transform);
            coins.Add(coin);
            coin.SetActive(false);
        }
    }

    public static void ActivateCoin(Vector3 startPos)
    {
        if (instance.coins.Count > 0)
        {
            for (int i = 0; i < instance.coins.Count; i++)
            {
                if (!instance.coins[i].activeInHierarchy)
                {
                    instance.coins[i].SetActive(true);
                    instance.coins[i].GetComponent<Coin_behav>().AnimateCoin(startPos);
                    return;
                }
            }
        }
    }


}
