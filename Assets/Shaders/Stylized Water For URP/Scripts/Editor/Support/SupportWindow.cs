//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━																												
// Copyright 2020, Alexander Ameye, All rights reserved.
// https://alexander-ameye.gitbook.io/stylized-water/
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━	

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Reflection;
using UnityEngine.Rendering.Universal;

namespace StylizedWater
{
    class RunOnImport : AssetPostprocessor
    {
        private static string registryKeyShowWindow = "ameye.stylizedwaterforurp.showsupportwindow";
        public static bool shouldshowSupportWindow
        {
            get { return EditorPrefs.GetBool(registryKeyShowWindow, true); }
            set { EditorPrefs.SetBool(registryKeyShowWindow, value); }
        }

        static RunOnImport() => EditorApplication.update += OpenSupportWindow;

        static void OpenSupportWindow()
        {
            if (!EditorApplication.isUpdating && shouldshowSupportWindow)
            {
                SupportWindow.ShowWindow();
                shouldshowSupportWindow = false;
                EditorApplication.update -= OpenSupportWindow;
            }
        }
    }

    public class SupportWindow : EditorWindow
    {
        private const string SupportedText = "Supported";
        private const string NotTestedText = "Not tested";
        private const string NotSupportedText = "Not supported";
        
        private VisualElement root;

        private VisualElement supportTab;
        private VisualElement aboutTab;
        private VisualElement acknowledgementsTab;
        private VisualElement detectIssuestab;

        private VisualTreeAsset support;
        private VisualTreeAsset about;
        private VisualTreeAsset acknowledgements;
        private VisualTreeAsset detectIssues;

        private StyleSheet styleSheet;

        static ListRequest listRequest;
        static SearchRequest urpSearchRequest;
        static AddRequest addRequest;

        static Label unityVersionLabel;
        static Label activeRendererLabel;
        static Label URPVersionLabel;
        static Label depthTextureLabel;
        static Label opaqueTextureLabel;
        static Label graphicsAPILabel;
        static Label planarReflectionsLabel;
        static Label shaderVariantLimitLabel;

        static VisualElement unityVersionIcon;
        static VisualElement activeRendererIcon;
        static VisualElement URPVersionIcon;
        static VisualElement depthTextureIcon;
        static VisualElement opaqueTextureIcon;
        static VisualElement graphicsAPIIcon;
        static VisualElement planarReflectionsIcon;
        static VisualElement shaderVariantLimitIcon;

        static Button unityVersionFix;
        static Button URPVersionFix;
        static Button activeRendererFix;
        static Button depthTextureFix;
        static Button opaqueTextureFix;
        static Button graphicsAPIFix;
        static Button planarReflectionsFix;
        static Button shaderVariantLimitFix;

        static Button aboutButton;
        static Button supportButton;
        static Button configureButton;
        static Button acknowledgementsButton;
        static Button graphicsAPIButton;

        static Texture2D neutral;
        static Texture2D positive;
        static Texture2D negative;

        public enum PipelineType
        {
            Custom,
            Default,
            Lightweight,
            Universal,
            HighDefinition
        }

        [MenuItem("Tools/Stylized Water For URP/About and Support")]
        public static SupportWindow ShowWindow()
        {
            var window = GetWindow<SupportWindow>();
            window.minSize = new Vector2(400, 400);
            window.maxSize = new Vector2(400, 400);
            return window;
        }

        private void OnEnable()
        {
            titleContent.text = "Support";
            titleContent.image = EditorGUIUtility.IconContent("Settings").image;
            Init();
        }

