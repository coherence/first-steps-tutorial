using Coherence.UI;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public TextMeshProUGUI textField;
    
    private void Start()
    {
        textField.text = WorldsConnectDialog.PlayerName;
    }
}
