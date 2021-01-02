using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public abstract class DataNode<T> : MonoBehaviour
{
    [SerializeField]
    public UnityEvent<DataNode<T>> OnValueChanged = new UnityEvent<DataNode<T>>();
    [SerializeField]
    protected T _value;
    public T value
    {
        get { return _value; }
        set
        {
            _value = value;
            OnValueChanged.Invoke(this);
        }
    }


    protected List<DataNode<T>> connections;

    private void Awake()
    {
        foreach (var connection in connections)
        {
            OnValueChanged.AddListener(connection.OnConnectedNodeValueChanged);
        }
    }

    public void AddConnection(DataNode<T> node)
    {
        if (connections.Contains(node))
            return;
        connections.Add(node);
        OnValueChanged.AddListener(node.OnConnectedNodeValueChanged);
    }

    public void RemoveConnection(DataNode<T> node)
    {
        if (!connections.Contains(node))
            return;
        connections.Remove(node);
        OnValueChanged.RemoveListener(node.OnConnectedNodeValueChanged);
    }

    protected abstract void OnConnectedNodeValueChanged(DataNode<T> cause);
}