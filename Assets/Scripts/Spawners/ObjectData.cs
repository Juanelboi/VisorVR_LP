using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ObjectData
{
    public string id;
    public string nombre;
    public string prefabName;
    public double longitude;
    public double latitude;
    public double height;
    public SerializableVector3 scale;
    public string tag;

    public SerializableVector3 localOffset;
}


[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public Vector3 ToVector3() => new Vector3(x, y, z);
}


[Serializable]
public class ObjectDataList
{
    public List<ObjectData> objects;
}