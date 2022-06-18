using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UIElements;

public class UseAnimationKeys : MonoBehaviour
{
    public Transform targetObject;
    public AnimationKeys animationObject;

    public bool isContinuous = true;

    public bool loop = false;
    [EnableIf("loop")] public float loopDelay;
    float tempLoopDelay;

    public bool timeStretch = false;
    [EnableIf("timeStretch")] public float timeMultiplier;

    public bool useDelay;
    [EnableIf("useDelay")] public float delayOverall;
    float tempDelay;

    private List<AnimationComponent> componentList;
    bool isFirst = true;
    float duration;
    float progress;
    Transform initialValues;
    Vector3[] oldValue = new Vector3[4];
    EaseMethods ease;
    List<float> componentDuration = new List<float>();
    List<float> componentDelay = new List<float>();
    List<ComponentResult> results = new List<ComponentResult>();
    Coroutine animationCoroutine;

    private void Start() {
        ease = new EaseMethods();

        componentList = animationObject.components;

        tempLoopDelay = loopDelay;
        tempDelay = delayOverall;
    }

    IEnumerator AnimationCoroutine(){
        while(true){
            if(tempDelay > 0f){
                tempDelay -= Time.deltaTime;
                yield return null;
            }

            if(progress > -1f){
                if(progress < 0f) {progress = -2f; CalculateTransformation(true);}
                else CalculateTransformation(false);
                progress -= Time.deltaTime;
            }
            else if(tempLoopDelay > 0) tempLoopDelay -= Time.deltaTime;
            else if(loop) ResetAnimation(results);
            else yield break;
            yield return null;
        }
    }

    public void PlayAnimation(){
        duration = 0f;  //resets duration

        results.Clear(); //clear keys

        if(isFirst || isContinuous){GetCurrentTransform(); isFirst = false;} //get transform reference
        
        foreach(AnimationComponent component in componentList){
            //Convert component to a child class with temp values
            var serializedComponent = JsonUtility.ToJson(component); 
            ComponentResult c  = JsonUtility.FromJson<ComponentResult>(serializedComponent);

            if(!timeStretch) timeMultiplier = 1f;
            c.MakeTemp(timeMultiplier);

            results.Add(c);

            if(duration < (component.duration + component.delay)) duration = (component.duration + component.delay)/1000f;
            duration = progress = duration*timeMultiplier;
        }
        
        if(animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimationCoroutine());
    }

    private void GetCurrentTransform(){
        oldValue[0] = targetObject.transform.localScale;
        oldValue[1] = targetObject.transform.localPosition;
        oldValue[2] = targetObject.transform.localRotation.eulerAngles;
    }

    private void CalculateTransformation(bool last){

        Vector3 totalRatio = new Vector3(1f,1f,1f);
        Vector3 totalRelative = new Vector3(0f, 0f, 0f);
        Vector3 totalAngle = new Vector3(0f, 0f, 0f);
        float componentProgress;

        foreach(ComponentResult result in results){
            int flag = (int) result.animType;

            if(result.tempDelay > 0){
                result.tempDelay -= Time.deltaTime;
                continue;
            } 
            else if(result.tempDuration > 0 || result.notLast){
                if(result.tempDuration < 0){result.tempDuration = 0f; result.notLast = false;}
                if(last) componentProgress = 1f;
                else componentProgress = (result.duration - result.tempDuration) / (result.duration);
                result.tempDuration -= Time.deltaTime;

                switch(result.animType){
                    case AnimationTypes.Scale: 
                        result.tempRatio = Vector3.LerpUnclamped(Vector3.one, result.ratio, ease.Smooth(result.easeType, componentProgress));
                    break;
                    case AnimationTypes.Translate: 
                        result.tempRelative = Vector3.LerpUnclamped(Vector3.zero, result.relativePosition, ease.Smooth(result.easeType, componentProgress));
                        flag = 1;
                    break;
                    case AnimationTypes.Rotate:
                        result.tempRotation = Vector3.LerpUnclamped(Vector3.zero, result.degrees, ease.Smooth(result.easeType, componentProgress));
                        flag = 2;
                    break;
                    case AnimationTypes.Skew:
                    { 
                        
                    }
                    break;
                }

            }
            if(flag == 0) totalRatio = Vector3.Scale(totalRatio, result.tempRatio);
            else if(flag == 1) totalRelative += result.tempRelative;
            else if(flag ==2) totalAngle += result.tempRotation;
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
        progress = duration;
        tempLoopDelay = loopDelay;
        tempDelay = delayOverall;
        if(isContinuous) GetCurrentTransform();
    }


    public class ComponentResult : AnimationComponent{
        public float tempDuration;
        public float tempDelay;
        public Vector3 tempRelative = new Vector3(0f, 0f, 0f);
        public Vector3 tempRatio = new Vector3(1f, 1f, 1f);
        public Vector3 tempRotation = new Vector3(0f, 0f, 0f);

        public bool notLast = true;

        public void MakeTemp(float multiplier){
            duration = duration*multiplier/1000f;
            delay = delay*multiplier/1000f;
            ResetTemp();
        }
        public void ResetTemp(){
            tempDuration = duration;
            tempDelay = delay;
            tempRatio = ratio;
            tempRelative = relativePosition;
            tempRotation = degrees;
            notLast = true;
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