        private void Init()
        {
            root = this.rootVisualElement;
            supportTab = new VisualElement();
            aboutTab = new VisualElement();
            detectIssuestab = new VisualElement();
            acknowledgementsTab = new VisualElement();

            support = GetUXML("StylizedWaterSupport");
            about = GetUXML("StylizedWaterAbout");
            acknowledgements = GetUXML("StylizedWaterAcknowledgements");
            detectIssues = GetUXML("StylizedWaterDetectIssues");

            if (support) support.CloneTree(supportTab);
            if (about) about.CloneTree(aboutTab);
            if (acknowledgements) acknowledgements.CloneTree(acknowledgementsTab);
            if (detectIssues) detectIssues.CloneTree(detectIssuestab);

            root.Add(supportTab);
            root.Add(aboutTab);
            root.Add(acknowledgementsTab);
            root.Add(detectIssuestab);

            supportTab.style.display = DisplayStyle.None;
            aboutTab.style.display = DisplayStyle.None;
            detectIssuestab.style.display = DisplayStyle.Flex;
            acknowledgementsTab.style.display = DisplayStyle.None;

            unityVersionLabel = root.Q<Label>("Unity Version");
            activeRendererLabel = root.Q<Label>("Active Renderer");
            URPVersionLabel = root.Q<Label>("URP_Version");
            depthTextureLabel = root.Q<Label>("Depth Texture");
            opaqueTextureLabel = root.Q<Label>("Opaque Texture");
            graphicsAPILabel = root.Q<Label>("Graphics_API");
            planarReflectionsLabel = root.Q<Label>("Planar_Reflections");
            shaderVariantLimitLabel = root.Q<Label>("Shader_Variant_Limit");

            unityVersionIcon = root.Q<VisualElement>("Unity Version Icon");
            activeRendererIcon = root.Q<VisualElement>("Active Renderer Icon");
            URPVersionIcon = root.Q<VisualElement>("URP Version Icon");
            depthTextureIcon = root.Q<VisualElement>("Depth Texture Icon");
            opaqueTextureIcon = root.Q<VisualElement>("Opaque Texture Icon");
            graphicsAPIIcon = root.Q<VisualElement>("Graphics_API_Icon");
            planarReflectionsIcon = root.Q<VisualElement>("Planar_Reflections_Icon");
            shaderVariantLimitIcon = root.Q<VisualElement>("Shader_Variant_Limit_Icon");

            unityVersionFix = root.Q<Button>("Unity Version Fix");
            activeRendererFix = root.Q<Button>("Active Renderer Fix");
            URPVersionFix = root.Q<Button>("URP_version_fix");
            depthTextureFix = root.Q<Button>("Depth Texture Fix");
            opaqueTextureFix = root.Q<Button>("Opaque Texture Fix");
            graphicsAPIFix = root.Q<Button>("Graphics_API_Fix");
            planarReflectionsFix = root.Q<Button>("Planar_Reflections_Fix");
            shaderVariantLimitFix = root.Q<Button>("Shader_Variant_Limit_Fix");

            supportButton = root.Q<Button>("SupportButton");
            aboutButton = root.Q<Button>("AboutButton");
            acknowledgementsButton = root.Q<Button>("AcknowledgementsButton");
            configureButton = root.Q<Button>("ConfigureButton");
            
            neutral = AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Assets/Stylized Water For URP/Scripts/UI/Icons/Neutral.png");
            positive = AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Assets/Stylized Water For URP/Scripts/UI/Icons/Positive.png");
            negative = AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Assets/Stylized Water For URP/Scripts/UI/Icons/Negative.png");

            root.Query<Button>().ForEach((button) =>
               {
                   button.clickable.clickedWithEventInfo += Clickedbutton;
               });

            SetUnchecked();
        }

        private static PipelineType DetectRenderPipeline()
        {
            if (GraphicsSettings.renderPipelineAsset == null) return PipelineType.Default;
            var type = GraphicsSettings.renderPipelineAsset.GetType().ToString();
            if (type.Contains("HDRenderPipelineAsset")) return PipelineType.HighDefinition;
            if (type.Contains("UniversalRenderPipelineAsset")) return PipelineType.Universal;
            if (type.Contains("LightweightRenderPipelineAsset")) return PipelineType.Lightweight;
            return PipelineType.Custom;
        }

