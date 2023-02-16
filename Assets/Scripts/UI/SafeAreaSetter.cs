using UnityEngine;

// Modified version of Adriaan de Jongh's script
// Source: https://forum.unity.com/threads/canvashelper-resizes-a-recttransform-to-iphone-xs-safe-area.521107

/// <summary>
/// Sets the size of a UI panel, which represents the safe area on phones that have rounded corners or notches.
/// </summary>
public class SafeAreaSetter : MonoBehaviour
{
    public RectTransform safeAreaTransform;
    
#if UNITY_ANDROID || UNITY_IOS
    private bool _screenChangeVarsInitialized;
    private ScreenOrientation _lastOrientation = ScreenOrientation.LandscapeLeft;
    private Vector2 _lastResolution = Vector2.zero;
    private Rect _lastSafeArea = Rect.zero;
    private Canvas _canvas;
    
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
 
        if(!_screenChangeVarsInitialized)
        {
            _lastOrientation = Screen.orientation;
            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;
            _lastSafeArea = Screen.safeArea;
 
            _screenChangeVarsInitialized = true;
        }
 
        ApplySafeArea();
    }
 
    private void Update()
    {
        if(Application.isMobilePlatform && Screen.orientation != _lastOrientation)
            OrientationChanged();
 
        if(Screen.safeArea != _lastSafeArea)
            SafeAreaChanged();
 
        if(Screen.width != _lastResolution.x || Screen.height != _lastResolution.y)
            ResolutionChanged();
    }
 
    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
 
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= _canvas.pixelRect.width;
        anchorMin.y /= _canvas.pixelRect.height;
        anchorMax.x /= _canvas.pixelRect.width;
        anchorMax.y /= _canvas.pixelRect.height;
 
        safeAreaTransform.anchorMin = anchorMin;
        safeAreaTransform.anchorMax = anchorMax;
    }
 
    private void OrientationChanged()
    {
        _lastOrientation = Screen.orientation;
        _lastResolution.x = Screen.width;
        _lastResolution.y = Screen.height;
    }
 
    private void ResolutionChanged()
    {
        _lastResolution.x = Screen.width;
        _lastResolution.y = Screen.height;
    }
 
    private void SafeAreaChanged()
    {
        _lastSafeArea = Screen.safeArea;

        ApplySafeArea();
    }
#endif
}