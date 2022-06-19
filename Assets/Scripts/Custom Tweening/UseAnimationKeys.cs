using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UIElements;

public class UseAnimationKeys : MonoBehaviour
{
    public Transform targetObject;
    public AnimationKeys animationObject;

    public bool playOnStart = false;

    public bool isContinuous = true;

    public bool loop = false;
    [EnableIf("loop")] public float loopDelay;
    float tempLoopDelay;

    public bool timeStretch = false;
    [EnableIf("timeStretch")] public float timeMultiplier;

    public bool useDelay;
    [EnableIf("useDelay")] public float delayOverall;
    float tempDelayOverall;

    private List<AnimationComponent> componentList;
    bool isFirst = true;
    float totalDuration;
    float progress;
    Transform initialValues;
    Vector3[] oldValue = new Vector3[4];
    EaseMethods ease;
    List<float> componentDuration = new List<float>();
    List<float> componentDelay = new List<float>();
    List<ComponentResult> results = new List<ComponentResult>();
    Coroutine animationCoroutine;

    public bool isPlaying;

    private void Start() {
        ease = new EaseMethods();

        componentList = animationObject.components;

        tempLoopDelay = loopDelay;
        tempDelayOverall = delayOverall;

        if(playOnStart) PlayAnimation();
    }

    public void PlayAnimation(){
        totalDuration = 0f;  //resets duration
        tempDelayOverall = delayOverall;

        results.Clear(); //clear keys

        if(isFirst || isContinuous){GetCurrentTransform(); isFirst = false;} //get transform reference
        
        //Convert component to a child class with temp values
        foreach(AnimationComponent component in componentList){
            var serializedComponent = JsonUtility.ToJson(component); 
            ComponentResult c  = JsonUtility.FromJson<ComponentResult>(serializedComponent);

            if(!timeStretch) timeMultiplier = 1f;
            c.MakeTemp(timeMultiplier);

            results.Add(c);

            if(totalDuration < (component.duration + component.delay)/1000) totalDuration = (component.duration + component.delay)/1000f;
            
        }

        totalDuration = progress = totalDuration*timeMultiplier;
        
        if(animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimationCoroutine());
        isPlaying = true;
    }

    private void GetCurrentTransform(){
        oldValue[0] = targetObject.transform.localScale;
        oldValue[1] = targetObject.transform.localPosition;
        oldValue[2] = targetObject.transform.localRotation.eulerAngles;
    }

    IEnumerator AnimationCoroutine(){
        while(true){
            if(tempDelayOverall > 0f && useDelay){
                tempDelayOverall -= Time.deltaTime;
            }

            else if(progress > -1f){
                if(progress < 0f) {progress = -2f;} // one more frame after reaching zero
                CalculateTransformation();
                progress -= Time.deltaTime;
            }
            else if(tempLoopDelay > 0) tempLoopDelay -= Time.deltaTime;
            else if(loop) ResetAnimation(results);
            else{
                isPlaying = false;
                yield break;
            } 
            yield return null;
        }
        
    }

    private void CalculateTransformation(){

        Vector3 totalRatio = new Vector3(1f,1f,1f);
        Vector3 totalRelative = new Vector3(0f, 0f, 0f);
        Vector3 totalAngle = new Vector3(0f, 0f, 0f);
        float componentProgress;

        foreach(ComponentResult result in results){

            if(result.tempDelay > 0){
                result.tempDelay -= Time.deltaTime;
                continue;
            } 
            else{
                if(result.tempDuration < 0) componentProgress = 1f; // make sure doesn't overshoot and went back
                // if(last) componentProgress = 1f;
                else componentProgress = (result.duration - result.tempDuration) / (result.duration);
                result.tempDuration -= Time.deltaTime;

                // if scale, start from 1 not 0
                if(result.animType == AnimationTypes.Scale) result.tempValue = Vector3.LerpUnclamped(Vector3.one, result.values, ease.Easing(result.easeType, componentProgress));
                else result.tempValue = Vector3.LerpUnclamped(Vector3.zero, result.values, ease.Easing(result.easeType, componentProgress));

                switch(result.animType){
                    case AnimationTypes.Scale: 
                        totalRatio = Vector3.Scale(totalRatio, result.tempValue);
                    break;
                    case AnimationTypes.Translate: 
                        totalRelative += result.tempValue;
                    break;
                    case AnimationTypes.Rotate:
                        totalAngle += result.tempValue;
                    break;
                }
            }
        }
        TransformObject(totalRatio, totalRelative, totalAngle);

    }

    private void TransformObject(Vector3 ratio, Vector3 translation, Vector3 angle){
        targetObject.transform.localScale = Vector3.Scale(oldValue[0], ratio);
        targetObject.transform.localPosition = oldValue[1] + translation; // doing translation this way allows for continous and repeat
        targetObject.localEulerAngles = oldValue[2] + angle;
    }

    private void ResetAnimation(List<ComponentResult> components){
        foreach(ComponentResult component in components){
            component.ResetTemp();
        }
        progress = totalDuration;
        tempLoopDelay = loopDelay;
        if(isContinuous) GetCurrentTransform();
    }


    public class ComponentResult : AnimationComponent{
        public float tempDuration;
        public float tempDelay;
        public Vector3 tempValue = new Vector3(0f, 0f, 0f);

        public void MakeTemp(float multiplier){
            duration = duration*multiplier/1000f;
            delay = delay*multiplier/1000f;
            ResetTemp();
        }
        public void ResetTemp(){
            tempDuration = duration;
            tempDelay = delay;
            tempValue = values;
        }

    }

    // private float Calculate(float ratio){
    //     if(ratio >= 1) return 1;

    //     float t = ratio * limit;
    //     return 1 - Exponent(t) * Oscillation(t);
    // }

    // private float Exponent(float t){
    //     return Mathf.Pow(Mathf.Epsilon, alpha*t);
    // }

    // private float Oscillation(float t){
    //     return Mathf.Cos(omega * t);
    // }

}
