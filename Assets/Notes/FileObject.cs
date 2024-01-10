using UnityEngine;
using UnityEngine.UI;

public class FileObject : MonoBehaviour
{
    public Text titleText;
    public Text bodyText;


    [SerializeField]
    private FileData fileData;

    public void SetFileData(FileData fileData)
    {
        this.fileData = fileData;
        titleText.text = fileData.title;
        bodyText.text = string.Join("\n", fileData.body);
    }

    public void OnClick()
    {
        // Do something when the file object is clicked
    }
}
