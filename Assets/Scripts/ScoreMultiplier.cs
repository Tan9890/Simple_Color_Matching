using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMultiplier : MonoBehaviour
{
    public CanvasGroup MultiplierPopUp;
    public static int currentMultiplier = 1;
    public int maxMatchSuccess;
    public AnimationCurve animcurve;
    public float animSpeed, initAnimMul, holdAnimMul, endAnimMul;
    [SerializeField]
    int matchSuccessCount = 0;
    Coroutine multiplierAnimRoutine;
    TMPro.TMP_Text multiplierDisplay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetListeners());
        multiplierDisplay = MultiplierPopUp.GetComponentInChildren<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetListeners()
    {
        while (Interaction_behav.onMatch == null)
        {
            yield return null;
        }
        Interaction_behav.onMatch.AddListener(UpdateMatchSuccessCount);
        while (Interaction_behav.onMisMatch == null)
        {
            yield return null;
        }
        Interaction_behav.onMisMatch.AddListener(ResetMatchSuccessCount);
    }

    void UpdateMatchSuccessCount()
    {
        if (matchSuccessCount < maxMatchSuccess)
        {
            matchSuccessCount++;
            currentMultiplier = (int)Mathf.Pow(2, matchSuccessCount - 1);
            Debug.Log("Currmul: " + currentMultiplier);
            if (multiplierAnimRoutine != null)
                StopCoroutine(multiplierAnimRoutine);
            multiplierAnimRoutine = StartCoroutine(MultiplierUpdate());
        }
            
    }

    void ResetMatchSuccessCount()
    {
        currentMultiplier = 1;
        matchSuccessCount = 0;
        if (multiplierAnimRoutine != null)
            StopCoroutine(multiplierAnimRoutine);
        multiplierAnimRoutine = StartCoroutine(MultiplierUpdate());
    }

    IEnumerator MultiplierUpdate()
    {
        float k = 0;
        while (k < 1f)
        {
            AnimationOps(k);
            k += animSpeed * initAnimMul;
            yield return null;
        }
        AnimationOps(1f);
        k = 0f;
        multiplierDisplay.text = currentMultiplier.ToString() + "X";
        while (k < 1f)
        {
            AnimationOps(1f);
            k += animSpeed * holdAnimMul;
            yield return null;
        }
        k = 1f;
        while (k > 0)
        {
            AnimationOps(k);
            k -= animSpeed * endAnimMul;
            yield return null;
        }
        k = 0;
        AnimationOps(k);
        yield return null;

    }

    void AnimationOps(float k)
    {
        MultiplierPopUp.alpha = animcurve.Evaluate(k);
        MultiplierPopUp.transform.localScale = Vector3.one * Utils.Map(animcurve.Evaluate(k), 0.6f, 1f);
    }


}
