using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UIElements;

public class UseAnimationKeys : MonoBehaviour
{
    public Transform targetObject;
    public AnimationKeys animationObject;

    private List<AnimationComponent> componentList;

    public bool loop;
    [EnableIf("loop")] public float loopDelay;

    public bool slowMo;
    [EnableIf("slowMo")] public float slowMultiplier;

    [SerializeField] private UIDocument mainUI;
    public Button playAnimation;
    public string buttonName;

    int stiffness = 3;
    int bounces = 4;
    float treshold;
    float alpha;
    float limit;
    float omega;

    bool isPlaying;
    float duration;
    float progress;
    Transform initialValues;
    Vector3 oldValue;
    Vector3 targetValue;
    EaseMethods ease;

    private void Start() {
        componentList = animationObject.components;
        treshold = 0.005f / Mathf.Pow(10, stiffness);
        alpha = stiffness / 100f;
        limit = Mathf.Floor(Mathf.Log(treshold) / -alpha);
        omega = (bounces + 0.5f) * Mathf.PI/limit;

        ease = GetComponent<EaseMethods>();

        playAnimation = mainUI.rootVisualElement.Q<Button>(buttonName);
        playAnimation.clicked += PlayAnimation;
    }

    private void Update() {
        if(!isPlaying) return;

        if(progress > 0f){

            targetObject.localScale = Vector3.LerpUnclamped(oldValue, targetValue, ease.Smooth(EaseTypes.Elastic, (duration-progress)/duration));

            progress -= Time.deltaTime;
        }
        else isPlaying = !isPlaying;
    }

    [Button(enabledMode: EButtonEnableMode.Always)]
    public void PlayAnimation(){

        initialValues = targetObject.transform;
        duration = 0f;  //resets duration

        foreach(AnimationComponent component in animationObject.components){
            switch(component.animType){
                case AnimationTypes.Scale:
                {
                    ScaleObject(component, component.useRatio);
                }
                break;
                case AnimationTypes.Translate:
                {
                    TranslateObject(component, component.useRelative);
                }
                break;
                case AnimationTypes.Rotate:
                {
                    RotateObject(component);
                }
                break;
                case AnimationTypes.Skew:
                {
                    SkewObject(component);
                }
                break;
            }
            if(duration < (component.duration + component.delay)) duration = (component.duration + component.delay)/1000;
            progress = duration;
        }
        
        isPlaying = true;
    }

    private void ScaleObject(AnimationComponent values, bool isRatio){
        if(isRatio){
            oldValue = targetObject.transform.localScale;
            targetValue = Vector3.Scale(oldValue, values.ratio);
        }
        else{
            oldValue = values.startScale;
            targetValue = values.endScale;
        }
    }

    private void RunScaleObject(){

    }

    private void TranslateObject(AnimationComponent values, bool isRelative){
        if(isRelative){
            oldValue = targetObject.transform.position;
            targetValue = oldValue + values.relativePosition;
        }
        else{
            oldValue = values.startPosition;
            targetValue = values.endPosition;
        }

    }
    private void RotateObject(AnimationComponent values){

    }
    private void SkewObject(AnimationComponent values){

    }

    private float Calculate(float ratio){
        if(ratio >= 1) return 1;

        float t = ratio * limit;
        return 1 - Exponent(t) * Oscillation(t);
    }

    private float Exponent(float t){
        return Mathf.Pow(Mathf.Epsilon, alpha*t);
    }

    private float Oscillation(float t){
        return Mathf.Cos(omega * t);
    }

}
