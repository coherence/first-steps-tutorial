using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Changes cosmetics when a button is pressed. Disabled on remote characters.
/// </summary>
public class CosmeticsInput : MonoBehaviour
{
    public InputActionReference randomiseCosmeticsAction;

    private CosmeticsChanger _cosmetics;
    
    private void Awake()
    {
        _cosmetics = GetComponent<CosmeticsChanger>();
    }

    private void Start()
    {
        RandomiseCosmetics();
    }

    private void OnEnable()
    {
        randomiseCosmeticsAction.asset.Enable();
        randomiseCosmeticsAction.action.performed += OnCosmeticsActionPerformed;
    }

    private void OnDisable()
    {
        //randomiseCosmeticsAction.asset.Disable();
        randomiseCosmeticsAction.action.performed -= OnCosmeticsActionPerformed;
    }

    private void OnCosmeticsActionPerformed(InputAction.CallbackContext obj)
    {
        RandomiseCosmetics();
    }

    private void RandomiseCosmetics()
    {
        int newHat = _cosmetics.GetRandomHat();
        int newHairstyle = _cosmetics.GetRandomHairstyle();
        int newFacialHair = _cosmetics.GetRandomFacialHair();
        int newBackpack = _cosmetics.GetRandomBackpack();
        int newSkinTone = _cosmetics.GetRandomSkinTone();

        _cosmetics.ChangeAllCosmetics(newHat, newHairstyle, newFacialHair, newBackpack, newSkinTone);
    }
}
