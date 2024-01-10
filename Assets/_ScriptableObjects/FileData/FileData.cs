using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New FileData", menuName = "ResidentEvilClone/File Data")]
public class FileData : ScriptableObject
{
    public string title;
    [TextArea(3, 10)]
    public string[] body;

    public Sprite fileIcon;
}