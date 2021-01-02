using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadcaster : Singleton<Broadcaster>
{
    protected Broadcaster() { }

    private Dictionary<string, Dictionary<MonoBehaviour, List<string>>> broadcastLookup = new Dictionary<string, Dictionary<MonoBehaviour, List<string>>>();
    private Queue<BroadcastRegistrationSignature> RegistrationQueue = new Queue<BroadcastRegistrationSignature>();
    private Queue<MonoBehaviour> DeregistrationAllQueue = new Queue<MonoBehaviour>();
    private Queue<BroadcastRegistrationSignature> DeregistrationQueue = new Queue<BroadcastRegistrationSignature>();

    private void LateUpdate()
    {
        ProcessDeregistrationAllQueue();
        ProcessDeregistrationQueue();
        ProcessRegistrationQueue();
    }

    public void RegisterToBroadcast(string broadcastType, string callbackMethodName, MonoBehaviour listener)
    {
        if (broadcastType == BroadcastType.None)
            return;
        RegistrationQueue.Enqueue(new BroadcastRegistrationSignature(broadcastType, callbackMethodName, listener));
    }

    /// <summary>
    /// Only use when not called inside another broadcast. Start, Awake etc are okay. Otherwise the collection could be enumerated incorrectly
    /// </summary>
    /// <param name="broadcastType"></param>
    /// <param name="callbackMethodName"></param>
    /// <param name="listener"></param>
    public void RegisterToBroadcastImmediate(string broadcastType, string callbackMethodName, MonoBehaviour listener)
    {
        if (broadcastType == BroadcastType.None)
            return;
        if (!broadcastLookup.ContainsKey(broadcastType))
            broadcastLookup.Add(broadcastType, new Dictionary<MonoBehaviour, List<string>>());
        if (!broadcastLookup[broadcastType].ContainsKey(listener))
            broadcastLookup[broadcastType].Add(listener, new List<string>());
        if (!broadcastLookup[broadcastType][listener].Contains(callbackMethodName))
            broadcastLookup[broadcastType][listener].Add(callbackMethodName);
        //Debug.Log("Listener " + listener.name + " added callback " + callbackMethodName + " to broadcast " + broadcastType);
    }

    public void DeregisterFromAllBroadcasts(MonoBehaviour listener)
    {
        DeregistrationAllQueue.Enqueue(listener);
    }

    public void DeregisterFromBroadcast(string broadcastType, string callbackMethodName, MonoBehaviour listener)
    {
        DeregistrationQueue.Enqueue(new BroadcastRegistrationSignature(broadcastType, callbackMethodName, listener));
    }

    public void Broadcast(string broadcastType, bool ensureDelivery, params object[] list)
    {
        object[] message = new object[2] { broadcastType, list };
        if (broadcastLookup.ContainsKey(broadcastType))
            foreach (var methodLookup in broadcastLookup[broadcastType])
                foreach (var method in methodLookup.Value)
                    if (methodLookup.Key.isActiveAndEnabled)
                        methodLookup.Key.SendMessage(method, message, SendMessageOptions.RequireReceiver);
                    else if (!methodLookup.Key.isActiveAndEnabled && ensureDelivery)
                    {
                        methodLookup.Key.enabled = true;
                        methodLookup.Key.SendMessage(method, message, SendMessageOptions.RequireReceiver);
                        methodLookup.Key.enabled = false;
                    }
    }

    private void ProcessRegistrationQueue()
    {
        while (RegistrationQueue.Count > 0)
        {
            BroadcastRegistrationSignature signature = RegistrationQueue.Dequeue();
            RegisterToBroadcastImmediate(signature.broadcastType, signature.callbackMethodName, signature.listener);
        }
    }

    public void DeregisterFromAllBroadcastsImmediate(MonoBehaviour listener)
    {
        foreach (var methodLookup in broadcastLookup)
            methodLookup.Value.Remove(listener);
        //Debug.Log("Listener " + listener.name + " removed from all callbacks");
    }

    private void ProcessDeregistrationAllQueue()
    {
        while (DeregistrationAllQueue.Count > 0)
        {
            MonoBehaviour listener = DeregistrationAllQueue.Dequeue();
            DeregisterFromAllBroadcastsImmediate(listener);
        }
    }

    private void ProcessDeregistrationQueue()
    {
        while (DeregistrationQueue.Count > 0)
        {
            BroadcastRegistrationSignature signature = DeregistrationQueue.Dequeue();
            if (broadcastLookup.ContainsKey(signature.broadcastType) && broadcastLookup[signature.broadcastType].ContainsKey(signature.listener))
            {
                broadcastLookup[signature.broadcastType][signature.listener].Remove(signature.callbackMethodName);
                //Debug.Log("Listener " + signature.listener.name + " removed callback " + signature.callbackMethodName + " from broadcast " + signature.broadcastType);
            }
        }
    }

    [System.Serializable]
    public struct BroadcastRegistrationSignature
    {
        public BroadcastRegistrationSignature(string broadcastType, string callbackMethodName, MonoBehaviour listener)
        {
            this.broadcastType = broadcastType;
            this.callbackMethodName = callbackMethodName;
            this.listener = listener;
        }
        public string broadcastType;
        public string callbackMethodName;
        public MonoBehaviour listener;
    }

    private struct BroadcastUndeliveredSignature
    {
        public BroadcastUndeliveredSignature(MonoBehaviour listener, string broadcastType, string callbackMethodName, object[] list)
        {
            signature = new BroadcastRegistrationSignature(broadcastType, callbackMethodName, listener);
            this.list = list;
        }
        public BroadcastRegistrationSignature signature;
        public object[] list;
    }
}

public static class BroadcastType
{
    public static string None = "none";
}