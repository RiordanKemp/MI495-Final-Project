using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public TextMeshProUGUI[] settingBinds;
    public Dictionary<string, KeyCode> controls = new Dictionary<string, KeyCode>();
    public Dictionary<string, KeyCode> tempDict = new Dictionary<string, KeyCode>();
    public Dictionary<string, TextMeshProUGUI> textDict = new Dictionary<string, TextMeshProUGUI>();

    bool listening = false;
    string control_name = "";
    void Awake(){
 
        DefaultDict();
        PrefsToTempDict();
        SetButtonsDict();

        //the main dict isnt updated until every key has been accounted for
        //that way, the loop doesn't break
        controls = tempDict;

    }

    void SetButtonsDict(){
        foreach(string key in controls.Keys){
            foreach(TextMeshProUGUI tmpro in settingBinds){
                if(tmpro.name == key){
            textDict.Add(key, tmpro);
                                    }
                                }
                            }
                        }
    

   void DefaultDict(){
        controls.Add("right", KeyCode.D);
        controls.Add("left", KeyCode.A);
        controls.Add("jump", KeyCode.Space);
        controls.Add("interact", KeyCode.E);
        controls.Add("triggerfan", KeyCode.F);
        controls.Add("nextdialogue", KeyCode.Return);
        controls.Add("holdobject", KeyCode.LeftShift);
   }

   void DictToPrefs(){
    foreach (string key in controls.Keys){
        PlayerPrefs.SetString(key, controls[key].ToString());
    }
   }

   void PrefsToTempDict(){
    foreach(string keyStr in controls.Keys){

            //if no player pref is set yet, set it to the default
            if (!PlayerPrefs.HasKey(keyStr)){
                PlayerPrefs.SetString(keyStr, controls[keyStr].ToString());
            }

            //update each dict key based on player prefs
            string prefStr = PlayerPrefs.GetString(keyStr);
            UpdateKey(keyStr, prefStr);
            print("keystr:" + keyStr + " prefstr:" + prefStr);
        }
   }

   void ResetDict(){
    controls.Clear();
    DefaultDict();

   }

   void ResetPlayerPrefs(){
    foreach (string key in controls.Keys){
        PlayerPrefs.DeleteKey(key);
    }
   }

    public void UpdateKey(string key, string bindingStr){
        KeyCode bindingKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), bindingStr) ;
        print("binding key:" + bindingKey);
        tempDict[key] = bindingKey;
    }

    public bool defaultSettings = false;

    private void OnDrawGizmos()
    {
        if (defaultSettings)
        {
            defaultSettings = false;
            ResetSettings();
        }
    }

    public void ResetSettings(){
            ResetPlayerPrefs();
            ResetDict();
            DictToPrefs();
            UpdateAllKeys();
            print("Player Prefs for settings set back to default");
    }

    void UpdateAllKeys(){
        foreach(string key in textDict.Keys){
            AwakeBinding bindTextScript = textDict[key].GetComponent<AwakeBinding>();
            bindTextScript.UpdateThisKey();
        }
    }

    public void ChangeControl(string control){
        print("change control is running");
        listening = true;
        control_name = control;

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))){
            if (Input.GetKeyDown(key)){
                controls[control] = key;
                listening = false;

                string keyStr = key.ToString();
                PlayerPrefs.SetString(control, keyStr);
                textDict[control].text = keyStr;

                break;
            }
        }
    }

            void Update(){
            if (listening){
                ChangeControl(control_name);
            }
        }
}

