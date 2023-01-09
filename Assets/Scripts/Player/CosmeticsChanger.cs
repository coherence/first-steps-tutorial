using Coherence.Toolkit;
using UnityEngine;
using Random = UnityEngine.Random;

public class CosmeticsChanger : MonoBehaviour
{
    [Header("Synchronised values")]
    [OnValueSynced(nameof(OnSkinToneChanged))] public int currentSkinTone = -2;
    [OnValueSynced(nameof(OnHatChanged))] public int currentHat = -2;
    [OnValueSynced(nameof(OnHairStyleChanged))] public int currentHairStyle = -2;
    [OnValueSynced(nameof(OnFacialHairChanged))] public int currentFacialHair = -2;
    [OnValueSynced(nameof(OnBackpackChanged))] public int currentBackpack = -2;
    //-2 is an invalid value, used to check if a cosmetic hasn't been initialised yet
    
    [Header("Cosmetic elements")]
    public GameObject[] hats;
    public GameObject[] hairstyles;
    public GameObject[] facialHair;
    public GameObject[] backpacks;
    public Texture[] skinTones;

    [Header("References")]
    public SkinnedMeshRenderer bodyRenderer;
    public ParticleSystem sparks;

    public int GetRandomSkinTone() => Random.Range(0, skinTones.Length);
    public int GetRandomBackpack() => Random.Range(-1, backpacks.Length);
    public int GetRandomFacialHair() => Random.Range(-1, facialHair.Length);
    public int GetRandomHairstyle() => Random.Range(-1, hairstyles.Length);
    public int GetRandomHat() => Random.Range(0, hats.Length);

    /// <summary>
    /// Changes all cosmetics at once. This is only called on characters that the local Client has authority on, via <see cref="CosmeticsInput"/>.
    /// </summary>
    public void ChangeAllCosmetics(int newHat, int newHairStyle, int newFacialHair, int newBackpack, int newSkinTone)
    {
        OnHatChanged(currentHat, newHat);
        OnHairStyleChanged(currentHairStyle, newHairStyle);
        OnFacialHairChanged(currentFacialHair, newFacialHair);
        OnBackpackChanged(currentBackpack, newBackpack);
        OnSkinToneChanged(currentSkinTone, newSkinTone);
    }

    public void OnHatChanged(int oldValue, int newValue)
    {
        if (newValue != oldValue)
        {
            if(oldValue > -1) hats[oldValue].SetActive(false);
            hats[newValue].SetActive(true);
            currentHat = newValue;
            
            if(!sparks.isPlaying && oldValue != -2) sparks.Play();
        }
    }
    
    public void OnHairStyleChanged(int oldValue, int newValue)
    {
        if (newValue != oldValue)
        {
            if(oldValue > -1) hairstyles[oldValue].SetActive(false);
            if(newValue > -1) hairstyles[newValue].SetActive(true);
            currentHairStyle = newValue;
            
            if(!sparks.isPlaying && oldValue != -2) sparks.Play();
        }
    }
    
    public void OnFacialHairChanged(int oldValue, int newValue)
    {
        if (newValue != oldValue)
        {
            if(oldValue > -1) facialHair[oldValue].SetActive(false);
            if(newValue > -1) facialHair[newValue].SetActive(true);
            currentFacialHair = newValue;
            
            if(!sparks.isPlaying && oldValue != -2) sparks.Play();
        }
    }
    
    public void OnBackpackChanged(int oldValue, int newValue)
    {
        if (newValue != oldValue)
        {
            if(oldValue > -1) backpacks[oldValue].SetActive(false);
            if(newValue > -1) backpacks[newValue].SetActive(true);
            currentBackpack = newValue;
            
            if(!sparks.isPlaying && oldValue != -2) sparks.Play();
        }
    }
    
    public void OnSkinToneChanged(int oldValue, int newValue)
    {
        if (newValue != oldValue)
        {
            bodyRenderer.material.SetTexture("_BaseMap", skinTones[newValue]);
            currentSkinTone = newValue;
            
            if(!sparks.isPlaying && oldValue != -2) sparks.Play();
        }
    }
}
