using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public int desiredWidth = 1920; // Set your desired width
    public int desiredHeight = 1080; // Set your desired height
    public bool fullscreen = true; // Set whether you want fullscreen

    void Start()
    {
        // Set the resolution
        Screen.SetResolution(desiredWidth, desiredHeight, fullscreen);
    }
}
