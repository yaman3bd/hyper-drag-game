using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UtilitiesScript
{
    public static float Remap(this float value, float inMin, float inMax, float outMin, float outMax)
    {
        return outMin + (((value - inMin) / (inMax - inMin)) * (outMax - outMin));
    }

    public static bool IsBetween(this float value, float bound1, float bound2)
    {
        return (value >= Math.Min(bound1, bound2) && value <= Math.Max(bound1, bound2));
    }
    public static void SetAlpha(this Graphic graphic, float alpha)
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }
}
