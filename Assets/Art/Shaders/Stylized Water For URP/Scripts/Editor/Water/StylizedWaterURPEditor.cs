//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━																												
// Copyright 2020, Alexander Ameye, All rights reserved.
// https://alexander-ameye.gitbook.io/stylized-water/
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━	

#if UNIVERSAL_RENDERER
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Reflection;

namespace StylizedWater
{
    [CustomEditor(typeof(StylizedWaterURP))]
    public class StylizedWaterURPEditor : Editor
    {
        StylizedWaterURP stylizedWater;
        new SerializedObject serializedObject;
        GameObject selected;

        public bool lightingDisabled;
        public bool shoreMovementEnabled;
        public bool usesReflections;
        public bool usesColorGradient;
        public bool surfaceFoamEnabled;
        public bool intersectionEffectsEnabled;
        public bool foamShadowsEnabled;
        public bool refractionEnabled;
        public bool usesWorldSpaceUVs;

        private const string shaderName = "Stylized Water";
        private const string mobileShaderName = "Stylized Water Mobile";
        private const string underwaterShaderName = "Stylized Water Underwater";

        #region Colors and Transparency
        SerializedProperty useColorGradient, colorGradient,
                           shallowColor, deepColor, colorDepth,
                           horizonColor, horizonDistance;
        SerializedProperty waveColor;
        #endregion

        #region Shore Color
        SerializedProperty shoreFade, shoreColor, shoreDepth, shoreBlend;
        #endregion
        
        #region Intersection Effects
        SerializedProperty intersectionFoamBlend, intersectionWaterBlend, intersectionFoamColor, intersectionFoamDirection, intersectionFoamScale, intersectionFoamSpeed,
        intersectionFoamCutoff, intersectionFoamDistortion,
        intersectionFoamTexture, intersectionFoamDepth, foamMovement;
        SerializedProperty shoreStrength;
        SerializedProperty enableIntersectionEffects;
        #endregion

        #region Foam Shadows
        SerializedProperty enableFoamShadows, enableRefraction, foamShadowStrength, foamShadowDepth, intersectionFoamShadowProjection, surfaceFoamShadowProjection;
        #endregion

        #region Shore Foam
        SerializedProperty shoreFoamSpeed, shoreFoamWidth, shoreFoamFrequency, shoreFoamBreakupStrength, shoreFoamBreakupScale;
        #endregion

        #region Surface Foam
        SerializedProperty enableSurfaceFoam, surfaceFoamTexture, surfaceFoamBlend,
                         surfaceFoamColor1, surfaceFoamColor2,
                         surfaceFoamMovement, surfaceFoamSampling;
        SerializedProperty surfaceFoamHeightMask, surfaceFoamHeightMaskSmoothness, enableHeightMask;
        SerializedProperty surfaceFoamOffsetX, surfaceFoamOffsetY, surfaceFoamScale1, surfaceFoamScale2;
        SerializedProperty surfaceFoamDirection1, surfaceFoamDirection2, surfaceFoamSpeed1, surfaceFoamSpeed2;
        SerializedProperty surfaceFoamCutoff, surfaceFoamDistortion;
        #endregion

        #region Underwater Effects
        SerializedProperty underwaterColor, underwaterColorStrength, underwaterRefractionStrength;
        #endregion

        #region Surface and Lighting
        SerializedProperty lighting,
                           lightingSmoothness, lightingHardness,
                           lightingSpecularColor, lightingDiffuseColor;
        SerializedProperty normalsTexture, surfaceNormals;
        SerializedProperty normalsStrength, normalsScale, normalsSpeed;
        #endregion

        #region Refraction
        SerializedProperty refractionStrength;
        #endregion

        #region Planar Reflections
        SerializedProperty reflectionStrength, reflectionFresnel;
        #endregion

        #region Sections
        SerializedProperty surfaceFoamExpanded, intersectionEffectsExpanded, foamShadowsExpanded, refractionExpanded, planarReflectionsExpanded;
        #endregion

