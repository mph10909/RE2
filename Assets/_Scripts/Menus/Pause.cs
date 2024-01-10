using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pause
{
    public static bool paused;

    public static void Paused()
    {
        paused = true;
        Time.timeScale = 0;
    }

    public static void UnPaused()
    {
        paused = false;
        Time.timeScale = 1;
    }
}
