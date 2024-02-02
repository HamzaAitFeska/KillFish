using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationSelector : MonoBehaviour
{
    // Start is called before the first frame update
    bool active;
    private void Start()
    {
        active = false;
        
    }
    public void ChangeLocalID(int localID)
    {
        if (active == true)
        {
            return;
        }
        StartCoroutine(SetLocal(localID));
    }
    IEnumerator SetLocal(int _setLocalID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_setLocalID];
        active = false;
    }
}