        private static VisualTreeAsset GetUXML(string name)
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(VisualTreeAsset)} {name}");
            if (guids.Length == 0)
                return null;
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(guids[0]));
            return uxml;
        }

        private void Clickedbutton(EventBase tab)
        {
            Button button = tab.target as Button;
            if (button == null) return;

            switch (button.name)
            {
                case "SupportButton":
                    supportTab.style.display = DisplayStyle.Flex;
                    aboutTab.style.display = DisplayStyle.None;
                    detectIssuestab.style.display = DisplayStyle.None;
                    acknowledgementsTab.style.display = DisplayStyle.None;
                    break;
                case "AboutButton":
                    supportTab.style.display = DisplayStyle.None;
                    aboutTab.style.display = DisplayStyle.Flex;
                    detectIssuestab.style.display = DisplayStyle.None;
                    acknowledgementsTab.style.display = DisplayStyle.None;
                    break;
                case "ConfigureButton":
                    supportTab.style.display = DisplayStyle.None;
                    aboutTab.style.display = DisplayStyle.None;
                    detectIssuestab.style.display = DisplayStyle.Flex;
                    acknowledgementsTab.style.display = DisplayStyle.None;
                    break;
                case "AcknowledgementsButton":
                    supportTab.style.display = DisplayStyle.None;
                    aboutTab.style.display = DisplayStyle.None;
                    detectIssuestab.style.display = DisplayStyle.None;
                    acknowledgementsTab.style.display = DisplayStyle.Flex;
                    break;
                case "Check":
                    SetUnchecked();
                    CheckUnityVersion();
                    break;
                case "Manual":
                    Application.OpenURL("https://alexander-ameye.gitbook.io/stylized-water/support/troubleshooting");
                    break;
                case "Forum":
                    Application.OpenURL("https://forum.unity.com/threads/stylized-water-for-urp-desktop-mobile-released.846313/");
                    break;
                case "Caustics":
                    Application.OpenURL("https://assetstore.unity.com/packages/vfx/shaders/water-caustics-for-urp-221106");
                    break;
                case "Contact":
                    Application.OpenURL("https://discord.gg/6QQ5JCc");
                    break;
                case "Review":
                    Application.OpenURL("https://assetstore.unity.com/packages/vfx/shaders/stylized-water-for-urp-162025");
                    break;
                case "Twitter":
                    Application.OpenURL("https://twitter.com/alexanderameye");
                    break;
                case "Website":
                    Application.OpenURL("https://alexanderameye.github.io/");
                    break;
                case "Unity Version Fix":
                    Application.OpenURL("https://unity3d.com/get-unity/download");
                    break;
                case "Active Renderer Fix":
                    SettingsService.OpenProjectSettings("Project/Graphics");
                    break;
                case "Shader_Variant_Limit_Fix":
                    EditorPrefs.SetInt("UnityEditor.ShaderGraph.VariantLimit", 256);
                    Debug.Log("Shader Variant Limit set to 256.");
                    var readMethod = typeof(Editor).Assembly.GetType("UnityEditor.CacheServerPreferences")
                        .GetMethod("ReadPreferences", BindingFlags.Static | BindingFlags.Public);
                    readMethod.Invoke(null, null);

                    DetectIssues();
                    SettingsService.OpenUserPreferences("Preferences/Shader Graph");
                    break;
                case "Planar_Reflections_Fix":
                    if (Camera.main != null)
                    {
                        Selection.activeGameObject = Camera.main.gameObject;
                        EditorGUIUtility.PingObject(Camera.main);
                    }
                    break;
                case "Depth Texture Fix":
                {
                    RenderPipelineAsset activeRenderPipelineAsset = GraphicsSettings.currentRenderPipeline;
                    UniversalRenderPipelineAsset pipeline = activeRenderPipelineAsset as UniversalRenderPipelineAsset;
                    EditorGUIUtility.PingObject(pipeline);
                    break;
                }
                case "Opaque Texture Fix":
                {
                    RenderPipelineAsset activeRenderPipelineAsset = GraphicsSettings.currentRenderPipeline;
                    UniversalRenderPipelineAsset pipeline = activeRenderPipelineAsset as UniversalRenderPipelineAsset;
                    EditorGUIUtility.PingObject(pipeline);
                    break;
                }
                case "KeenanWoodall":
                    Application.OpenURL("https://twitter.com/keenanwoodall");
                    break;
                case "JoshSauter":
                    Application.OpenURL("https://github.com/JoshSauter");
                    break;
            }
        }

        private void SetUnchecked()
        {
            unityVersionLabel.text = "Untested";
            activeRendererLabel.text = "Untested";
            URPVersionLabel.text = "Untested";
            depthTextureLabel.text = "Untested";
            opaqueTextureLabel.text = "Untested";
            graphicsAPILabel.text = "Untested";
            planarReflectionsLabel.text = "Untested";
            shaderVariantLimitLabel.text = "Untested";

            unityVersionIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            activeRendererIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            URPVersionIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            depthTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            opaqueTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            planarReflectionsIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            shaderVariantLimitIcon.style.backgroundImage = Background.FromTexture2D(neutral);

            URPVersionFix.SetEnabled(false);
            unityVersionFix.SetEnabled(false);
            activeRendererFix.SetEnabled(false);
            depthTextureFix.SetEnabled(false);
            opaqueTextureFix.SetEnabled(false);
            graphicsAPIFix.SetEnabled(false);
            planarReflectionsFix.SetEnabled(false);
            shaderVariantLimitFix.SetEnabled(false);
        }

        private void CheckUnityVersion()
        {
            var currentUnityVersion = Application.unityVersion;
            var currentUnityVersionArray = currentUnityVersion.Split(".".ToCharArray());
            var currentUnityVersionTruncated = string.Join(".", currentUnityVersionArray.Take(2));
            unityVersionLabel.text = currentUnityVersion;

            var tested = new Version("2021.3.0");
            var current = new Version(currentUnityVersionTruncated);
            var unityVersionComparison = tested.CompareTo(current);

            switch (unityVersionComparison)
            {
                // supported versions
                case var value when value >= 0:
                    unityVersionIcon.style.backgroundImage = Background.FromTexture2D(positive);
                    unityVersionIcon.tooltip = SupportedText;
                    break;
                // untested versions
                case var value when value < 0:
                    unityVersionIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    unityVersionFix.SetEnabled(false);
                    unityVersionIcon.tooltip = NotTestedText;
                    break;
            }
            
            CheckSRPVersion();
        }

        private void CheckSRPVersion()
        {
            URPVersionLabel.text = "Testing ...";
            URPVersionIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            listRequest = Client.List();
            urpSearchRequest = Client.Search("com.unity.render-pipelines.universal");
            EditorApplication.update += FindSRPVersion;
        }

        static void DetectIssues()
        {
            // check shader variant limit
            var limit = EditorPrefs.GetInt("UnityEditor.ShaderGraph.VariantLimit");
            shaderVariantLimitLabel.text = limit.ToString();

            if (limit < 256)
            {
                shaderVariantLimitIcon.style.backgroundImage = Background.FromTexture2D(negative);
                shaderVariantLimitFix.style.visibility = Visibility.Visible;
            }
            else
            {
                shaderVariantLimitIcon.style.backgroundImage = Background.FromTexture2D(positive);
                shaderVariantLimitFix.style.visibility = Visibility.Hidden;
            }

            activeRendererIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            depthTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);
            opaqueTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);

            int planarReflectionComponents = 0;
            PlanarReflections[] planarReflections = FindObjectsOfType<PlanarReflections>();
            foreach (PlanarReflections planarReflection in planarReflections) if (planarReflection.enabled) planarReflectionComponents += 1;

            if (planarReflectionComponents == 1)
            {
                planarReflectionsLabel.text = "Enabled";
                planarReflectionsIcon.style.backgroundImage = Background.FromTexture2D(positive);
            }

            else if (planarReflectionComponents > 1)
            {
                planarReflectionsLabel.text = "Multiple";
                planarReflectionsIcon.style.backgroundImage = Background.FromTexture2D(negative);
            }

            else if (planarReflectionComponents == 0)
            {
                planarReflectionsLabel.text = "Disabled";
                planarReflectionsIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                if (Camera.main != null) planarReflectionsFix.style.visibility = Visibility.Visible;
            }

            switch (SystemInfo.graphicsDeviceType)
            {
                case GraphicsDeviceType.Vulkan:
                    graphicsAPILabel.text = "Vulkan";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(positive);
                    graphicsAPIIcon.tooltip = SupportedText;
                    break;
                case GraphicsDeviceType.Direct3D11:
                    graphicsAPILabel.text = "Direct3D11";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(positive);
                    graphicsAPIIcon.tooltip = SupportedText;
                    break;
                case GraphicsDeviceType.OpenGLES3:
                    graphicsAPILabel.text = "OpenGLES3";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    graphicsAPIIcon.tooltip = NotTestedText;
                    break;
                case GraphicsDeviceType.Direct3D12:
                    graphicsAPILabel.text = "Direct3D12";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    graphicsAPIIcon.tooltip = NotTestedText;
                    break;
                case GraphicsDeviceType.OpenGLES2:
                    graphicsAPILabel.text = "OpenGLES2";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(negative);
                    graphicsAPIIcon.tooltip = NotSupportedText;
                    break;
                case GraphicsDeviceType.Null:
                    graphicsAPILabel.text = "Null";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    graphicsAPIIcon.tooltip = NotTestedText;
                    break;
                case GraphicsDeviceType.PlayStation4:
                    graphicsAPILabel.text = "PlayStation4";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    graphicsAPIIcon.tooltip = NotTestedText;
                    break;
                case GraphicsDeviceType.XboxOne:
                    graphicsAPILabel.text = "XboxOne";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    graphicsAPIIcon.tooltip = NotTestedText;
                    break;
                case GraphicsDeviceType.Metal:
                    graphicsAPILabel.text = "Metal";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(positive);
                    graphicsAPIIcon.tooltip = SupportedText;
                    break;
                case GraphicsDeviceType.OpenGLCore:
                    graphicsAPILabel.text = "OpenGLCore";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    graphicsAPIIcon.tooltip = NotTestedText;
                    break;
                case GraphicsDeviceType.XboxOneD3D12:
                    graphicsAPILabel.text = "XboxOneD3D12";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    graphicsAPIIcon.tooltip = NotTestedText;
                    break;
                case GraphicsDeviceType.Switch:
                    graphicsAPILabel.text = "Switch";
                    graphicsAPIIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    graphicsAPIIcon.tooltip = NotTestedText;
                    break;
            }

            switch (DetectRenderPipeline())
            {
                case PipelineType.Custom:
                    activeRendererLabel.text = "Custom";
                    depthTextureLabel.text = NotTestedText;
                    opaqueTextureLabel.text = NotTestedText;
                    
                    activeRendererIcon.tooltip = NotSupportedText;
                    depthTextureIcon.tooltip = NotTestedText;
                    opaqueTextureIcon.tooltip = NotTestedText;

                    activeRendererIcon.style.backgroundImage = Background.FromTexture2D(negative);
                    depthTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    opaqueTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);

                    depthTextureFix.SetEnabled(false);
                    activeRendererFix.SetEnabled(false);
                    opaqueTextureFix.SetEnabled(false);
                    break;
                case PipelineType.Default:
                    activeRendererLabel.text = "Default";
                    depthTextureLabel.text = NotTestedText;
                    opaqueTextureLabel.text = NotTestedText;
                    
                    activeRendererIcon.tooltip = NotSupportedText;
                    depthTextureIcon.tooltip = NotTestedText;
                    opaqueTextureIcon.tooltip = NotTestedText;

                    activeRendererIcon.style.backgroundImage = Background.FromTexture2D(negative);
                    depthTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    opaqueTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);

                    depthTextureFix.SetEnabled(false);
                    activeRendererFix.SetEnabled(false);
                    opaqueTextureFix.SetEnabled(false);
                    break;
                case PipelineType.Lightweight:
                    activeRendererLabel.text = "Lightweight";
                    depthTextureLabel.text = NotTestedText;
                    opaqueTextureLabel.text = NotTestedText;
                    
                    activeRendererIcon.tooltip = NotSupportedText;
                    depthTextureIcon.tooltip = NotTestedText;
                    opaqueTextureIcon.tooltip = NotTestedText;

                    activeRendererIcon.style.backgroundImage = Background.FromTexture2D(negative);
                    depthTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    opaqueTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);

                    depthTextureFix.SetEnabled(false);
                    activeRendererFix.SetEnabled(false);
                    opaqueTextureFix.SetEnabled(false);
                    break;
                case PipelineType.Universal:
