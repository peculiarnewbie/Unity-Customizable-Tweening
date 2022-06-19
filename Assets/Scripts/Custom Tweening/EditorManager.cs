using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    [SerializeField] GameObject keyPrefab;
    public AnimationKeys animationObject;
    public UseAnimationKeys useAnimationKeys;

    

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<animationObject.components.Count; i++){
            Transform content = this.transform.Find("Horizontal/Vertical/Scroll View/Viewport/Content").transform;
            GameObject tempPrefab = Instantiate(keyPrefab, content);
            EditorComponent temp = tempPrefab.GetComponent<EditorComponent>();
            temp.componentIndex = i;
        }
    }

    public void ChangeValue(int componentIndex){

    }

    public void ChangeAnimationType(int componentIndex, int value){
        animationObject.components[componentIndex].animType = (AnimationTypes)value;
    }

    public void ChangeTransformValue(int componentIndex, int dimension, int value){
        // animationObject.components[componentIndex].
    }

    public void PlayAnimation(){
        useAnimationKeys.PlayAnimation();
    }
}
