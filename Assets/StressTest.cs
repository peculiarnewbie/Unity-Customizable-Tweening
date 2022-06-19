using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressTest : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 60; i++){
            for(int j = 0; j < 60; j++){
                float x = (float) i*2;
                float z = (float) j*2;
                Vector3 position = new Vector3(x, 0f, z);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);

                UseAnimationKeys key;
                key = cube.GetComponent<UseAnimationKeys>();
                key.useDelay = true;
                key.delayOverall = ((float)i+ (float) j)/30;
            }
        }
    }
}
