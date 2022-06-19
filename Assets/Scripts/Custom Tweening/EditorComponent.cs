using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EditorComponent : MonoBehaviour
{
    public int componentIndex = 0;

    EditorManager editorManager;
    AnimationComponent component;

    [SerializeField] GameObject animationType;
    TMP_Dropdown animationTypeDropdown;

    [SerializeField] GameObject easeType;
    TMP_Dropdown easeTypeDropdown;
    
    TMP_InputField transformValueX;
    TMP_InputField transformValueY;
    TMP_InputField transformValueZ;
    

    // Start is called before the first frame update
    void Start()
    {
        
        editorManager = gameObject.GetComponentInParent<EditorManager>();
        animationTypeDropdown = animationType.GetComponent<TMP_Dropdown>();

        easeTypeDropdown = easeType.GetComponent<TMP_Dropdown>();
        easeTypeDropdown.AddOptions(System.Enum.GetNames(typeof(EaseTypes)).ToList());

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
            ChangeFieldValue(ref component.values.x, transformValueX);
        });
    }

    void InitializeValues(){
        animationTypeDropdown.value = (int) component.animType;
        easeTypeDropdown.value = (int) component.easeType;

        transformValueX.text = component.values.x.ToString();
        transformValueY.text = component.values.y.ToString();
        transformValueZ.text = component.values.z.ToString();

    }

    void AnimationTypeDropdownChanged(TMP_Dropdown change){
        component.animType = (AnimationTypes) change.value;
        ReloadAnimation();
    }

    void ChangeFieldValue(ref float p, TMP_InputField input){
        p = int.Parse(input.text);
        ReloadAnimation();
    }

    void ReloadAnimation(){
        if(editorManager.useAnimationKeys.isPlaying) editorManager.useAnimationKeys.PlayAnimation();
    }
}
