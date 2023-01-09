using UnityEditor;
using UnityEngine;

/// <summary>
/// This AssetPostProcessor acts on all directional lightmap textures, and fixes the black pixels in the alpha
/// (aka pixels with "no direction"), a product of invalid texels during Lightmap baking.
/// As it's a PostProcessor, the original files on disk shouldn't be modified, only the artifacts in the Library.
/// </summary>
public class DirectionalLightmapperFix : AssetPostprocessor
{
    private void OnPostprocessTexture(Texture2D texture)
    {
        bool hasProcessedAnything = false;
        
        if (!(assetPath.Contains("Lightmap-") && assetPath.Contains("_comp_dir")))
            return;
        
        Color[] colors = texture.GetPixels();

        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].a <= .1f)
            {
                colors[i].a = .1f;
                hasProcessedAnything = true;
            }
        }

        if (hasProcessedAnything)
        {
            texture.SetPixels(colors);
            texture.Apply(true);
            
            Debug.Log("Corrected Directional Lightmap in " + assetPath);
        }
    }
}

