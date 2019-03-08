using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string playerNamePrefKey  = "Name";
        string defaultName = "";
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
    }

    public void PlayerNameSet(string value)
    {
        PlayerPrefs.SetString("Name", value);
    }

}
