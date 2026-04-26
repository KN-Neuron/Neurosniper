using System;
using System.Collections.Generic;
using UnityEngine;

public static class DamageMultipliers
{
    private static Dictionary<string, float> bodyDamageMultipliers = new Dictionary<string, float>
    {
        { "Hips", 1.0f },
        { "MiddleSpine", 1.5f },
        { "UpperLeg", 0.6f },
        { "LowerLeg", 0.4f },
        { "Shoulder", 0.8f },
        { "Elbow", 0.4f },
        { "Head", 2.0f },
    };

    public static float GetDamageMultiplayer(string bodyPartName)
    {
        if (bodyDamageMultipliers.TryGetValue(bodyPartName, out float multiplier))
        {
            return multiplier;
        }
        return 1.0f;
    }
}
