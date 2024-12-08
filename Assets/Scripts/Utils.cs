using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float Map(float val, float minRange, float maxRange)
    {
        return minRange + val * (maxRange - minRange);
    }
}