        #region Waves
        SerializedProperty waveDirection1, waveDirection2, waveDirection3, waveDirection4;
        SerializedProperty waveSteepness, waveLength, waveSpeed;
        #endregion

        #region Additional Settings
        SerializedProperty hideComponents, waterUVs;
        #endregion

        #region Section Foldouts
        private static bool colorTransparencySettings,
             surfaceFoamSettings,
             intersectionFoamSettings,
             surfaceSettings,
             refractionSettings,
             planarReflectionSettings,
             waveSettings,
             underwaterSettings,
             additionalSettings;
        public delegate void DrawSettingsMethod();
        #endregion

        [MenuItem("GameObject/3D Object/Stylized Water/Square", priority = 7)]
        static void CreateSquareWater() => InstantiateWater("Square Water");

        [MenuItem("GameObject/3D Object/Stylized Water/Hexagonal", priority = 7)]
        static void CreateHexagonalWater() => InstantiateWater("Hexagonal Water");

        [MenuItem("GameObject/3D Object/Stylized Water/Circular", priority = 7)]
        static void CreateCircularWater() => InstantiateWater("Circular Water");

        static void InstantiateWater(string name)
        {
            string[] guids = AssetDatabase.FindAssets($"t:Prefab " + name);
            if (guids.Length == 0)
            {
                Debug.Log("Error: water prefab not found");
                return;
            }

            else
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[0]));
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                Undo.RegisterCreatedObjectUndo(instance, "Create Water Object");
                Selection.activeObject = instance;
                SceneView.FrameLastActiveSceneView();
            }
        }

        public void OnEnable()
        {
            selected = Selection.activeGameObject;

            if (!selected) return;
            if (!stylizedWater) stylizedWater = selected.GetComponent<StylizedWaterURP>();
            if (stylizedWater)
            {
                serializedObject = new SerializedObject(stylizedWater);
                GetWaterProperties();
            }
            Undo.undoRedoPerformed += ApplyChanges;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            stylizedWater.ReadMaterialProperties();
            EditorGUI.BeginChangeCheck();
            DrawSections();
            if (EditorGUI.EndChangeCheck()) ApplyChanges();
        }

        void ApplyChanges()
        {
            if (serializedObject.targetObject) serializedObject.ApplyModifiedProperties();
            stylizedWater.WriteMaterialProperties();
            GetWaterProperties();
            stylizedWater.ReadMaterialProperties();
        }

        void DrawSections()
        {
            if (!stylizedWater.meshRenderer || !stylizedWater.meshRenderer.sharedMaterial || !stylizedWater.meshRenderer.enabled)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Object has no active mesh renderer and/or material. Please add those first.", MessageType.Warning);
                EditorGUILayout.Space();
                return;
            }

            string name = stylizedWater.meshRenderer.sharedMaterial.shader.name;
            if (name != shaderName && name != mobileShaderName && name != underwaterShaderName)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Material incompatible. You should add a material that uses the Stylized Water shader.", MessageType.Warning);
                EditorGUILayout.Space();
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            switch (name)
            {
                case shaderName: EditorGUILayout.LabelField("   " + stylizedWater.material.name); break;
                case mobileShaderName: EditorGUILayout.LabelField("   " + stylizedWater.material.name + " (Mobile Variant)"); break;
                case underwaterShaderName: EditorGUILayout.LabelField("   " + stylizedWater.material.name + " (Underwater Variant)"); break;
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            CoreEditorUtils.DrawSplitter();

            if (name == shaderName || name == mobileShaderName)
            {
                colorTransparencySettings = CoreEditorUtils.DrawHeaderFoldout("Colors and Transparency", colorTransparencySettings, false, (Func<bool>)null, null);
                DrawPropertiesInspector(colorTransparencySettings, DrawColorSettings);

                DrawPropertiesInspector(CoreEditorUtils.DrawHeaderToggle(EditorGUIUtility.TrTextContent("Surface Foam"), surfaceFoamExpanded, enableSurfaceFoam, null), DrawSurfaceFoamSettings);

                DrawPropertiesInspector(CoreEditorUtils.DrawHeaderToggle(EditorGUIUtility.TrTextContent("Intersection Effects"), intersectionEffectsExpanded, enableIntersectionEffects, null), DrawIntersectionEffectSettings);
            }

            if (name == shaderName)
            {
                DrawPropertiesInspector(CoreEditorUtils.DrawHeaderToggle(EditorGUIUtility.TrTextContent("Foam Shadows"), foamShadowsExpanded, enableFoamShadows, null), DrawFoamShadowSettings);
                
                planarReflectionSettings = CoreEditorUtils.DrawHeaderFoldout("Planar Reflections", planarReflectionSettings, false, (Func<bool>)null, null);
                DrawPropertiesInspector(planarReflectionSettings, DrawPlanarReflectionSettings);

                surfaceSettings = CoreEditorUtils.DrawHeaderFoldout("Surface and Lighting", surfaceSettings, false, (Func<bool>)null, null);
                DrawPropertiesInspector(surfaceSettings, DrawSurfaceSettings);
            }

            if (name == mobileShaderName || name == underwaterShaderName)
            {
                DrawPropertiesInspector(CoreEditorUtils.DrawHeaderToggle(EditorGUIUtility.TrTextContent("Refraction"), refractionExpanded, enableRefraction, null), DrawRefractionSettings);
            }

            if (name == shaderName || name == mobileShaderName)
            {
                waveSettings = CoreEditorUtils.DrawHeaderFoldout("Waves", waveSettings, false, (Func<bool>)null, null);
                DrawPropertiesInspector(waveSettings, DrawWaveSettings);
            }

            if (name == shaderName || name == underwaterShaderName)
            {
                underwaterSettings = CoreEditorUtils.DrawHeaderFoldout("Underwater", underwaterSettings, false, (Func<bool>)null, null);
                DrawPropertiesInspector(underwaterSettings, DrawUnderwaterSettings);
            }

            additionalSettings = CoreEditorUtils.DrawHeaderFoldout("Additional Settings", additionalSettings, false, (Func<bool>)null, null);
            DrawPropertiesInspector(additionalSettings, DrawAdditionalSettings);

            EditorGUILayout.Space();
            if (stylizedWater.meshRenderer.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.On) CoreEditorUtils.DrawFixMeBox("Water is casting shadows. \nYou should turn this off.", () => TurnOffWaterShadowCasting());
        }

        private void TurnOffWaterShadowCasting() => stylizedWater.meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        private void OnContextClick(Vector2 position, string section)
        {
            var menu = new GenericMenu();
            menu.AddItem(EditorGUIUtility.TrTextContent("Documentation"), false, () => OpenDocumentation(section));
            menu.DropDown(new Rect(position, Vector2.zero));
        }

        private void OpenDocumentation(string link) => Application.OpenURL("https://alexander-ameye.gitbook.io/stylized-water/features/shader-properties" + "#" + link);

        void DrawPropertiesInspector(bool active, DrawSettingsMethod DrawProperties)
        {
            if (active)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                DrawProperties();
                if (EditorGUI.EndChangeCheck()) ApplyChanges();
                EditorGUI.indentLevel--;
            }
            CoreEditorUtils.DrawSplitter();
        }

        void DrawColorSettings()
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Depth", EditorStyles.helpBox);
            if(stylizedWater.meshRenderer.sharedMaterial.shader.name != mobileShaderName) EditorGUILayout.PropertyField(useColorGradient, EditorGUIUtility.TrTextContent("Use Gradient"));

            if (!usesColorGradient)
            {
                EditorGUILayout.PropertyField(shallowColor, EditorGUIUtility.TrTextContent("Shallow"));
                EditorGUILayout.PropertyField(deepColor, EditorGUIUtility.TrTextContent("Deep"));
                EditorGUILayout.PropertyField(colorDepth, EditorGUIUtility.TrTextContent("Depth"));
            }

            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(colorGradient, EditorGUIUtility.TrTextContent("Color"));
                if (GUILayout.Button("Apply")) ApplyColorGradient(stylizedWater);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(colorDepth, EditorGUIUtility.TrTextContent("Depth"));
            }

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("Horizon", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(horizonColor, EditorGUIUtility.TrTextContent("Color"));
            EditorGUILayout.PropertyField(horizonDistance, EditorGUIUtility.TrTextContent("Distance"));

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("Shore", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(shoreColor, EditorGUIUtility.TrTextContent("Color"));
            EditorGUILayout.PropertyField(shoreDepth, EditorGUIUtility.TrTextContent("Depth"));
            EditorGUILayout.PropertyField(shoreStrength, EditorGUIUtility.TrTextContent("Strength"));
            EditorGUILayout.PropertyField(shoreFade, EditorGUIUtility.TrTextContent("Water Fade"));
            EditorGUILayout.PropertyField(shoreBlend, EditorGUIUtility.TrTextContent("Shore Blend"));

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("Waves", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(waveColor, EditorGUIUtility.TrTextContent("Top"));

            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        public static void ApplyColorGradient(StylizedWaterURP water)
        {
            water.colorGradientTexture = GradientTextureMaker.CreateGradientTexture(water.material, water.colorGradient);
            water.material.SetTexture("_ColorGradientTexture", water.colorGradientTexture);
        }

        void DrawSurfaceFoamSettings()
        {
            EditorGUILayout.Space();

            if (!surfaceFoamEnabled)
            {
                EditorGUILayout.HelpBox("Feature disabled.", MessageType.Info);
                EditorGUILayout.Space();
                return;
            }

            EditorGUILayout.LabelField("General", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(surfaceFoamTexture, EditorGUIUtility.TrTextContent("Texture"));
            EditorGUILayout.PropertyField(surfaceFoamCutoff, EditorGUIUtility.TrTextContent("Cutoff"));
            EditorGUILayout.PropertyField(surfaceFoamDistortion, EditorGUIUtility.TrTextContent("Distortion"));
            EditorGUILayout.PropertyField(surfaceFoamBlend, EditorGUIUtility.TrTextContent("Color Blend"));

            if (stylizedWater.meshRenderer.sharedMaterial.shader.name == shaderName)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(enableHeightMask, EditorGUIUtility.TrTextContent("Wave Mask"));
            }

            if (enableHeightMask.boolValue)
            {
                if (waveSteepness.floatValue == 0f)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("Wave steepness is set to zero.", MessageType.Warning);
                }

                else
                {
                    EditorGUILayout.PropertyField(surfaceFoamHeightMask, EditorGUIUtility.TrTextContent("Height"));
                    EditorGUILayout.PropertyField(surfaceFoamHeightMaskSmoothness, EditorGUIUtility.TrTextContent("Blend"));
                }
            }

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("Primary", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(surfaceFoamColor1, EditorGUIUtility.TrTextContent("Color"));
            EditorGUILayout.PropertyField(surfaceFoamScale1, EditorGUIUtility.TrTextContent("Scale"));
            EditorGUILayout.PropertyField(surfaceFoamDirection1, EditorGUIUtility.TrTextContent("Direction"));
            EditorGUILayout.PropertyField(surfaceFoamSpeed1, EditorGUIUtility.TrTextContent("Speed"));

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("Secondary", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(surfaceFoamColor2, EditorGUIUtility.TrTextContent("Color"));
            EditorGUILayout.PropertyField(surfaceFoamScale2, EditorGUIUtility.TrTextContent("Scale"));
            EditorGUILayout.PropertyField(surfaceFoamDirection2, EditorGUIUtility.TrTextContent("Direction"));
            EditorGUILayout.PropertyField(surfaceFoamSpeed2, EditorGUIUtility.TrTextContent("Speed"));

            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;
            CoreEditorUtils.DrawMultipleFields("Offset", new[] { surfaceFoamOffsetX, surfaceFoamOffsetY }, new[] { EditorGUIUtility.TrTextContent("X"), EditorGUIUtility.TrTextContent("Y") });
            EditorGUIUtility.labelWidth = labelWidth;

            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        void DrawIntersectionEffectSettings()
        {
            EditorGUILayout.Space();

            if (!intersectionEffectsEnabled)
            {
                EditorGUILayout.HelpBox("Feature disabled.", MessageType.Info);
                EditorGUILayout.Space();
                return;
            }

            if (stylizedWater.meshRenderer.sharedMaterial.shader.name == shaderName)
            {
                foamMovement.enumValueIndex = EditorGUILayout.Popup(EditorGUIUtility.TrTextContent("Movement"), foamMovement.enumValueIndex, foamMovement.enumDisplayNames);

                if (foamMovement.enumValueIndex == 1)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("Shore movement feature is experimental.", MessageType.Warning);
                }
                EditorGUILayout.Space();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("General", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(intersectionFoamDepth, EditorGUIUtility.TrTextContent("Depth"));
            EditorGUILayout.PropertyField(intersectionFoamColor, EditorGUIUtility.TrTextContent("Color"));
            EditorGUILayout.PropertyField(intersectionFoamBlend, EditorGUIUtility.TrTextContent("Color Blend"));
            EditorGUILayout.PropertyField(intersectionWaterBlend, EditorGUIUtility.TrTextContent("Color Fade"));

            if (foamMovement.enumValueIndex == 1 && stylizedWater.meshRenderer.sharedMaterial.shader.name == shaderName)
            {
                EditorGUILayout.Space(); EditorGUILayout.Space();
                EditorGUILayout.LabelField("Shore", EditorStyles.helpBox);
                EditorGUILayout.PropertyField(shoreFoamSpeed, EditorGUIUtility.TrTextContent("Speed"));
                EditorGUILayout.PropertyField(shoreFoamWidth, EditorGUIUtility.TrTextContent("Width"));
                EditorGUILayout.PropertyField(shoreFoamFrequency, EditorGUIUtility.TrTextContent("Frequency"));
                EditorGUILayout.PropertyField(shoreFoamBreakupScale, EditorGUIUtility.TrTextContent("Breakup Scale"));
                EditorGUILayout.PropertyField(shoreFoamBreakupStrength, EditorGUIUtility.TrTextContent("Breakup Strength"));
            }

            else
            {
                EditorGUILayout.Space(); EditorGUILayout.Space();
                EditorGUILayout.LabelField("Directional", EditorStyles.helpBox);
                EditorGUILayout.PropertyField(intersectionFoamTexture, EditorGUIUtility.TrTextContent("Texture"));
                EditorGUILayout.PropertyField(intersectionFoamCutoff, EditorGUIUtility.TrTextContent("Cutoff"));
                EditorGUILayout.PropertyField(intersectionFoamDistortion, EditorGUIUtility.TrTextContent("Distortion"));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(intersectionFoamScale, EditorGUIUtility.TrTextContent("Scale"));
                EditorGUILayout.PropertyField(intersectionFoamDirection, EditorGUIUtility.TrTextContent("Direction"));
                EditorGUILayout.PropertyField(intersectionFoamSpeed, EditorGUIUtility.TrTextContent("Speed"));
            }

            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        void DrawFoamShadowSettings()
        {
            EditorGUILayout.Space();

            if (!foamShadowsEnabled)
            {
                EditorGUILayout.HelpBox("Feature disabled.", MessageType.Info);
                EditorGUILayout.Space();
                return;
            }

            if (!surfaceFoamEnabled && !intersectionEffectsEnabled)
            {
                EditorGUILayout.HelpBox("Either surface foam or intersection effects need to be enabled in order to display foam shadows.", MessageType.Warning);
                EditorGUILayout.Space();
                return;
            }

            EditorGUILayout.LabelField("General", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(foamShadowStrength, EditorGUIUtility.TrTextContent("Strength"));
            EditorGUILayout.PropertyField(foamShadowDepth, EditorGUIUtility.TrTextContent("Depth"));

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("Projection", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(intersectionFoamShadowProjection, EditorGUIUtility.TrTextContent("Intersection Effects"));
            EditorGUILayout.PropertyField(surfaceFoamShadowProjection, EditorGUIUtility.TrTextContent("Surface Foam"));

            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        void DrawRefractionSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(normalsScale, EditorGUIUtility.TrTextContent("Scale"));
            EditorGUILayout.PropertyField(normalsSpeed, EditorGUIUtility.TrTextContent("Speed"));
            EditorGUILayout.PropertyField(refractionStrength, EditorGUIUtility.TrTextContent("Strength"));
            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        void DrawSurfaceSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Lighting", EditorStyles.helpBox);
            lighting.enumValueIndex = EditorGUILayout.Popup(EditorGUIUtility.TrTextContent("Lighting"), lighting.enumValueIndex, lighting.enumDisplayNames);

            if (!lightingDisabled)
            {
                EditorGUILayout.PropertyField(lightingHardness, EditorGUIUtility.TrTextContent("Hardness"));
                EditorGUILayout.PropertyField(lightingSmoothness, EditorGUIUtility.TrTextContent("Smoothness"));
                EditorGUILayout.PropertyField(lightingSpecularColor, EditorGUIUtility.TrTextContent("Specular"));
                EditorGUILayout.PropertyField(lightingDiffuseColor, EditorGUIUtility.TrTextContent("Diffuse"));
            }

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("Normals", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(normalsTexture, EditorGUIUtility.TrTextContent("Texture"));
            if (!lightingDisabled) EditorGUILayout.PropertyField(normalsStrength, EditorGUIUtility.TrTextContent("Strength"));
            EditorGUILayout.PropertyField(normalsScale, EditorGUIUtility.TrTextContent("Scale"));
            EditorGUILayout.PropertyField(normalsSpeed, EditorGUIUtility.TrTextContent("Speed"));

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("Refraction", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(refractionStrength, EditorGUIUtility.TrTextContent("Strength"));
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Refraction is synced with the normal map. See the Normals section for scale and speed.", MessageType.Info);

            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        void DrawPlanarReflectionSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(reflectionStrength, EditorGUIUtility.TrTextContent("Strength"));
            EditorGUILayout.PropertyField(reflectionFresnel, EditorGUIUtility.TrTextContent("Fresnel Effect"));

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("A planar reflection component should be added to the main camera.", MessageType.Info);
            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        void DrawWaveSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Visuals", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(waveSteepness, EditorGUIUtility.TrTextContent("Steepness"));
            EditorGUILayout.PropertyField(waveLength, EditorGUIUtility.TrTextContent("Scale"));
            EditorGUILayout.PropertyField(waveSpeed, EditorGUIUtility.TrTextContent("Speed"));

            EditorGUILayout.Space();
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;
            CoreEditorUtils.DrawMultipleFields("Directions", new[] { waveDirection1, waveDirection2, waveDirection3, waveDirection4 },
                                                             new[] { EditorGUIUtility.TrTextContent("1"), EditorGUIUtility.TrTextContent("2"), EditorGUIUtility.TrTextContent("3"), EditorGUIUtility.TrTextContent("4") });
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        void DrawUnderwaterSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(underwaterColor, EditorGUIUtility.TrTextContent("Color"));
            EditorGUILayout.PropertyField(underwaterColorStrength, EditorGUIUtility.TrTextContent("Transparency"));
            if (stylizedWater.meshRenderer.sharedMaterial.shader.name == shaderName)
            {
                EditorGUILayout.PropertyField(underwaterRefractionStrength, EditorGUIUtility.TrTextContent("Refraction"));
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Refraction is synced with the normal map. You can set the texture, scale and speed in the 'Surface and Lighting' section.", MessageType.Info);
            }
            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        void DrawAdditionalSettings()
        {
            EditorGUIUtility.labelWidth = 160;
            EditorGUILayout.Space();
            CoreEditorUtils.DrawPopup(EditorGUIUtility.TrTextContent("UV Space"), waterUVs, new string[] { "Local", "World" });
            EditorGUILayout.PropertyField(hideComponents, EditorGUIUtility.TrTextContent("Hide Components"));
            EditorGUILayout.Space(); EditorGUILayout.Space();
        }

        void GetWaterProperties()
        {
            if (!selected) return;

            #region Sections
            surfaceFoamExpanded = serializedObject.FindProperty("surfaceFoamExpanded");
            intersectionEffectsExpanded = serializedObject.FindProperty("intersectionEffectsExpanded");
            foamShadowsExpanded = serializedObject.FindProperty("foamShadowsExpanded");
            refractionExpanded = serializedObject.FindProperty("refractionExpanded");
            planarReflectionsExpanded = serializedObject.FindProperty("planarReflectionsExpanded");
            #endregion

            #region Colors and Transparency
            useColorGradient = serializedObject.FindProperty("useColorGradient");
            colorGradient = serializedObject.FindProperty("colorGradient");
            shallowColor = serializedObject.FindProperty("shallowColor");
            deepColor = serializedObject.FindProperty("deepColor");
            horizonColor = serializedObject.FindProperty("horizonColor");
            colorDepth = serializedObject.FindProperty("colorDepth");
            horizonDistance = serializedObject.FindProperty("horizonDistance");
            waveColor = serializedObject.FindProperty("waveColor");
            #endregion

            #region Underwater
            underwaterColor = serializedObject.FindProperty("underwaterColor");
            underwaterColorStrength = serializedObject.FindProperty("underwaterColorStrength");
            underwaterRefractionStrength = serializedObject.FindProperty("underwaterRefractionStrength");
            #endregion

            #region Shore Color
            shoreColor = serializedObject.FindProperty("shoreColor");
            shoreStrength = serializedObject.FindProperty("shoreStrength");
            shoreDepth = serializedObject.FindProperty("shoreDepth");
            shoreFade = serializedObject.FindProperty("shoreFade");
            shoreBlend = serializedObject.FindProperty("shoreBlend");
            #endregion

            #region Refraction
            refractionStrength = serializedObject.FindProperty("refractionStrength");
            #endregion

            #region Foam Shadows
            foamShadowDepth = serializedObject.FindProperty("foamShadowDepth");
            foamShadowStrength = serializedObject.FindProperty("foamShadowStrength");
            intersectionFoamShadowProjection = serializedObject.FindProperty("intersectionFoamShadowProjection");
            surfaceFoamShadowProjection = serializedObject.FindProperty("surfaceFoamShadowProjection");
            #endregion

            #region Surface Foam
            enableSurfaceFoam = serializedObject.FindProperty("enableSurfaceFoam");
            surfaceFoamTexture = serializedObject.FindProperty("surfaceFoamTexture");
            surfaceFoamSampling = serializedObject.FindProperty("surfaceFoamSampling");
            surfaceFoamCutoff = serializedObject.FindProperty("surfaceFoamCutoff");
            surfaceFoamDistortion = serializedObject.FindProperty("surfaceFoamDistortion");
            surfaceFoamBlend = serializedObject.FindProperty("surfaceFoamBlend");
            intersectionFoamBlend = serializedObject.FindProperty("intersectionFoamBlend");
            intersectionWaterBlend = serializedObject.FindProperty("intersectionWaterBlend");
            surfaceFoamColor1 = serializedObject.FindProperty("surfaceFoamColor1");
            surfaceFoamColor2 = serializedObject.FindProperty("surfaceFoamColor2");
            surfaceFoamMovement = serializedObject.FindProperty("surfaceFoamMovement");
            surfaceFoamDirection1 = serializedObject.FindProperty("surfaceFoamDirection1");
            surfaceFoamDirection2 = serializedObject.FindProperty("surfaceFoamDirection2");
            surfaceFoamSpeed1 = serializedObject.FindProperty("surfaceFoamSpeed1");
            surfaceFoamSpeed2 = serializedObject.FindProperty("surfaceFoamSpeed2");
            surfaceFoamOffsetX = serializedObject.FindProperty("surfaceFoamOffsetX");
            surfaceFoamOffsetY = serializedObject.FindProperty("surfaceFoamOffsetY");
            surfaceFoamScale1 = serializedObject.FindProperty("surfaceFoamScale1");
            surfaceFoamScale2 = serializedObject.FindProperty("surfaceFoamScale2");
            #endregion

            #region Shore Foam
            shoreFoamSpeed = serializedObject.FindProperty("shoreFoamSpeed");
            shoreFoamWidth = serializedObject.FindProperty("shoreFoamWidth");
            shoreFoamFrequency = serializedObject.FindProperty("shoreFoamFrequency");
            shoreFoamBreakupScale = serializedObject.FindProperty("shoreFoamBreakupScale");
            shoreFoamBreakupStrength = serializedObject.FindProperty("shoreFoamBreakupStrength");
            #endregion

            #region Surface Foam Height Mask
            enableHeightMask = serializedObject.FindProperty("enableHeightMask");
            surfaceFoamHeightMask = serializedObject.FindProperty("surfaceFoamHeightMask");
            surfaceFoamHeightMaskSmoothness = serializedObject.FindProperty("surfaceFoamHeightMaskSmoothness");
            #endregion

            #region Intersection Effects
            enableIntersectionEffects = serializedObject.FindProperty("enableIntersectionEffects");
            intersectionFoamTexture = serializedObject.FindProperty("intersectionFoamTexture");
            intersectionFoamDepth = serializedObject.FindProperty("intersectionFoamDepth");
            intersectionFoamColor = serializedObject.FindProperty("intersectionFoamColor");
            intersectionFoamDistortion = serializedObject.FindProperty("intersectionFoamDistortion");
            intersectionFoamCutoff = serializedObject.FindProperty("intersectionFoamCutoff");
            intersectionFoamSpeed = serializedObject.FindProperty("intersectionFoamSpeed");
            intersectionFoamDirection = serializedObject.FindProperty("intersectionFoamDirection");
            intersectionFoamScale = serializedObject.FindProperty("intersectionFoamScale");
            foamMovement = serializedObject.FindProperty("foamMovement");
            shoreMovementEnabled = foamMovement.enumValueIndex == 1;
            #endregion

            #region Lighting
            lighting = serializedObject.FindProperty("lighting");
            lightingDisabled = lighting.enumValueIndex == 1;
            lightingSmoothness = serializedObject.FindProperty("lightingSmoothness");
            lightingHardness = serializedObject.FindProperty("lightingHardness");
            lightingSpecularColor = serializedObject.FindProperty("lightingSpecularColor");
            lightingDiffuseColor = serializedObject.FindProperty("lightingDiffuseColor");
            #endregion

            #region Surface 
            normalsTexture = serializedObject.FindProperty("normalsTexture");
            normalsStrength = serializedObject.FindProperty("normalsStrength");
            normalsScale = serializedObject.FindProperty("normalsScale");
            normalsSpeed = serializedObject.FindProperty("normalsSpeed");
            surfaceNormals = serializedObject.FindProperty("surfaceNormals");

            reflectionStrength = serializedObject.FindProperty("reflectionStrength");
            reflectionFresnel = serializedObject.FindProperty("reflectionFresnel");
            #endregion

            #region Additional Settings
            hideComponents = serializedObject.FindProperty("hideComponents");
            enableFoamShadows = serializedObject.FindProperty("enableFoamShadows");
            enableRefraction = serializedObject.FindProperty("enableRefraction");
            waterUVs = serializedObject.FindProperty("waterUVs");
            #endregion

            #region Waves
            waveSteepness = serializedObject.FindProperty("waveSteepness");
            waveLength = serializedObject.FindProperty("waveLength");
            waveSpeed = serializedObject.FindProperty("waveSpeed");

            waveDirection1 = serializedObject.FindProperty("waveDirection1");
            waveDirection2 = serializedObject.FindProperty("waveDirection2");
            waveDirection3 = serializedObject.FindProperty("waveDirection3");
            waveDirection4 = serializedObject.FindProperty("waveDirection4");
            #endregion

            usesColorGradient = useColorGradient.boolValue;
            usesWorldSpaceUVs = waterUVs.enumValueIndex == 1;
            surfaceFoamEnabled = enableSurfaceFoam.boolValue;
            foamShadowsEnabled = enableFoamShadows.boolValue;
            refractionEnabled = enableRefraction.boolValue;
            intersectionEffectsEnabled = enableIntersectionEffects.boolValue;
        }
    }
}
#endif