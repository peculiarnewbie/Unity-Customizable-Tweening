using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    [SerializeField] GameObject keyPrefab;
    [SerializeField] AnimationKeys animationObject;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<animationObject.components.Count; i++){
            GameObject tempPrefab = Instantiate(keyPrefab, gameObject.transform);
            EditorComponent temp = tempPrefab.GetComponent<EditorComponent>();
            temp.componentIndex = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeValue(int componentIndex){

    }

    public void ChangeAnimationType(int componentIndex, int value){
        animationObject.components[componentIndex].animType = (AnimationTypes)value;
    }

    public void ChangeTransformValue(int componentIndex, int dimension, int value){
        // animationObject.components[componentIndex].
    }
}
