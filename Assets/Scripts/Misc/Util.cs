using System.Collections;
using UnityEngine;
using System;

public class Util
{
    public static IEnumerator DelayedExecution(float seconds, Action delayedFunction)
    {
        yield return new WaitForSeconds(seconds);
        delayedFunction.Invoke();
    }

    public static IEnumerator DelayedMultipleExecution(float seconds, int times, MonoBehaviour source, Action delayedFunction)
    {
        yield return new WaitForSeconds(seconds);
        if (times > 0)
        {
            delayedFunction.Invoke();
            --times;
            source.StartCoroutine(DelayedMultipleExecution(seconds, times, source, delayedFunction));
        }
    }

    public static IEnumerator DelayedMultipleExecutionFinally(float seconds, int times, MonoBehaviour source, Action delayedFunction, Action finalFunction)
    {
        yield return new WaitForSeconds(seconds);
        if (times > 0)
        {
            delayedFunction.Invoke();
            --times;
            source.StartCoroutine(DelayedMultipleExecutionFinally(seconds, times, source, delayedFunction, finalFunction));
        }
        else
            finalFunction.Invoke();
    }

    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
    {
        return new Vector3(
            Mathf.Clamp(value.x, min.x, max.x),
            Mathf.Clamp(value.y, min.y, max.y),
            Mathf.Clamp(value.z, min.z, max.z)
            );
    }

    public static Color GetColorInRange(float min, float max)
    {
        return new Color(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    }

    public static string GetPrefabNameFromObject(GameObject target)
    {
        return target.name.TrimEnd(new char[] { '(', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ')', ' ' });
    }

    public static string FormatVector(Vector3 value)
    {
        return "<b>(" + value.x + "|" + value.y + "|" + value.z + ")</b>";
    }

    public static bool RandomBool(float probability)
    {
        return UnityEngine.Random.value <= probability;
    }

    public static Vector3 GetRandomVector3InRange(float min, float max)
    {
        return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    }

    public static Vector3 GetRandomPow2Vector(int minPow, int maxPow)
    {
        return new Vector3(Mathf.Pow(2, UnityEngine.Random.Range(minPow, maxPow)),
            Mathf.Pow(2, UnityEngine.Random.Range(minPow, maxPow)),
            Mathf.Pow(2, UnityEngine.Random.Range(minPow, maxPow)));
    }

    public static int RandomSign()
    {
        return UnityEngine.Random.value > 0.5f ? -1 : 1;
    }

    public static T GetBroadcastParamAtIndex<T>(object[] list, int index)
    {
        try
        {
            return (T)(((object[])list[1])[index]);
        }
        catch (System.InvalidCastException e)
        {
            Debug.LogError(e.Message + " can not cast to " + typeof(T));
            return default(T);
        }
    }
    public static Vector3 DivideVector3(Vector3 left, Vector3 right)
    {
        left.x /= right.x;
        left.y /= right.y;
        left.z /= right.z;
        return left;
    }

    public static int ColumnRowToIndex(int row, int column, int columnCount)
    {
        return columnCount * row + column;
    }

    public static double NormalDistributionProbabilityDensity(float x, float mean, float standardDeviation)
    {
        //e ^ (-(x - μ) ^ 2 / (2 σ ^ 2))/ (sqrt(2 π) σ)
        return Math.Pow(Math.E, -Math.Pow(x - mean, 2) / (2 * Math.Pow(standardDeviation, 2)) / (Math.Sqrt(2 * Math.PI) * standardDeviation));
    }

    //NOT WORKING!

    public static double NormalDistributionCummulative(float x, float mean, float standardDeviation)
    {
        //P(X <= x) = 1 / 2 erfc((μ - x) / (sqrt(2) σ 
        return 1 / 2 * ErrorFunction((mean - x) / (Math.Sqrt(2) * standardDeviation));
    }

    public static double ErrorFunction(double x)
    {
        //https://www.johndcook.com/blog/csharp_erf/ 18.12.2020
        // constants
        double a1 = 0.254829592;
        double a2 = -0.284496736;
        double a3 = 1.421413741;
        double a4 = -1.453152027;
        double a5 = 1.061405429;
        double p = 0.3275911;

        // Save the sign of x
        int sign = 1;
        if (x < 0)
            sign = -1;
        x = Math.Abs(x);

        // A&S formula 7.1.26
        double t = 1.0 / (1.0 + p * x);
        double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

        return sign * y;
    }
}
