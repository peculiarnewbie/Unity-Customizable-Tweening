using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorComponent : MonoBehaviour
{
    public int componentIndex = 0;

    EditorManager editorManager;
    AnimationComponent component;

    [SerializeField] GameObject animationType;
    TMP_Dropdown animationTypeDropdown;
    
    TMP_InputField transformValueX;
    TMP_InputField transformValueY;
    TMP_InputField transformValueZ;
    

    // Start is called before the first frame update
    void Start()
    {
        
        editorManager = gameObject.GetComponentInParent<EditorManager>();
        animationTypeDropdown = animationType.GetComponent<TMP_Dropdown>();

        GameObject vector3 = this.transform.Find("Vector 3").gameObject;
        transformValueX = vector3.transform.GetChild(0).GetComponent<TMP_InputField>();
        transformValueY = vector3.transform.GetChild(1).GetComponent<TMP_InputField>();
        transformValueZ = vector3.transform.GetChild(2).GetComponent<TMP_InputField>();
        
        SetupListeners();

        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
     {
         yield return new WaitForSeconds(waitTime);

        component = editorManager.animationObject.components[componentIndex];
         InitializeValues();
     }

    void SetupListeners(){
        animationTypeDropdown.onValueChanged.AddListener(delegate {
            AnimationTypeDropdownChanged(animationTypeDropdown);
        });
        transformValueX.onEndEdit.AddListener(delegate{

        });
    }

    void InitializeValues(){
        animationTypeDropdown.value = (int) component.animType;

        transformValueX.text = component.relativePosition.x.ToString();
        transformValueY.text = component.relativePosition.y.ToString();
        transformValueZ.text = component.relativePosition.z.ToString();

    }

    void AnimationTypeDropdownChanged(TMP_Dropdown change){
        Debug.Log(change.value);
        editorManager.ChangeAnimationType(componentIndex, change.value);
    }

    void TransformValueChanged(int dimension, TMP_InputField input){
        
    }
}
