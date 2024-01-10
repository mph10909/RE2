using System.Collections;
using System;
using UnityEngine;

public interface ISavableObjectState : ISavable<string> { }  

[Serializable]
public struct ObjectStateEntry
{
    public string Name;
}

public class SaveObjectState : MonoBehaviour
{

}
