using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSController : Singleton<RTSController>
{
    protected RTSController() { }

    [SerializeField]
    private float updateTimeStepSeconds = 1.0f;
    private float deltaTime = 0.0f;

    private List<IRTSUpdateReciever> registeredUpdateReceivers = new List<IRTSUpdateReciever>();

    private void FixedUpdate()
    {
        deltaTime += Time.deltaTime;
        if(deltaTime >= updateTimeStepSeconds)
        {
            InvokeRTSUpdate();
            deltaTime -= updateTimeStepSeconds;
        }
    }

    public void RegisterUpdateReceiver(IRTSUpdateReciever reciever)
    {
        if (!registeredUpdateReceivers.Contains(reciever))
            registeredUpdateReceivers.Add(reciever);
    }

    public void DeregisterUpdateReceiver(IRTSUpdateReciever reciever)
    {
        if (registeredUpdateReceivers.Contains(reciever))
            registeredUpdateReceivers.Remove(reciever);
    }

    private void InvokeRTSUpdate()
    {
        foreach (var receiver in registeredUpdateReceivers)
            receiver.UpdateRTSSimulation(1);
    }
}
