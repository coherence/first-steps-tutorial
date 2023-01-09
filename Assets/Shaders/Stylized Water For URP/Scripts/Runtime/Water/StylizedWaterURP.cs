//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━																												
// Copyright 2020, Alexander Ameye, All rights reserved.
// https://alexander-ameye.gitbook.io/stylized-water/
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━	

#if UNIVERSAL_RENDERER
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering;

namespace StylizedWater
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter)), ExecuteAlways, AddComponentMenu("Effects/Stylized Water")]
    [HelpURL("https://alexander-ameye.gitbook.io/stylized-water/")]
    public class StylizedWaterURP : MonoBehaviour
    {
        #region Inspector Sections
        public bool refractionExpanded;
        public bool vertexPaintingExpanded;
        public bool surfaceFoamExpanded;
        public bool foamShadowsExpanded;
        public bool intersectionEffectsExpanded;
        public bool planarReflectionsExpanded;
        #endregion

        private const string shaderName = "Stylized Water";
        private const string mobileShaderName = "Stylized Water Mobile";
        private const string underwaterShaderName = "Stylized Water Underwater";

        #region Planar Reflections
        [Range(0f, 1f)] public float reflectionStrength = 0f;
        [Range(0f, 7f)] public float reflectionFresnel = 1f;
        #endregion
        
        #region Colors and Transparency
        public bool useColorGradient;
        [GradientUsage(true)] public Gradient colorGradient;
        public Texture2D colorGradientTexture;
        [ColorUsage(true, true)] public Color shallowColor;
        [ColorUsage(true, true)] public Color deepColor;
        [Range(0f, 4f)] public float colorDepth;
        [ColorUsage(true, true)] public Color horizonColor;
        [Range(0f, 20f)] public float horizonDistance;
        [Range(0f, 1f)] public float shoreFade;
        [ColorUsage(false, false)] public Color shoreColor;
        [Range(0f, 1f)] public float shoreStrength;
        [Range(0f, 1f)] public float shoreBlend;
        [Range(0f, 3f)] public float shoreDepth;
        [ColorUsage(false, false)] public Color waveColor;
        #endregion

        #region Underwater Effects
        [ColorUsage(false, false)] public Color underwaterColor;
        [Range(0f, 1f)] public float underwaterColorStrength;
        [Range(0f, 0.3f)] public float underwaterRefractionStrength;
        #endregion

        #region Surface Foam
        public Texture surfaceFoamTexture;
        public Vector2 surfaceFoamSampling;
        [Range(0f, 1f)] public float surfaceFoamCutoff;
        [Range(0f, 3f)] public float surfaceFoamDistortion;
        [Range(0f, 1f)] public float surfaceFoamBlend;
        [ColorUsage(true, false)] public Color surfaceFoamColor1;
        [ColorUsage(true, false)] public Color surfaceFoamColor2;
        public Vector4 surfaceFoamMovement;
        [Range(0f, 1f)] public float surfaceFoamDirection1;
        [Range(0f, 1f)] public float surfaceFoamDirection2;
        [Range(0f, 2f)] public float surfaceFoamSpeed1;
        [Range(0f, 2f)] public float surfaceFoamSpeed2;
        public Vector4 surfaceFoamTilingAndOffset;
        [Range(0f, 1f)] public float surfaceFoamOffsetX;
        [Range(0f, 1f)] public float surfaceFoamOffsetY;
        [Range(0f, 2f)] public float surfaceFoamScale1;
        [Range(0f, 2f)] public float surfaceFoamScale2;
        public bool enableSurfaceFoam;
        [Range(0f, 1f)] public float surfaceFoamHeightMask;
        [Range(0f, 1f)] public float surfaceFoamHeightMaskSmoothness;
        public bool enableHeightMask;
        #endregion

        #region Foam Shadows
        [Range(0f, 1f)] public float foamShadowStrength;
        [Range(0f, 10f)] public float foamShadowDepth;
        [Range(0f, 15f)] public float surfaceFoamShadowProjection;
        [Range(0f, 15f)] public float intersectionFoamShadowProjection;
        #endregion

        #region Intersection Effects
        public enum FoamMovement { Directional, Shore };
        public FoamMovement foamMovement;
        [Range(0f, 3f)] public float intersectionFoamDepth;
        public Texture intersectionFoamTexture;
        [ColorUsage(true, false)] public Color intersectionFoamColor;
        public Vector2 intersectionFoamMovement;
        [Range(0f, 1f)] public float intersectionFoamDirection;
        [Range(0f, 2f)] public float intersectionFoamSpeed;
        [Range(0f, 2f)] public float intersectionFoamScale;
        public Vector2 intersectionFoamSampling;
        [Range(0f, 1f)] public float intersectionFoamCutoff;
        [Range(0f, 3f)] public float intersectionFoamDistortion;
        public bool enableIntersectionEffects;
        [Range(0f, 1f)] public float intersectionFoamBlend;
        [Range(0f, 1f)] public float intersectionWaterBlend;
        #endregion

        #region Shore Foam
        [Range(-5f, 5f)] public float shoreFoamSpeed;
        [Range(0f, 2f)] public float shoreFoamWidth;
        [Range(0f, 40f)] public float shoreFoamFrequency;
        [Range(0f, 5f)] public float shoreFoamBreakupStrength;
        [Range(0f, 50f)] public float shoreFoamBreakupScale;
        #endregion

        #region Lighting
        public enum Lighting { Enabled, Disabled };
        public Lighting lighting;
        [Range(0f, 1f)] public float lightingSmoothness;
        [ColorUsage(false, true)] public Color lightingSpecularColor;
        [ColorUsage(false, true)] public Color lightingDiffuseColor;
        [Range(0f, 1f)] public float lightingHardness;
        #endregion

        #region Surface
        public Texture normalsTexture;
        public Vector2 normalsMovement;
        [Range(0f, 1f)] public float normalsStrength;
        [Range(0.01f, 2f)] public float normalsScale;
        [Range(0f, 0.3f)] public float normalsSpeed;
        #endregion

        #region Refraction
        [Range(0f, 0.3f)] public float refractionStrength;
        #endregion

        #region Waves
        public Vector3 waveVisuals;
        [Range(0f, 0.5f)] public float waveSteepness;
        [Range(0f, 20f)] public float waveLength;
        [Range(0f, 3f)] public float waveSpeed;

        public Vector4 waveDirections;
        [Range(0f, 1f)] public float waveDirection1;
        [Range(0f, 1f)] public float waveDirection2;
        [Range(0f, 1f)] public float waveDirection3;
        [Range(0f, 1f)] public float waveDirection4;
        #endregion

        #region Additional Settings
        public enum WaterUVs { Local, World };
        public WaterUVs waterUVs;
        public bool hideComponents = false;
        public bool enableFoamShadows, enableRefraction;
        #endregion

        public MeshRenderer meshRenderer;
        public MeshFilter meshFilter;
        public Material material;

        void OnEnable()
        {
            if (!meshRenderer) meshRenderer = this.GetComponent<MeshRenderer>();
            if (!meshFilter) meshFilter = this.GetComponent<MeshFilter>();

            material = meshRenderer.sharedMaterial;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;

            if (material && meshRenderer && meshFilter && !Application.isPlaying)
            {
                meshRenderer.sharedMaterial.hideFlags = (hideComponents) ? HideFlags.HideInInspector : HideFlags.None;
                meshRenderer.hideFlags = (hideComponents) ? HideFlags.HideInInspector : HideFlags.None;
                meshFilter.hideFlags = (hideComponents) ? HideFlags.HideInInspector : HideFlags.None;
            }

            this.gameObject.layer = 4;

            ReadMaterialProperties();
            WriteMaterialProperties();
        }

        void Reset() => OnEnable();

        private void ResetHideFlags()
        {
            if (material && meshRenderer && meshFilter && !Application.isPlaying)
            {
                meshRenderer.sharedMaterial.hideFlags = HideFlags.None;
                meshRenderer.hideFlags = HideFlags.None;
                meshFilter.hideFlags = HideFlags.None;
            }
        }

        private void OnDisable() => ResetHideFlags();

        private void OnDestroy() => ResetHideFlags();

        void SafeDestroyObject(UnityEngine.Object obj)
        {
            if (Application.isEditor) DestroyImmediate(obj);
            else Destroy(obj);
        }

        public void ReadMaterialProperties()
        {
            if (meshRenderer) material = meshRenderer.sharedMaterial;
            if (!material) return;
            if (material.shader.name != shaderName && material.shader.name != mobileShaderName && material.shader.name != underwaterShaderName) return;

            enableRefraction = (material.IsKeywordEnabled("REFRACTION_ON")) ? true : false;

            #region Waves
            waveVisuals = material.GetVector("_WaveVisuals");
            waveSteepness = waveVisuals.x;
            waveLength = waveVisuals.y;
            waveSpeed = waveVisuals.z;
            waveDirections = material.GetVector("_WaveDirections");
            waveDirection1 = waveDirections.x;
            waveDirection2 = waveDirections.y;
            waveDirection3 = waveDirections.z;
            waveDirection4 = waveDirections.w;
            #endregion

            #region Refraction
            refractionStrength = material.GetFloat("_RefractionStrength");
            #endregion

            normalsMovement = material.GetVector("_NormalsMovement");
            normalsSpeed = normalsMovement.x;
            normalsScale = normalsMovement.y;

            if (material.shader.name == shaderName || material.shader.name == underwaterShaderName) underwaterColor = material.GetColor("_WaterColorUnderwater");

            if (material.shader.name == shaderName || material.shader.name == mobileShaderName)
            {
                #region Colors and Transparency
                shallowColor = material.GetColor("_WaterColorShallow");
                deepColor = material.GetColor("_WaterColorDeep");
                horizonColor = material.GetColor("_WaterColorHorizon");
                colorDepth = material.GetFloat("_WaterColorDepth");
                horizonDistance = material.GetFloat("_WaterColorHorizonDistance");
                waveColor = material.GetColor("_WaveColor");
                #endregion

                #region Shore Color
                shoreColor = material.GetColor("_ShoreColor");
                shoreDepth = material.GetFloat("_ShoreDepth");
                shoreFade = material.GetFloat("_ShoreFade");
                shoreBlend = material.GetFloat("_ShoreBlend");
                #endregion

                #region Surface Foam
                surfaceFoamTexture = material.GetTexture("_SurfaceFoamTexture");
                surfaceFoamSampling = material.GetVector("_SurfaceFoamSampling");
                surfaceFoamCutoff = surfaceFoamSampling.x;
                surfaceFoamDistortion = surfaceFoamSampling.y;
                surfaceFoamBlend = material.GetFloat("_SurfaceFoamBlend");
                surfaceFoamColor1 = material.GetColor("_SurfaceFoamColor1");
                surfaceFoamColor2 = material.GetColor("_SurfaceFoamColor2");
                surfaceFoamMovement = material.GetVector("_SurfaceFoamMovement");
                surfaceFoamDirection1 = surfaceFoamMovement.x;
                surfaceFoamDirection2 = surfaceFoamMovement.z;
                surfaceFoamSpeed1 = surfaceFoamMovement.y;
                surfaceFoamSpeed2 = surfaceFoamMovement.w;
                surfaceFoamTilingAndOffset = material.GetVector("_SurfaceFoamTilingAndOffset");
                surfaceFoamOffsetX = surfaceFoamTilingAndOffset.x;
                surfaceFoamOffsetY = surfaceFoamTilingAndOffset.y;
                surfaceFoamScale1 = surfaceFoamTilingAndOffset.z;
                surfaceFoamScale2 = surfaceFoamTilingAndOffset.w;
                enableSurfaceFoam = (material.IsKeywordEnabled("SURFACE_FOAM_ON")) ? true : false;
                #endregion

                if (material.IsKeywordEnabled("WORLD_SPACE_UV_ON")) waterUVs = WaterUVs.World;
                else waterUVs = WaterUVs.Local;
            }

            if (material.shader.name == shaderName)
            {
                #region Colors and Transparency
                useColorGradient = (material.IsKeywordEnabled("COLOR_GRADIENT_ON")) ? true : false;
                colorGradientTexture = (Texture2D)material.GetTexture("_WaterColorGradientTexture");
                #endregion

                #region Underwater Effects
                underwaterRefractionStrength = material.GetFloat("_UnderwaterRefractionStrength");
                #endregion

                #region Foam Shadows
                foamShadowStrength = material.GetFloat("_FoamShadowStrength");
                foamShadowDepth = material.GetFloat("_FoamShadowDepth");
                surfaceFoamShadowProjection = material.GetFloat("_SurfaceFoamShadowProjection");
                intersectionFoamShadowProjection = material.GetFloat("_IntersectionFoamShadowProjection");
                #endregion

                #region Surface Foam
                surfaceFoamHeightMaskSmoothness = material.GetFloat("_SurfaceFoamHeightMaskSmoothness");
                surfaceFoamHeightMask = material.GetFloat("_SurfaceFoamHeightMask");
                #endregion

                #region Intersection Foam
                intersectionFoamBlend = material.GetFloat("_IntersectionFoamBlend");
                intersectionWaterBlend = material.GetFloat("_IntersectionWaterBlend");
                intersectionFoamDepth = material.GetFloat("_IntersectionFoamDepth");
                intersectionFoamTexture = material.GetTexture("_IntersectionFoamTexture");
                intersectionFoamColor = material.GetColor("_IntersectionFoamColor");
                intersectionFoamMovement = material.GetVector("_IntersectionFoamMovement");
                intersectionFoamDirection = intersectionFoamMovement.x;
                intersectionFoamSpeed = intersectionFoamMovement.y;
                intersectionFoamScale = material.GetFloat("_IntersectionFoamScale");
                intersectionFoamSampling = material.GetVector("_IntersectionFoamSampling");
                intersectionFoamCutoff = intersectionFoamSampling.x;
                intersectionFoamDistortion = intersectionFoamSampling.y;
                enableIntersectionEffects = (material.IsKeywordEnabled("INTERSECTION_EFFECTS_ON")) ? true : false;
                #endregion

                #region Shore Foam
                shoreFoamSpeed = material.GetFloat("_ShoreFoamSpeed");
                shoreFoamWidth = material.GetFloat("_ShoreFoamWidth");
                shoreFoamFrequency = material.GetFloat("_ShoreFoamFrequency");
                shoreFoamBreakupScale = material.GetFloat("_ShoreFoamBreakupScale");
                shoreFoamBreakupStrength = material.GetFloat("_ShoreFoamBreakupStrength");
                #endregion

                #region Lighting
                if (material.IsKeywordEnabled("WATER_LIGHTING_ON")) lighting = Lighting.Enabled;
                else lighting = Lighting.Disabled;

                if (material.IsKeywordEnabled("SHORE_MOVEMENT_ON")) foamMovement = FoamMovement.Shore;
                else foamMovement = FoamMovement.Directional;

                lightingSmoothness = material.GetFloat("_LightingSmoothness");
                lightingSpecularColor = material.GetColor("_LightingSpecularColor");
                lightingDiffuseColor = material.GetColor("_LightingDiffuseColor");
                lightingHardness = material.GetFloat("_LightingHardness");
                #endregion

                #region Surface
                normalsTexture = material.GetTexture("_NormalsTexture");
                normalsStrength = material.GetFloat("_NormalsStrength");
                #endregion

                #region Planar Reflections
                reflectionStrength = material.GetFloat("_PlanarReflectionStrength");
                reflectionFresnel = material.GetFloat("_PlanarReflectionFresnel");
                #endregion

                enableFoamShadows = (material.IsKeywordEnabled("FOAM_SHADOWS_ON")) ? true : false;
            }
        }

        public void WriteMaterialProperties()
        {
            if (!material) return;
            if (material.shader.name != shaderName && material.shader.name != mobileShaderName && material.shader.name != underwaterShaderName) return;

            if (material && meshRenderer && meshFilter && meshRenderer.sharedMaterial && !Application.isPlaying)
            {
                meshRenderer.sharedMaterial.hideFlags = (hideComponents) ? HideFlags.HideInInspector : HideFlags.None;
                meshRenderer.hideFlags = (hideComponents) ? HideFlags.HideInInspector : HideFlags.None;
                meshFilter.hideFlags = (hideComponents) ? HideFlags.HideInInspector : HideFlags.None;
            }

            #region Colors and Transparency
            if (useColorGradient) material.EnableKeyword("COLOR_GRADIENT_ON");
            else material.DisableKeyword("COLOR_GRADIENT_ON");
            material.SetTexture("_WaterColorGradientTexture", colorGradientTexture);
            SetColorIfDifferent("_WaterColorShallow", shallowColor);
            SetColorIfDifferent("_WaterColorHorizon", horizonColor);
            SetFloatIfDifferent("_WaterColorDepth", colorDepth);
            SetFloatIfDifferent("_WaterColorHorizonDistance", horizonDistance);
            SetColorIfDifferent("_WaveColor", waveColor);
            SetColorIfDifferent("_WaterColorDeep", deepColor);
            #endregion

            #region Underwater Effects
            underwaterColor.a = underwaterColorStrength;
            SetColorIfDifferent("_WaterColorUnderwater", underwaterColor);
            SetFloatIfDifferent("_UnderwaterRefractionStrength", underwaterRefractionStrength);
            #endregion

            #region Shore Color
            shoreColor.a = shoreStrength;
            SetColorIfDifferent("_ShoreColor", shoreColor);
            SetFloatIfDifferent("_ShoreDepth", shoreDepth);
            SetFloatIfDifferent("_ShoreFade", shoreFade);
            SetFloatIfDifferent("_ShoreBlend", shoreBlend);
            #endregion

            #region Foam Shadows
            SetFloatIfDifferent("_FoamShadowStrength", foamShadowStrength);
            SetFloatIfDifferent("_FoamShadowDepth", foamShadowDepth);
            SetFloatIfDifferent("_SurfaceFoamShadowProjection", surfaceFoamShadowProjection);
            SetFloatIfDifferent("_IntersectionFoamShadowProjection", intersectionFoamShadowProjection);
            #endregion

            #region Surface Foam
            material.SetTexture("_SurfaceFoamTexture", surfaceFoamTexture);
            SetVectorIfDifferent("_SurfaceFoamSampling", new Vector2(surfaceFoamCutoff, surfaceFoamDistortion));
            SetFloatIfDifferent("_SurfaceFoamBlend", surfaceFoamBlend);
            SetColorIfDifferent("_SurfaceFoamColor1", surfaceFoamColor1);
            SetColorIfDifferent("_SurfaceFoamColor2", surfaceFoamColor2);
            SetVectorIfDifferent("_SurfaceFoamMovement", new Vector4(surfaceFoamDirection1, surfaceFoamSpeed1, surfaceFoamDirection2, surfaceFoamSpeed2));
            SetVectorIfDifferent("_SurfaceFoamTilingAndOffset", new Vector4(surfaceFoamOffsetX, surfaceFoamOffsetY, surfaceFoamScale1, surfaceFoamScale2));
            if (enableSurfaceFoam) material.EnableKeyword("SURFACE_FOAM_ON");
            else material.DisableKeyword("SURFACE_FOAM_ON");
            if (enableHeightMask)
            {
                SetFloatIfDifferent("_SurfaceFoamHeightMask", surfaceFoamHeightMask);
                SetFloatIfDifferent("_SurfaceFoamHeightMaskSmoothness", surfaceFoamHeightMaskSmoothness);
            }
            else
            {
                SetFloatIfDifferent("_SurfaceFoamHeightMask", 0f);
                SetFloatIfDifferent("_SurfaceFoamHeightMaskSmoothness", 0f);
            }
            #endregion

            #region Intersection Foam
            SetFloatIfDifferent("_IntersectionFoamBlend", intersectionFoamBlend);
            SetFloatIfDifferent("_IntersectionWaterBlend", intersectionWaterBlend);
            SetFloatIfDifferent("_IntersectionFoamDepth", intersectionFoamDepth);
            material.SetTexture("_IntersectionFoamTexture", intersectionFoamTexture);
            SetColorIfDifferent("_IntersectionFoamColor", intersectionFoamColor);
            SetFloatIfDifferent("_IntersectionFoamScale", intersectionFoamScale);
            SetVectorIfDifferent("_IntersectionFoamMovement", new Vector2(intersectionFoamDirection, intersectionFoamSpeed));
            SetVectorIfDifferent("_IntersectionFoamSampling", new Vector2(intersectionFoamCutoff, intersectionFoamDistortion));
            if (enableIntersectionEffects) material.EnableKeyword("INTERSECTION_EFFECTS_ON");
            else material.DisableKeyword("INTERSECTION_EFFECTS_ON");
            #endregion

            #region Shore Foam
            SetFloatIfDifferent("_ShoreFoamSpeed", shoreFoamSpeed);
            SetFloatIfDifferent("_ShoreFoamWidth", shoreFoamWidth);
            SetFloatIfDifferent("_ShoreFoamFrequency", shoreFoamFrequency);
            SetFloatIfDifferent("_ShoreFoamBreakupStrength", shoreFoamBreakupStrength);
            SetFloatIfDifferent("_ShoreFoamBreakupScale", shoreFoamBreakupScale);
            #endregion

            #region Waves
            SetVectorIfDifferent("_WaveVisuals", new Vector3(waveSteepness, waveLength, waveSpeed));
            SetVectorIfDifferent("_WaveDirections", new Vector4(waveDirection1, waveDirection2, waveDirection3, waveDirection4));
            #endregion

            if (waterUVs == WaterUVs.World) material.EnableKeyword("WORLD_SPACE_UV_ON");
            else material.DisableKeyword("WORLD_SPACE_UV_ON");

            #region Lighting
            if (lighting == Lighting.Enabled) material.EnableKeyword("WATER_LIGHTING_ON");
            else material.DisableKeyword("WATER_LIGHTING_ON");

            if (foamMovement == FoamMovement.Shore) material.EnableKeyword("SHORE_MOVEMENT_ON");
            else material.DisableKeyword("SHORE_MOVEMENT_ON");

            SetFloatIfDifferent("_LightingSmoothness", lightingSmoothness);
            SetColorIfDifferent("_LightingSpecularColor", lightingSpecularColor);
            SetColorIfDifferent("_LightingDiffuseColor", lightingDiffuseColor);
            SetFloatIfDifferent("_LightingHardness", lightingHardness);
            #endregion

            #region Surface
            material.SetTexture("_NormalsTexture", normalsTexture);
            SetVectorIfDifferent("_NormalsMovement", new Vector2(normalsSpeed, normalsScale));
            SetFloatIfDifferent("_NormalsStrength", normalsStrength);
            #endregion

            #region Planar Reflections
            SetFloatIfDifferent("_PlanarReflectionStrength", reflectionStrength);

            SetFloatIfDifferent("_PlanarReflectionFresnel", reflectionFresnel);
            #endregion

            #region Refraction
            SetFloatIfDifferent("_RefractionStrength", refractionStrength);
            if (material.shader.name == mobileShaderName || material.shader.name == underwaterShaderName)
            {
                if (enableRefraction) material.EnableKeyword("REFRACTION_ON");
                else material.DisableKeyword("REFRACTION_ON");
            }
            #endregion

            if (enableFoamShadows) material.EnableKeyword("FOAM_SHADOWS_ON");
            else material.DisableKeyword("FOAM_SHADOWS_ON");
        }
        
        private void SetVectorIfDifferent(string propertyName, Vector4 newVectorValue)
        {
            if(!(material.GetVector(propertyName) == newVectorValue))
                material.SetVector(propertyName, newVectorValue);
        }

        private void SetColorIfDifferent(string propertyName, Color newColorValue)
        {
            if(!(material.GetColor(propertyName) == newColorValue))
                material.SetColor(propertyName, newColorValue);
        }

        private void SetFloatIfDifferent(string propertyName, float newFloatValue)
        {
            if(!Mathf.Approximately(material.GetFloat(propertyName), newFloatValue))
                material.SetFloat(propertyName, newFloatValue);
        }

        public float GetWaveSteepness() => waveSteepness;

        public float GetWaveLength() => waveLength;

        public float GetWaveSpeed() => waveSpeed;

        public float[] GetWaveDirections() => new float[] { waveDirection1, waveDirection2, waveDirection3, waveDirection4 };
    }
}
#endif