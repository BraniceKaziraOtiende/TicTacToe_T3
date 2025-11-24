using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatically configures Canvas Scaler for different platforms.
/// Attach to Canvas GameObject in each scene.
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class ResponsiveCanvasSetup : MonoBehaviour
{
    [Header("Reference Resolutions")]
    [SerializeField] private Vector2 pcResolution = new Vector2(1920, 1080);
    [SerializeField] private Vector2 mobilePortraitResolution = new Vector2(1080, 1920);
    [SerializeField] private Vector2 mobileLandscapeResolution = new Vector2(1920, 1080);

    [Header("Match Values")]
    [SerializeField] private float defaultMatch = 0.5f;
    [SerializeField] private float mobilePortraitMatch = 0.5f;
    [SerializeField] private float mobileLandscapeMatch = 0.5f;

    private CanvasScaler canvasScaler;

    private void Awake()
    {
        ConfigureCanvasScaler();
    }

    /// <summary>
    /// Configure canvas scaler based on platform
    /// </summary>
    private void ConfigureCanvasScaler()
    {
        canvasScaler = GetComponent<CanvasScaler>();

        if (canvasScaler == null)
        {
            Debug.LogError("CanvasScaler component not found!");
            return;
        }

        // Set base settings
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        // Platform-specific configuration
#if UNITY_ANDROID || UNITY_IOS
            ConfigureForMobile();
#elif UNITY_WEBGL
        ConfigureForWebGL();
#else
            ConfigureForPC();
#endif

        Debug.Log($"Canvas configured for: {GetPlatformName()} - Resolution: {canvasScaler.referenceResolution}, Match: {canvasScaler.matchWidthOrHeight}");
    }

    /// <summary>
    /// Configure for PC/Standalone
    /// </summary>
    private void ConfigureForPC()
    {
        canvasScaler.referenceResolution = pcResolution;
        canvasScaler.matchWidthOrHeight = defaultMatch;
    }

    /// <summary>
    /// Configure for WebGL
    /// </summary>
    private void ConfigureForWebGL()
    {
        // WebGL uses same as PC but with balanced match
        canvasScaler.referenceResolution = pcResolution;
        canvasScaler.matchWidthOrHeight = 0.5f; // Always balanced for web
    }

    /// <summary>
    /// Configure for Mobile (detects orientation)
    /// </summary>
    private void ConfigureForMobile()
    {
        // Detect orientation
        bool isPortrait = Screen.height > Screen.width;

        if (isPortrait)
        {
            canvasScaler.referenceResolution = mobilePortraitResolution;
            canvasScaler.matchWidthOrHeight = mobilePortraitMatch;
            Debug.Log("Mobile: Portrait mode");
        }
        else
        {
            canvasScaler.referenceResolution = mobileLandscapeResolution;
            canvasScaler.matchWidthOrHeight = mobileLandscapeMatch;
            Debug.Log("Mobile: Landscape mode");
        }

        // Lock orientation (optional - for Tic-Tac-Toe, portrait might be better)
        // Uncomment if you want to force an orientation:
        // Screen.orientation = ScreenOrientation.Portrait;
    }

    /// <summary>
    /// Get current platform name
    /// </summary>
    private string GetPlatformName()
    {
#if UNITY_ANDROID
            return "Android";
#elif UNITY_IOS
            return "iOS";
#elif UNITY_WEBGL
        return "WebGL";
#elif UNITY_STANDALONE_WIN
            return "Windows";
#elif UNITY_STANDALONE_OSX
            return "macOS";
#elif UNITY_STANDALONE_LINUX
            return "Linux";
#else
            return "Unknown";
#endif
    }

    /// <summary>
    /// Handle orientation changes at runtime (mobile)
    /// </summary>
    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
            // Reconfigure if orientation changes
            if (Input.deviceOrientation == DeviceOrientation.Portrait || 
                Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                if (canvasScaler.referenceResolution != mobilePortraitResolution)
                {
                    ConfigureForMobile();
                }
            }
            else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || 
                     Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            {
                if (canvasScaler.referenceResolution != mobileLandscapeResolution)
                {
                    ConfigureForMobile();
                }
            }
#endif
    }

    /// <summary>
    /// Force reconfiguration (useful for testing)
    /// </summary>
    [ContextMenu("Reconfigure Canvas")]
    public void ReconfigureCanvas()
    {
        ConfigureCanvasScaler();
    }
}