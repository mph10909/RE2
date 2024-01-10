using UnityEngine;
using UnityEngine.Serialization;

    [CreateAssetMenu(fileName = "Animator State", menuName = "Animator State")]
    public class AnimatorScript : ScriptableObject
    {
        [FormerlySerializedAs("m_StateName")]
        public string StateName;
        public int Hash = 0;

        public void OnValidate()
        {
            Hash = Animator.StringToHash(StateName);
        }

        public void ResetName(string name)
        {
            StateName = name;
            Hash = Animator.StringToHash(StateName);
        }
    }
