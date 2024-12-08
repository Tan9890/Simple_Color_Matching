using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
    private float count;

    void Awake()
    {


    }

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 30;
#endif
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif

        GUI.depth = 2;
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 40, 100, 25), "FPS: " + Mathf.Round(count));
    }
}
