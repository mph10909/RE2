using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    [CreateAssetMenu(fileName = "SceneReference", menuName = "ResidentEvilClone/Scene Management/Scene Reference")]
    public class SceneReference : ScriptableObject
    {
        public string sceneName;
    }
}
