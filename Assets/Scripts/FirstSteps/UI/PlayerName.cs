using TMPro;
using UnityEngine;

namespace Coherence.FirstSteps
{
    /// <summary>
    /// Retrieves the player name from a static field on the <see cref="UserInterface"/> script.
    /// </summary>
    public class PlayerName : MonoBehaviour
    {
        public TextMeshProUGUI textField;
    
        private void Start()
        {
            textField.text = UserInterface.PLAYER_NAME;
        }
    }
}
