using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_behav : MonoBehaviour
{
    public List<GameObject> selectedBalls;
    public int maxSelectedBalls = 2;
    // Start is called before the first frame update
    void Start()
    {
        selectedBalls = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform.parent.CompareTag("Ball") && selectedBalls.Count < maxSelectedBalls)
                {
                    if (selectedBalls.Count == 0)
                        selectedBalls.Add(hit.collider.gameObject.transform.parent.gameObject);
                    else if (hit.collider.gameObject != selectedBalls[0])
                    {
                        selectedBalls.Add(hit.collider.gameObject.transform.parent.gameObject);
                    }
                }
            }
        }

        if (selectedBalls.Count >= maxSelectedBalls)
            CheckifMatch();
    }

    void CheckifMatch()
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < selectedBalls.Count; i++)
        {
            colors.Add(selectedBalls[i].GetComponentInChildren<MeshRenderer>().material.GetColor("_Color"));
        }
        if (IsSameColor(colors[0],colors[1]))
        {
            Debug.Log(colors[0]);
            Debug.Log(colors[1]);

            selectedBalls[0].transform.gameObject.SetActive(false);
            selectedBalls[1].transform.gameObject.SetActive(false);
        }
        selectedBalls = new List<GameObject>();
    }

    bool IsSameColor(Color a, Color b)
    {
        return a.r == b.r && a.g == b.g && a.b == b.b ? true : false;
    }
}
