using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIAnimations : MonoBehaviour
{
    UIDocument animationPanel;
    public List<ComponentsForUI> components = new List<ComponentsForUI>();

    private void Start() {
        animationPanel = GetComponent<UIDocument>();
        foreach(ComponentsForUI component in components){
            component.playButton = animationPanel.rootVisualElement.Q<Button>(component.buttonName);
            component.playButton.clicked += component.key.PlayAnimation;
        }
        
    }
}


[System.Serializable]
public class ComponentsForUI{
    public UseAnimationKeys key;
    public Button playButton;
    public string buttonName;
}
