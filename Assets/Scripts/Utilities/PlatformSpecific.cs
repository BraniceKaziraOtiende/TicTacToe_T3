using UnityEngine;

/// <summary>
/// Utility class for platform-specific functionality.
/// Demonstrates Unity conditional compilation.
/// </summary>
public class PlatformSpecific : MonoBehaviour
{
    [Header("Platform-Specific GameObjects")]
    [SerializeField] private GameObject pcOnlyUI;
    [SerializeField] private GameObject mobileOnlyUI;
    [SerializeField] private GameObject webGLOnlyUI;

    private void Awake()
    {
        ConfigurePlatformUI();
        SetupPlatformSpecificSettings();
    }

    /// <summary>
    /// Show/hide UI based on platform
    /// </summary>
    private void ConfigurePlatformUI()
    {
#if UNITY_STANDALONE
        // PC/Mac/Linux
        SetActiveIfNotNull(pcOnlyUI, true);
        SetActiveIfNotNull(mobileOnlyUI, false);
        SetActiveIfNotNull(webGLOnlyUI, false);
        Debug.Log("Platform: PC/Standalone");

#elif UNITY_ANDROID
        // Android
        SetActiveIfNotNull(pcOnlyUI, false);
        SetActiveIfNotNull(mobileOnlyUI, true);
        SetActiveIfNotNull(webGLOnlyUI, false);
        Debug.Log("Platform: Android");
        
#elif UNITY_IOS
        // iOS
        SetActiveIfNotNull(pcOnlyUI, false);
        SetActiveIfNotNull(mobileOnlyUI, true);
        SetActiveIfNotNull(webGLOnlyUI, false);
        Debug.Log("Platform: iOS");
        
#elif UNITY_WEBGL
        // WebGL
        SetActiveIfNotNull(pcOnlyUI, false);
        SetActiveIfNotNull(mobileOnlyUI, false);
        SetActiveIfNotNull(webGLOnlyUI, true);
        Debug.Log("Platform: WebGL");
        
#else
        // Unknown/Other platforms
        Debug.Log("Platform: Other");
#endif
    }

    /// <summary>
    /// Setup platform-specific settings
    /// </summary>
    private void SetupPlatformSpecificSettings()
    {
#if UNITY_ANDROID || UNITY_IOS
        // Mobile-specific settings
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
#elif UNITY_STANDALONE
        // PC-specific settings
        Application.targetFrameRate = -1; // Unlimited

#elif UNITY_WEBGL
        // WebGL-specific settings
        Application.targetFrameRate = 60;
#endif
    }

    /// <summary>
    /// Get platform-specific button size multiplier
    /// </summary>
    public static float GetButtonSizeMultiplier()
    {
#if UNITY_ANDROID || UNITY_IOS
        return 1.3f; // Larger buttons for mobile
#else
        return 1.0f; // Normal size for PC/WebGL
#endif
    }

    /// <summary>
    /// Check if quit functionality should be available
    /// </summary>
    public static bool CanQuitApplication()
    {
#if UNITY_WEBGL
        return false; // Can't quit a web game
#else
        return true;
#endif
    }

    /// <summary>
    /// Get platform display name
    /// </summary>
    public static string GetPlatformName()
    {
#if UNITY_STANDALONE_WIN
        return "Windows";
#elif UNITY_STANDALONE_OSX
        return "macOS";
#elif UNITY_STANDALONE_LINUX
        return "Linux";
#elif UNITY_ANDROID
        return "Android";
#elif UNITY_IOS
        return "iOS";
#elif UNITY_WEBGL
        return "WebGL";
#else
        return "Unknown";
#endif
    }

    /// <summary>
    /// Helper method to safely set GameObject active state
    /// </summary>
    private void SetActiveIfNotNull(GameObject go, bool active)
    {
        if (go != null)
        {
            go.SetActive(active);
        }
    }

    /// <summary>
    /// Check if running on mobile
    /// </summary>
    public static bool IsMobile()
    {
#if UNITY_ANDROID || UNITY_IOS
        return true;
#else
        return false;
#endif
    }

    /// <summary>
    /// Get recommended canvas reference resolution
    /// </summary>
    public static Vector2 GetCanvasResolution()
    {
#if UNITY_ANDROID || UNITY_IOS
        return new Vector2(1080, 1920); // Portrait for mobile
#else
        return new Vector2(1920, 1080); // Landscape for PC/WebGL
#endif
    }
}