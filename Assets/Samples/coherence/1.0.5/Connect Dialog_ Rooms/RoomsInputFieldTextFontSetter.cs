using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// We use this Component to set the Input Field text fonts at runtime to avoid the SendMessage warning spam from Unity
public class RoomsInputFieldTextFontSetter : MonoBehaviour
{
    public Font font;

    public List<InputField> inputFields;

    void Awake()
    {
        foreach (var inputField in inputFields)
        {
            inputField.textComponent.font = font;
            var placeHolder = inputField.placeholder as Text;

            if (placeHolder != null)
            {
                placeHolder.font = font;
            }
        }
    }
}
