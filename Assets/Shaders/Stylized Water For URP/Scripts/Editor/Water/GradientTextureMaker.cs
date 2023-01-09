//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━																												
// Copyright 2020, Alexander Ameye, All rights reserved.
// https://alexander-ameye.gitbook.io/stylized-water/
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━	

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

namespace StylizedWater
{
    public static class GradientTextureMaker
    {
        public static int width = 128;
        public static int height = 4; // needs to be multiple of 4 for DXT1 format compression

        public static Texture2D CreateGradientTexture(Material targetMaterial, Gradient gradient)
        {
            Texture2D gradientTexture = new Texture2D(width, height, TextureFormat.ARGB32, false, false)
            {
                name = "_gradient",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++) gradientTexture.SetPixel(i, j, gradient.Evaluate((float)i / (float)width));
            }

            gradientTexture.Apply(false);
            gradientTexture = SaveAndGetTexture(targetMaterial, gradientTexture);
            return gradientTexture;
        }

        private static Texture2D SaveAndGetTexture(Material targetMaterial, Texture2D sourceTexture)
        {
            string targetFolder = AssetDatabase.GetAssetPath(targetMaterial);
            targetFolder = targetFolder.Replace(targetMaterial.name + ".mat", string.Empty);

            targetFolder += "Gradient Textures/";

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
                AssetDatabase.Refresh();
            }

            string path = targetFolder + targetMaterial.name + sourceTexture.name + ".png";
            File.WriteAllBytes(path, sourceTexture.EncodeToPNG());
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.Default);
            sourceTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
            return sourceTexture;
        }
    }
}