#if UNIVERSAL_RENDERER
                    UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset pipeline = GraphicsSettings.currentRenderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
#if UNIVERSAL_731 // GetRenderer() was introduced in 7.3.1
                    if (pipeline.GetRenderer(0).GetType().ToString().Contains("Renderer2D"))
                    {
                        activeRendererLabel.text = "2D Renderer";
                        activeRendererIcon.style.backgroundImage = Background.FromTexture2D(negative);
                        activeRendererFix.SetEnabled(false);
                        activeRendererIcon.tooltip = NotSupportedText;
                    }

                    else
                    {
                        activeRendererLabel.text = "Universal";
                        activeRendererIcon.style.backgroundImage = Background.FromTexture2D(positive);
                        activeRendererFix.SetEnabled(false);
                        activeRendererIcon.tooltip = SupportedText;
                    }
#else
                    activeRendererLabel.text = "Universal";
                    activeRendererIcon.style.backgroundImage = Background.FromTexture2D(positive);
                    activeRendererFix.style.visibility = Visibility.Hidden;
#endif

                    if (pipeline.supportsCameraDepthTexture)
                    {
                        depthTextureLabel.text = "Enabled";
                        depthTextureIcon.style.backgroundImage = Background.FromTexture2D(positive);
                        depthTextureFix.SetEnabled(false);
                        depthTextureIcon.tooltip = "Depth texture enabled";
                    }

                    else
                    {
                        depthTextureLabel.text = "Disabled";
                        depthTextureIcon.style.backgroundImage = Background.FromTexture2D(negative);
                        depthTextureFix.SetEnabled(true);
                        depthTextureIcon.tooltip = "Depth texture disabled";
                    }

                    if (pipeline.supportsCameraOpaqueTexture)
                    {
                        opaqueTextureLabel.text = "Enabled";
                        opaqueTextureIcon.style.backgroundImage = Background.FromTexture2D(positive);
                        opaqueTextureFix.SetEnabled(false);
                        opaqueTextureIcon.tooltip = "Opaque texture enabled";
                    }

                    else
                    {
                        opaqueTextureLabel.text = "Disabled";
                        opaqueTextureIcon.style.backgroundImage = Background.FromTexture2D(negative);
                        opaqueTextureFix.SetEnabled(true);
                        opaqueTextureIcon.tooltip = "Opaque texture disabled";
                    }
