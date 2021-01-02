using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeGraph<T> : MonoBehaviour where T : MonoBehaviour
{
    private DataNode<T> root;
}
