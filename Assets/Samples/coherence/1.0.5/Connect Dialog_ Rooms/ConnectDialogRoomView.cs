// Copyright (c) coherence ApS.
// See the license file in the package root for more information.

using Coherence.Cloud;

namespace Coherence.Samples.RoomsDialog
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;

    public class ConnectDialogRoomView: MonoBehaviour
    {
        [SerializeField] protected Button selectButton;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Text roomNameText;
        [SerializeField] private Text roomPlayersText;
        [SerializeField] protected Color defaultColor = new Color(243, 247, 250);
        [SerializeField] protected Color selectedColor = new Color(122, 184, 240);

        public RoomData RoomData
        {
            get => roomData;
            set
            {
                roomData = value;
                roomNameText.text = !String.IsNullOrEmpty(RoomData.RoomName) ? TruncateName(RoomData.RoomName) : RoomData.ToString();
                roomPlayersText.text = $"{RoomData.ConnectedPlayers}/{RoomData.MaxPlayers}";
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
        public RoomData roomData;

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
