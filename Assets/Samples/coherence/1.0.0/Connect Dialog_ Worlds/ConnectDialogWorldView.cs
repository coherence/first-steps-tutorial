// Copyright (c) coherence ApS.
// See the license file in the package root for more information.

using Coherence.Cloud;

namespace Coherence.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;

    public class ConnectDialogWorldView : MonoBehaviour
    {
        [SerializeField] protected Button selectButton;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Text worldNameText;
        [SerializeField] private Text worldIdText;
        [SerializeField] protected Color defaultColor = new Color(243, 247, 250);
        [SerializeField] protected Color selectedColor = new Color(122, 184, 240);

        public WorldData WorldData
        {
            get => worldData;
            set
            {
                worldData = value;
                worldNameText.text = !String.IsNullOrEmpty(WorldData.Name) ? TruncateName(WorldData.Name) : WorldData.ToString();
                worldIdText.text = WorldData.WorldId.ToString();
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                backgroundImage.color = isSelected ? selectedColor : defaultColor;
            }
        }

        public Action OnClick
        {
            set
            {
                selectButton.onClick.RemoveAllListeners();
                selectButton.onClick.AddListener(() => value?.Invoke());
            }
        }

        [SerializeField, HideInInspector] private bool isSelected;
        public WorldData worldData;

        private static string TruncateName(string name, int maxLength = 30)
        {
            string newName = name;

            if (newName.Length > maxLength)
            {
                newName = newName.Substring(0, maxLength) + "...";
            }

            return newName;
        }
    }
}
