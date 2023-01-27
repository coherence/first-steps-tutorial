using Coherence.UI;
using TMPro;
using UnityEngine;

/// <summary>
/// Retrieves the player name from the coherence sample Connect UI.
/// </summary>
public class PlayerName : MonoBehaviour
{
    public TextMeshProUGUI textField;
    
    private void Start()
    {
        textField.text = WorldsConnectDialog.PlayerName;
    }
}
