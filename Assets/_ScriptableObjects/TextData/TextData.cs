using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New TextData", menuName = "ResidentEvilClone/Text Data")]
public class TextData : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] body;
}
