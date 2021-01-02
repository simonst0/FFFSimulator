using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class IndexingDictionaryManager<K, V> : DictionaryManager<K, V> where V : MonoBehaviour where K : System.IComparable
{
    public string keyFieldName = "key";
    public Transform targetParent;
    public GameObject prefab;
    public ChildAddMode childAddMode = ChildAddMode.First;

    protected virtual void Awake()
    {
        if (!targetParent)
            Debug.LogError("Target parent not assigned");
        int i = 0;
        foreach (var value in targetParent.GetComponentsInChildren<V>())
        {
            K key = (K)value.GetType().GetField(keyFieldName).GetValue(value);
            Add(key, value);
            ++i;
        }
    }

    public virtual V AddNew(K key)
    {
        GameObject instantiatedPrefab = GameObject.Instantiate(prefab, targetParent);
        V value;
        if (!instantiatedPrefab.TryGetComponent<V>(out value))
        {
            Debug.LogError("Prefab does not have component of type " + typeof(V).ToString());
            return default(V);
        }
        Add(key, value);
        return value;
    }

    public override void Add(K key, V value)
    {
        base.Add(key, value);
        SetParent(value.transform);
    }

    public override void RemoveAt(K key)
    {
        V value = GetAt(key);
        Destroy(value);
        base.RemoveAt(key);
    }

    protected void SetParent(Transform child)
    {
        switch (childAddMode)
        {
            case ChildAddMode.First:
                child.SetParent(targetParent);
                child.SetAsFirstSibling();
                break;
            case ChildAddMode.Last:
                child.SetParent(targetParent);
                child.SetAsLastSibling();
                break;
            case ChildAddMode.Penultimate:
                child.SetParent(targetParent);
                if (targetParent.childCount == 1)
                    child.SetAsFirstSibling();
                else if (targetParent.childCount > 1)
                    child.SetSiblingIndex(targetParent.childCount - 2);
                break;
            default: //none
                break;
        }
    }

    public enum ChildAddMode
    {
        None,
        First,
        Last,
        Penultimate
    }
}
