using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public float speed = 3.5f, Amp, Lam, AngFreq, rate;
    private float X;
    private float Y;
    bool isCamShaking;
    public bool testSHake = false;

    // Start is called before the first frame update
    void Start()
    {
        Interaction_behav.onMatch.AddListener(CamShake);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * speed, 0));
        }

        if (testSHake)
        {
            CamShake();
            testSHake = false;
        }
    }

    public void CamShake()
    {
        isCamShaking = false;
        StartCoroutine(CamShakeRoutine());
    }

    IEnumerator CamShakeRoutine()
    {
        isCamShaking = true;
        float c = 1f;
        Transform cam = Camera.main.transform;
        float DampedSin = 1f;
        float initTime = Time.time;
        while (c > 0 && isCamShaking)
        {
            DampedSin = Amp * Mathf.Exp(-Lam * (Time.time - initTime)) * Mathf.Cos(AngFreq * (Time.time - initTime));
            cam.localEulerAngles = new Vector3(cam.localEulerAngles.x, cam.localRotation.y, DampedSin);
            c -= rate;
            yield return null;
        }
        cam.localEulerAngles = new Vector3(cam.localEulerAngles.x, cam.localEulerAngles.y, 0);

        isCamShaking = false;
    }
}
