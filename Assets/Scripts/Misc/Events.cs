using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { };
[System.Serializable]
public class Vector4Event : UnityEvent<Vector4> { };
[System.Serializable]
public class Vector3Event : UnityEvent<Vector3> { };
[System.Serializable]
public class Vector2Event : UnityEvent<Vector2> { };
[System.Serializable]
public class BoolEvent : UnityEvent<bool> { };
[System.Serializable]
public class GameObjectIntEvent : UnityEvent<GameObject, int> { };
[System.Serializable]
public class IntIntEvent : UnityEvent<int, int> { };
[System.Serializable]
public class IntStringEvent : UnityEvent<int, string> { };
[System.Serializable]
public class Matrix4x4Event : UnityEvent<Matrix4x4> { };
public class IntBoolEvent : UnityEvent<int, bool> { };