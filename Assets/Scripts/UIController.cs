using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public AnimationKeys jellyValues;
    public Transform forAnimation;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("animate-button");
        quitButton = root.Q<Button>("ratio-button");

        startButton.clicked += StartButtonPressed;
        quitButton.clicked += Quit;
    }

    void StartButtonPressed(){
        // StartCoroutine(JellyAnimation());
    }

    void Quit(){
        Application.Quit();
    }

    // IEnumerator JellyAnimation(){
    //     /* Generated with Bounce.js. Edit at http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A1%2Cy%3A1%7D%2Ct%3A%7Bx%3A2%2Cy%3A1%7D%2Cs%3A1%2Cb%3A4%7D%2C%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A1%2Cy%3A1%7D%2Ct%3A%7Bx%3A1%2Cy%3A2%7D%2Cs%3A1%2Cb%3A6%7D%5D%7D */
    //     float animationProgress = 0f;
    //     int i = 0;
    //     float[] keyTimes = new float[jellyValues.keyFrames.Count];
    //     Vector3[] UIScales = new Vector3[jellyValues.keyFrames.Count];
    //     int currentFrame = 1;
    //     float UISpeed = 100f;

    //     foreach(AnimationKeys.KeyData key in jellyValues.keyFrames){
    //         keyTimes[i] = key.keyTimes;
    //         UIScales[i] = key.value;
    //         i++;
    //     }

    //     while(animationProgress <= 100f){
    //         if(animationProgress > keyTimes[currentFrame]) currentFrame+=1;

    //         float lerpTime = (keyTimes[currentFrame+1]-keyTimes[currentFrame])/(keyTimes[currentFrame+1]-keyTimes[currentFrame-1]);
    //         Vector3 UIScale = Vector3.Lerp(UIScales[currentFrame], UIScales[currentFrame+1], lerpTime);
    //         forAnimation.transform.localScale = UIScale;
    //         animationProgress += Time.deltaTime*UISpeed;
    //         yield return new WaitForSeconds(0.001f);

    //         Debug.Log(forAnimation.transform.localScale.x);
    //     }
    //     yield return null;
    // }
}
