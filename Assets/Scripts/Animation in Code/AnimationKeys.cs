using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AnimationKeys", order = 1)]
public class AnimationKeys : ScriptableObject
{
    public string animationName;
    public List<KeyData> keyFrames = new List<KeyData>();

    [System.Serializable]
    public struct KeyData{
        public float keyTimes;
        public Vector3 value;
        /*
        public KeyData(int frame, float keyTimes, Vector3 value){

        }
        */
    }

    public bool loop;
    [EnableIf("loop")] public float loopDelay;

    public List<AnimationComponent> components = new List<AnimationComponent>();

    public AnimationComponent defaultVaules;
    
    public NestingTest nest1;

    [Button(enabledMode: EButtonEnableMode.Always)]
    private void UpdateValues(){

    }
}

[System.Serializable]
public class AnimationComponent{
    // [Dropdown("AnimationType")]
    // public string animationType;

    // private List<string> AnimationType { get { return new List<string>() { "Scale", "Translate", "Rotate", "Skew" }; } }

    public AnimationTypes animType;

    [ShowIf("animType", AnimationTypes.Scale)][AllowNesting] public bool useRatio = true;
    [ShowIf("animType", AnimationTypes.Scale)][EnableIf("useRatio")][AllowNesting] public Vector3 ratio = new Vector3(1f,2f, 1f);
    [ShowIf("animType", AnimationTypes.Scale)][DisableIf("useRatio")][AllowNesting] public Vector3 startScale = new Vector3(1f,2f, 1f);
    [ShowIf("animType", AnimationTypes.Scale)][DisableIf("useRatio")][AllowNesting] public Vector3 endScale = new Vector3(1f,2f, 1f);

    [ShowIf("animType", AnimationTypes.Translate)][AllowNesting] public bool useRelative = true;
    [ShowIf("animType", AnimationTypes.Translate)][EnableIf("useRelative")][AllowNesting] public Vector3 relativePosition = new Vector3(0f,100f, 0f);
    [ShowIf("animType", AnimationTypes.Translate)][DisableIf("useRelative")][AllowNesting] public Vector3 startPosition = new Vector3(0f,0f, 0f);
    [ShowIf("animType", AnimationTypes.Translate)][DisableIf("useRelative")][AllowNesting] public Vector3 endPosition = new Vector3(0f,0f, 0f);

    [ShowIf("animType", AnimationTypes.Rotate)][AllowNesting] public Vector3 degrees = new Vector3(0f,90f, 0f);

    [ShowIf("animType", AnimationTypes.Skew)][AllowNesting] public Vector2 xDegree = new Vector2(0f,20f);
    [ShowIf("animType", AnimationTypes.Skew)][AllowNesting] public Vector2 yDegree = new Vector2(0f,0f);

    public EasingTypes easeType;

    [Label("Duration (ms)")][AllowNesting] public int duration = 1000;
    [Label("Delay (ms)")][AllowNesting] public int delay = 0;
    public int bounces = 4;
    public int stiffness = 3;

}

[System.Serializable]
public class NestingTest{
    [InfoBox("Error", EInfoBoxType.Error)]
        public int error;
}

public enum AnimationTypes
{
    Scale,
    Translate,
    Rotate,
    Skew
}

public enum EasingTypes
{
    Bounce,
    Sway,
    HardBounce,
    HardSway
}