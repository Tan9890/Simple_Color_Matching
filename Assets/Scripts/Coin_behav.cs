using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_behav : MonoBehaviour
{
    public AnimationCurve initialAnim, secondAnim;
    public float initAnimSpeed, secondAnimSpeed, initRotSpeed, initDist;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimateCoin(Vector3 startPos)
    {
        StartCoroutine(Animate(startPos));
    }

    IEnumerator Animate(Vector3 _startPos)
    {
        Vector3 ran = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f));
        Debug.Log(ran);
        Vector3 randDir = ran.normalized;
        Transform CoinModel = transform.Find("Coin_holder");
        transform.position = _startPos;
        float t = 0;
        while(t < 1f)
        {
            transform.Translate(randDir * initDist * initialAnim.Evaluate(1f - t));
            CoinModel.Rotate(randDir, initRotSpeed * initialAnim.Evaluate(1f - t));
            t += initAnimSpeed;
            yield return null;
        }
        t = 0;
        Vector3 intermPos = transform.position;
        while (t < 1f)
        {
            transform.position = Vector3.Slerp(intermPos, CoinSystem.instance.scoreTrigger.transform.position, secondAnim.Evaluate(t));
            CoinModel.Rotate(randDir, initRotSpeed * initialAnim.Evaluate(t));
            t += secondAnimSpeed;
            yield return null;
        }
        ScoreKeeper.UpdateScore(CoinSystem.instance.CoinValue);
        gameObject.SetActive(false);
    }
}
