using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalBroadcast
{
    public static void Broadcast(string methodName, params object[] parameter)
    {
        foreach (var gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            gameObject.BroadcastMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }
}