#endif
                    break;
                case PipelineType.HighDefinition:
                    activeRendererLabel.text = "High Definition";
                    depthTextureLabel.text = NotTestedText;
                    opaqueTextureLabel.text = NotTestedText;
                    
                    activeRendererIcon.tooltip = NotSupportedText;
                    depthTextureIcon.tooltip = NotTestedText;
                    opaqueTextureIcon.tooltip = NotTestedText;

                    activeRendererIcon.style.backgroundImage = Background.FromTexture2D(negative);
                    depthTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                    opaqueTextureIcon.style.backgroundImage = Background.FromTexture2D(neutral);

                    activeRendererFix.SetEnabled(false);
                    depthTextureFix.SetEnabled(false);
                    opaqueTextureFix.SetEnabled(false);
                    break;
            }
        }

        static void FindSRPVersion()
        {
#if UNIVERSAL_RENDERER
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    foreach (var package in listRequest.Result)
                    {
                        if (package.name == "com.unity.render-pipelines.universal")
                        {
                            var currentUrpVersion = package.version;
                            var currentUrpVersionArray = currentUrpVersion.Split(".".ToCharArray());
                            var currentUrpVersionTruncated = string.Join(".", currentUrpVersionArray.Take(3));
                            URPVersionLabel.text = currentUrpVersion;

                            var tested = new Version("12.1.7");
                            var current = new Version(currentUrpVersionTruncated);
                            var urpVersionComparison = tested.CompareTo(current);

                            switch (urpVersionComparison)
                            {
                                // supported versions
                                case var value when value >= 0:
                                    URPVersionIcon.style.backgroundImage = Background.FromTexture2D(positive);
                                    URPVersionIcon.tooltip = SupportedText;
                                    DetectIssues();
                                    break;
                                // untested versions
                                case var value when value < 0:
                                    URPVersionIcon.style.backgroundImage = Background.FromTexture2D(neutral);
                                    URPVersionIcon.tooltip = NotTestedText;
                                    break;
                            }
                        }
                    }
                }
                else if (listRequest.Status >= StatusCode.Failure)
                    Debug.Log(listRequest.Error.message);

                EditorApplication.update -= FindSRPVersion;
            }
#else
            URPVersionLabel.text = "Not Installed";
            URPVersionIcon.style.backgroundImage = Background.FromTexture2D(negative);
#endif
        }
    }
}
