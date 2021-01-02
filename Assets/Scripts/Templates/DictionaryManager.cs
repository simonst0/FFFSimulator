using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager<K, V> : MonoBehaviour where V : MonoBehaviour where K : IComparable
{
    protected Dictionary<K, V> managedReferences = new Dictionary<K, V>();

    public virtual void Add(K key, V value)
    {
        managedReferences.Add(key, value);
    }

    public virtual void RemoveAt(K key)
    {
        managedReferences.Remove(key);
    }

    public V GetAt(K key)
    {
        if (managedReferences.ContainsKey(key))
            return managedReferences[key];
        else
            return default(V);
    }
}
