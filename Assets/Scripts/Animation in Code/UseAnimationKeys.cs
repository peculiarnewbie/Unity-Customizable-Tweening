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

    public bool isContinuous = true;

    public bool loop = false;
    [EnableIf("loop")] public float loopDelay;
    float tempLoopDelay;

    public bool timeStretch = false;
    [EnableIf("timeStretch")] public float timeMultiplier;
    public bool useDelay;
    [EnableIf("useDelay")] public float delayOverall;
    float tempDelay;

    int stiffness = 3;
    int bounces = 4;
    float treshold;
    float alpha;
    float limit;
    float omega;

    bool isPlaying;
    bool isFirst = true;
    float duration;
    float progress;
    Transform initialValues;
    Vector3[] oldValue = new Vector3[4];
    EaseMethods ease;
    List<float> componentDuration = new List<float>();
    List<float> componentDelay = new List<float>();
    List<ComponentResult> results = new List<ComponentResult>();

    private void Start() {
        ease = new EaseMethods();

        componentList = animationObject.components;
        treshold = 0.005f / Mathf.Pow(10, stiffness);
        alpha = stiffness / 100f;
        limit = Mathf.Floor(Mathf.Log(treshold) / -alpha);
        omega = (bounces + 0.5f) * Mathf.PI/limit;

        tempLoopDelay = loopDelay;
        tempDelay = delayOverall;
    }

    private void Update() {
        if(!isPlaying) return;

        if(tempDelay > 0f){
            tempDelay -= Time.deltaTime;
            return;
        }

        if(progress > 0f){
            TransformObject();
            progress -= Time.deltaTime;
        }
        else if(tempLoopDelay > 0) tempLoopDelay -= Time.deltaTime;
        else if(loop) ResetAnimation(results);
        else isPlaying = !isPlaying;
    }

    [Button(enabledMode: EButtonEnableMode.Always)]
    public void PlayAnimation(){

        initialValues = targetObject.transform;
        duration = 0f;  //resets duration

        results.Clear();

        if(isFirst || isContinuous){GetCurrentTransform(); isFirst = false;}
        

        foreach(AnimationComponent component in componentList){
            if(!timeStretch) timeMultiplier = 1f;

            var serializedComponent = JsonUtility.ToJson(component); 
            ComponentResult c  = JsonUtility.FromJson<ComponentResult>(serializedComponent);
            c.MakeTemp(timeMultiplier);
            results.Add(c);
            //CopyComponent(component);

            if(duration < (component.duration + component.delay)) duration = (component.duration + component.delay)/1000f;
            duration = progress = duration*timeMultiplier;
        }
        
        isPlaying = true;
    }

    private void GetCurrentTransform(){
        oldValue[0] = targetObject.transform.localScale;
        oldValue[1] = targetObject.transform.localPosition;
        oldValue[2] = targetObject.transform.localRotation.eulerAngles;
    }

    private void TransformObject(){

        Vector3 totalRatio = new Vector3(1f,1f,1f);
        Vector3 totalRelative = new Vector3(0f, 0f, 0f);
        Vector3 totalAngle = new Vector3(0f, 0f, 0f);
        Vector3 prevAngle = new Vector3(0f, 0f, 0f);
        float componentProgress;

        foreach(ComponentResult result in results){
            int flag = (int) result.animType;
            prevAngle += result.tempRotation;

            if(result.tempDelay > 0){
                result.tempDelay -= Time.deltaTime;
                continue;
            } 
            else if(result.tempDuration > 0){

                componentProgress = (result.duration - result.tempDuration) / (result.duration);
                result.tempDuration -= Time.deltaTime;

                switch(result.animType){
                    case AnimationTypes.Scale: 
                        result.tempRatio = Vector3.LerpUnclamped(Vector3.one, result.ratio, ease.Smooth(EaseTypes.Elastic, componentProgress));
                    break;
                    case AnimationTypes.Translate: 
                        result.tempRelative = Vector3.LerpUnclamped(Vector3.zero, result.relativePosition, ease.Smooth(EaseTypes.Elastic, componentProgress));
                        flag = 1;
                    break;
                    case AnimationTypes.Rotate:
                        result.tempRotation = Vector3.LerpUnclamped(Vector3.zero, result.degrees, ease.Smooth(EaseTypes.Elastic, componentProgress));
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
        targetObject.transform.localScale = Vector3.Scale(oldValue[0], totalRatio);
        targetObject.transform.localPosition = oldValue[1] + totalRelative; // doing translation this way allows for continous and repeat
        targetObject.localEulerAngles = oldValue[2] + totalAngle;

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
        }

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
