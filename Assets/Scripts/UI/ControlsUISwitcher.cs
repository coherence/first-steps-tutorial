using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
#pragma warning disable CS0414

/// <summary>
/// Enables and disables mobile controls (based on compilation platform), and responds to the buttons
/// to switch controls on standalone platforms between keyboard and gamepad.
/// </summary>
public class ControlsUISwitcher : MonoBehaviour
{
    public GameObject mobileControls;
    public GameObject standaloneControls;

    public GameObject keyboardControls;
    public GameObject gamepadControls;

    public Button keyboardButton;
    public Button gamepadButton;

    public VerticalLayoutGroup bottomBarLayoutGroup;

    private static int CONTROLS_TYPE = 0;

    private void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        // Mobile controls
        mobileControls.SetActive(true);
        standaloneControls.SetActive(false);
        
        // Little hack to make sure the Vertical Layout Group doesn't break (until Unity fixes the issue...)
        bottomBarLayoutGroup.enabled = true;
#else
        // Standalone controls
        standaloneControls.SetActive(true);
        mobileControls.SetActive(false);
        
        // Little hack to make sure the Vertical Layout Group doesn't break (until Unity fixes the issue...)
        bottomBarLayoutGroup.enabled = false;

        if (CONTROLS_TYPE == 0)
            ShowKeyboardControls();
        else
            ShowGamepadControls();
#endif
    }

    public void ShowKeyboardControls()
    {
        CONTROLS_TYPE = 0;
        keyboardControls.SetActive(true);
        gamepadControls.SetActive(false);
        keyboardButton.interactable = false;
        gamepadButton.interactable = true;
    }
    
    public void ShowGamepadControls()
    {
        CONTROLS_TYPE = 1;
        gamepadControls.SetActive(true);
        keyboardControls.SetActive(false);
        gamepadButton.interactable = false;
        keyboardButton.interactable = true;
    }
}
