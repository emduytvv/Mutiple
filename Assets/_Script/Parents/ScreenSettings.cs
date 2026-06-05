using UnityEngine;

public static class ScreenSettings
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        Application.runInBackground = true;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
}
