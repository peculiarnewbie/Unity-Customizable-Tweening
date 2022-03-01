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

    [SerializeField] private UnityEngine.UIElements.UIDocument mainUI;
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
    Vector3[] oldValue = new Vector3[4];
    EaseMethods ease;
    List<float> componentDuration = new List<float>();
    List<float> componentDelay = new List<float>();
    List<ComponentResult> results = new List<ComponentResult>();

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

            //targetObject.localScale = Vector3.LerpUnclamped(oldValue, targetValue, ease.Smooth(EaseTypes.Elastic, (duration-progress)/duration));
            RunScaleObject();
            progress -= Time.deltaTime;
        }
        else isPlaying = !isPlaying;
    }

    [Button(enabledMode: EButtonEnableMode.Always)]
    public void PlayAnimation(){

        initialValues = targetObject.transform;
        duration = 0f;  //resets duration

        results.Clear();

        oldValue[0] = targetObject.transform.localScale;
        oldValue[1] = targetObject.transform.localPosition;

        foreach(AnimationComponent component in componentList){
            var serializedComponent = JsonUtility.ToJson(component); 
            ComponentResult c  = JsonUtility.FromJson<ComponentResult>(serializedComponent);
            c.MakeTemp();
            results.Add(c);
            //CopyComponent(component);

            if(duration < (component.duration + component.delay)) duration = (component.duration + component.delay)/1000f;
            progress = duration;
        }
        
        isPlaying = true;
    }

    private void CopyComponent(AnimationComponent values){
        ComponentResult temp = new ComponentResult();
        temp.tempDuration = temp.duration = values.duration/1000f;
        temp.tempDelay = temp.delay = values.delay/1000f;
        temp.tempRatio = temp.ratio = values.ratio;
        results.Add(temp);
    }

    

    private void RunScaleObject(){

        Vector3 totalRatio = new Vector3(1f,1f,1f);
        Vector3 totalRelative = new Vector3(0f, 0f, 0f);
        float componentProgress;

        foreach(ComponentResult result in results){
            int flag = (int) result.animType;
            if(result.tempDelay > 0){
                result.tempDelay -= Time.deltaTime;
                continue;
            } 
            else if(result.tempDuration > 0){

                componentProgress = (result.duration - result.tempDuration) / (result.duration);
                result.tempDuration -= Time.deltaTime;

                switch(result.animType){
                    case AnimationTypes.Scale: 
                        {result.tempRatio = Vector3.LerpUnclamped(Vector3.one, result.ratio, ease.Smooth(EaseTypes.Elastic, componentProgress));}
                    break;
                    case AnimationTypes.Translate: 
                        {result.tempRelative = Vector3.LerpUnclamped(Vector3.zero, result.relativePosition, ease.Smooth(EaseTypes.Elastic, componentProgress));}
                        flag = 1;
                    break;
                    case AnimationTypes.Rotate:
                    {
                        
                    }
                    break;
                    case AnimationTypes.Skew:
                    { 
                        
                    }
                    break;
                }

            }
            if(flag == 0) totalRatio = Vector3.Scale(totalRatio, result.tempRatio);
            else if(flag == 1) totalRelative += result.tempRelative;
        }
        targetObject.transform.localScale = Vector3.Scale(oldValue[0], totalRatio);
        targetObject.transform.localPosition = oldValue[1] + totalRelative;
        
    }

    private Vector3 ScaleObject(ComponentResult values, bool isRatio){
        
        return values.tempRatio;
    }


    private void RunTranslateObject(){

        Vector3 totalRealtive = new Vector3(0f, 0f, 0f);

        foreach(ComponentResult result in results){

        }
    }


    public class ComponentResult : AnimationComponent{
        public float tempDuration;
        public float tempDelay;
        public Vector3 tempRelative;
        public Vector3 tempRatio = new Vector3(1f, 1f, 1f);

        public void MakeTemp(){
            duration = tempDuration = duration/1000f;
            delay = tempDelay = delay/1000f;
            tempRatio = ratio;
            tempRelative = relativePosition;
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
