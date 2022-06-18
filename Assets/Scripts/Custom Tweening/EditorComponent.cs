using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorComponent : MonoBehaviour
{
    public int componentIndex = 0;

    EditorManager editorManager;

    [SerializeField] GameObject animationType;
    TMP_Dropdown animationTypeDropdown;
    

    [SerializeField] GameObject transformValue;
    TMP_InputField transformValueX;
    TMP_InputField transformValueY;
    TMP_InputField transformValueZ;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(componentIndex);
        editorManager = gameObject.GetComponentInParent<EditorManager>();
        animationTypeDropdown = animationType.GetComponent<TMP_Dropdown>();

        transformValueX = transformValue.transform.GetChild(0).GetComponent<TMP_InputField>();
        transformValueY = transformValue.transform.GetChild(1).GetComponent<TMP_InputField>();
        transformValueZ = transformValue.transform.GetChild(2).GetComponent<TMP_InputField>();
        
    }

    void SetupListeners(){
        animationTypeDropdown.onValueChanged.AddListener(delegate {
            AnimationTypeDropdownChanged(animationTypeDropdown);
        });
        transformValueX.onEndEdit.AddListener(delegate{

        });
    }

    void AnimationTypeDropdownChanged(TMP_Dropdown change){
        Debug.Log(change.value);
        editorManager.ChangeAnimationType(componentIndex, change.value);
    }

    void TransformValueChanged(int dimension, TMP_InputField input){
        
    }
}
