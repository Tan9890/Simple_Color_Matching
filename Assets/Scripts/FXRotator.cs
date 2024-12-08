using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXRotator : MonoBehaviour
{
    public List<RectTransform> FXElements;
    public List<float> elementSpeed;
    bool isRotating = false;
    Coroutine fxRoutine;
    // Start is called before the first frame update
    void Start()
    {
        RotateFX();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RotateFX()
    {
        //if (fxRoutine != null)
        //    StopCoroutine(fxRoutine);
        isRotating = false;
        fxRoutine = StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        isRotating = true;
        while (isRotating)
        {
            for (int i = 0; i < FXElements.Count; i++)
            {
                FXElements[i].Rotate(Vector3.forward, elementSpeed[i]);
            }
            yield return null;
        }
    }
}
