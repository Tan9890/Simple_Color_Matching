using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interaction_behav : MonoBehaviour
{
    public List<GameObject> selectedBalls;
    int maxSelectedBalls = 2;
    public AnimationCurve matchConditionCurve;
    public float matchConditionSpeed;
    public ParticleSystem matchConditionPSEffect;
    public static UnityEvent onMatch;
    public static UnityEvent onMisMatch;

    // Start is called before the first frame update
    void Start()
    {
        selectedBalls = new List<GameObject>();
        maxSelectedBalls = GetComponent<Ball_Mechanics>().maxMatches;
        matchConditionPSEffect.Stop();
        onMatch = new UnityEvent();
        onMisMatch = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        #region old_Code
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            if (hit.collider.transform.parent.CompareTag("Ball") && selectedBalls.Count < maxSelectedBalls)
        //            {
        //                if (selectedBalls.Count == 0)
        //                {
        //                    selectedBalls.Add(hit.collider.gameObject.transform.parent.gameObject);
        //                }
        //                else if (hit.collider.gameObject.transform.parent.gameObject != selectedBalls[0])
        //                {
        //                    selectedBalls.Add(hit.collider.gameObject.transform.parent.gameObject);
        //                }
        //            }
        //        }
        //    }

        //    if (selectedBalls.Count >= maxSelectedBalls)
        //        CheckifMatch();
        #endregion

    }

    public void UpdateSelectedBalls(Ball_behav _ball_behav)
    {
        if (selectedBalls.Count >= 0 && selectedBalls.Count < maxSelectedBalls)
        {
            if (selectedBalls.Contains(_ball_behav.gameObject))
            {
                selectedBalls.Remove(_ball_behav.gameObject);
            }
            else
            {
                if (selectedBalls.Count < maxSelectedBalls)
                {
                    selectedBalls.Add(_ball_behav.gameObject);
                }
                if (selectedBalls.Count >= maxSelectedBalls)
                {
                    CheckifMatch();
                }
            }
        }
    }


    void CheckifMatch()
    {
        List<Color> colors = new List<Color>();
        foreach (GameObject ball in selectedBalls)
            colors.Add(ball.transform.Find("Ball_Mesh").gameObject.GetComponent<MeshRenderer>().material.GetColor("_MainColor"));
        if (IsSameColor(colors))
        {
            Debug.Log("Match!");
            StartCoroutine(AnimateMatchCondition(selectedBalls));
        }
        else
        {
            Debug.Log("Not Match, you blind fellow!");
            onMisMatch.Invoke();
            foreach (GameObject ball in selectedBalls)
                ball.GetComponent<Ball_behav>().ResetBall();
            selectedBalls = new List<GameObject>();
        }

    }

    Vector3 GetMidpoint()
    {
        Vector3 mp = Vector3.zero;
        if (selectedBalls.Count > 0)
        {
            foreach (GameObject ball in selectedBalls)
            {
                mp.x += ball.transform.position.x;
                mp.y += ball.transform.position.y;
                mp.z += ball.transform.position.z;
                mp /= selectedBalls.Count;
                mp.y += 2f;
            }
        }
        return mp;
    }

    IEnumerator AnimateMatchCondition(List<GameObject> _selectedBalls)
    {
        Vector3 mp = GetMidpoint();
        List<Vector3> initPos = new List<Vector3>();
        if (_selectedBalls.Count > 0)
        {
            for (int i = 0; i < _selectedBalls.Count; i++)
                initPos.Add(_selectedBalls[i].transform.position);

            float curvePos = 0;
            while (curvePos < 1f)
            {
                for (int i = 0; i < _selectedBalls.Count; i++)
                {
                    _selectedBalls[i].transform.position = Vector3.Slerp(initPos[i], mp, matchConditionCurve.Evaluate(curvePos));
                    curvePos += matchConditionSpeed;
                }
                yield return null;
            }

            curvePos = 1f;
            for (int i = 0; i < _selectedBalls.Count; i++)
            {
                _selectedBalls[i].transform.position = mp;
            }
            matchConditionPSEffect.transform.position = mp;
            matchConditionPSEffect.Play();
            //Camera.main.transform.parent.GetComponent<CamControl>().CamShake();
            onMatch.Invoke();

            for (int i = 0; i < _selectedBalls.Count; i++)
            {
                for (int j = 0; j < ScoreMultiplier.currentMultiplier; j++)
                {
                    CoinSystem.ActivateCoin(mp);
                    yield return null;
                }
            }

            for (int i = 0; i < _selectedBalls.Count; i++)
            {
                _selectedBalls[i].SetActive(false);
            }

            foreach (GameObject ball in selectedBalls)
                ball.GetComponent<Ball_behav>().ResetBall();
            selectedBalls = new List<GameObject>();
        }
    }

    bool IsSameColor(List<Color> colors)
    {
        bool isSame = true;
        for (int i = 1; i < colors.Count; i++)
        {
            isSame &= (colors[0].r == colors[i].r & colors[0].g == colors[i].g & colors[0].b == colors[i].b);
        }
        return isSame;
    }
}
