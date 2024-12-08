using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ball_behav : MonoBehaviour
{
    public AnimationCurve bounceCurve;
    public float ballRiseRate, rotationRate;
    public bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        GameManager.instance.GetComponent<Interaction_behav>().UpdateSelectedBalls(this);
    }

    public void OnSelectedAnimateBounce()
    {
        if (!isSelected)
        {
            isSelected = true;
            StartCoroutine(SelectedBounceCoroutine());
        }
        else
        {
            isSelected = false;
        }

    }

    public void ResetBall()
    {
        isSelected = false;
    }

    IEnumerator SelectedBounceCoroutine()
    {
        Vector3 initPos = transform.localPosition;
        Transform ballMesh = transform.Find("Ball_Mesh").transform;
        Material Shadowmat = transform.Find("Shadow").GetComponent<MeshRenderer>().material;

        float i = 0;
        while (isSelected)
        {
            if (i < 1f)
            {
                transform.localPosition = new Vector3(initPos.x, bounceCurve.Evaluate(i), initPos.z);
                Shadowmat.SetFloat("_AlphaVal", bounceCurve.Evaluate(1f - i));
                i += ballRiseRate;
            }
            
            ballMesh.Rotate(transform.up, bounceCurve.Evaluate(i) * rotationRate);

            //testBounceVal = bounceCurve.Evaluate(Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)));
            //transform.localPosition = new Vector3(transform.localPosition.x, testBounceVal, transform.localPosition.z);
            yield return null;
        }
        i = 1f;
        while (!isSelected && i > 0)
        {
            if (i > 0)
            {
                transform.localPosition = new Vector3(initPos.x, bounceCurve.Evaluate(i), initPos.z);
                Shadowmat.SetFloat("_AlphaVal", bounceCurve.Evaluate(1f - i));
                i -= ballRiseRate;
            }

            ballMesh.Rotate(transform.up, bounceCurve.Evaluate(i) * rotationRate);
            yield return null;
        }
        i = 0f;
    }


    public void BounceTowards(GameObject BallA, Vector3 otherPosition)
    {
        StartCoroutine(BounceTowardsCoroutine(BallA, otherPosition));
    }

    IEnumerator BounceTowardsCoroutine(GameObject BallA, Vector3 otherPosition)
    {
        isSelected = false;
        float step = 0;
        Vector3 initPos = BallA.transform.localPosition;
        while (bounceCurve.Evaluate(step) < 1f)
        {
            BallA.transform.localPosition = Vector3.Slerp(initPos, otherPosition, bounceCurve.Evaluate(step));
            step += Time.deltaTime * 1f;
            yield return null;
        }
        BallA.transform.localPosition = otherPosition;
    }
}
