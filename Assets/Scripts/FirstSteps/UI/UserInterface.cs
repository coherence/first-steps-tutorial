using UnityEngine;
using UnityEngine.UI;

namespace Coherence.FirstSteps
{
    /// <summary>
    /// Basic implementation to fetch the name inputted in the coherence sample UI,
    /// and use it as a player name in the world-space UI.
    /// We store the value in a static field, that the player UI (<see cref="PlayerName"/>) will pick up
    /// when the player character is instantiated.
    /// </summary>
    public class UserInterface : MonoBehaviour
    {
        public static string PLAYER_NAME;

        public InputField playerNameField;

        private void Awake()
        {
            if (string.IsNullOrEmpty(playerNameField.text))
            {
                // Try to carry over the name from the previous scene
                playerNameField.text = PLAYER_NAME;
            }
            else
            {
                // Set the value to the default in the text field (should be "Player")
                PLAYER_NAME = playerNameField.text;
            }
        
            playerNameField.onValueChanged.AddListener(TextFieldChanged);
        }

        private void TextFieldChanged(string newName)
        {
            PLAYER_NAME = newName;
        }

        private void OnDestroy()
        {
            playerNameField.onValueChanged.RemoveListener(TextFieldChanged);
        }
    }
}
