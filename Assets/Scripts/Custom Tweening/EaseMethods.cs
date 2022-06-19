using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaseMethods : MonoBehaviour
{
	float t = 1.0f;

    float e = 3.0f;

    float result;

    public float Easing(EaseTypes ease, float t){

        float amp = 1.0f;
        float per = 0.3f;

        switch(ease){

            case EaseTypes.Linear:{
                result = t;
            }
            break;

            case EaseTypes.EasyEase:{
                t *= 2;
                if(t <= 1.0f) result = Mathf.Pow(t, 2);
                else result = (2 - Mathf.Pow(2 - t, 2));
                result /= 2;
            }
            break;

            // from AE expressions
            case EaseTypes.Elastic:{
                float ampl = 0.05f;
                float freq = 4.0f;
                float decay = 8.0f;

                result = ampl * Mathf.Sin(freq * t * 2 * Mathf.PI) / Mathf.Exp(decay * t);
            }
            break;

            case EaseTypes.Back:{
                result = t * t * (e * (t - 1) + t);
            }
            break;

            // e is power defaults to 3

            case EaseTypes.PolyIn:{
                result = Mathf.Pow(t, e);
            }
            break;

            case EaseTypes.PolyOut:{
                result = 1 - Mathf.Pow(1 - t, e);
            }
            break;

            case EaseTypes.PolyInOut:{
                t *= 2;
                if(t <= 1.0f) result = Mathf.Pow(t, e);
                else result = (2 - Mathf.Pow(2 - t, e));
                result /= 2;
            }
            break;

            // e is exp basis defaults to 2

            case EaseTypes.ExpIn:{
                result = Mathf.Pow(e, -10 * (1 - t));
            }
            break;

            case EaseTypes.ExpOut:{
                result = 1 - Mathf.Pow(e, -10 * (t));
            }
            break;

            case EaseTypes.ExpInOut:{
                t *= 2;
                if(t <= 1.0f) result = Mathf.Pow(e, -10 * (1 - t));
                else result = 2 - Mathf.Pow(e, -10 * (t - 1));
                result /= 2;
            }
            break;

            case EaseTypes.SinIn:{
                if(t==1) return 1;
                else result = 1 - Mathf.Cos(t * Mathf.PI / 2);
            }
            break;

            case EaseTypes.SinOut:{
                result = Mathf.Sin(t * Mathf.PI / 2);
            }
            break;

            case EaseTypes.SinInOut:{
                result = (1 - Mathf.Cos(Mathf.PI * t)) / 2;
            }
            break;

            case EaseTypes.CircleIn:{
                result = 1 - Mathf.Sqrt(1 - t * t);
            }
            break;

            case EaseTypes.CircleOut:{
                result = Mathf.Sqrt(1 - (t - 1) * t);
            }
            break;

            case EaseTypes.CircleInOut:{
                t *= 2;
                if(t <= 1.0f) result = 1 - Mathf.Sqrt(1 - t * t);
                else result = Mathf.Sqrt(1 - (t - 2) * (t - 2)) + 1;
                result /= 2;
            }
            break;

            // e defaults to 1

            case EaseTypes.ElasticIn:{
                var s = Mathf.Asin(1 / (amp = Mathf.Max(1, amp))) * (per /= 2 * Mathf.PI);

                result = amp * Mathf.Pow(2, -10 * -(--t)) * Mathf.Sin((s - t) / per);
            }
            break;

            case EaseTypes.ElasticOut:{
                var s = Mathf.Asin(1 / (amp = Mathf.Max(1, amp))) * (per /= 2 * Mathf.PI);

                result = 1 - amp * Mathf.Pow(2, -10 * (t = +t)) * Mathf.Sin((t + s) / per);
            }
            break;

            // e is overshoot defaults to 1.5

            case EaseTypes.BackIn:{
                result = t * t * (e * (t - 1) + t);
            }
            break;
            
            case EaseTypes.BackOut:{
                result = -t * t * (e * (t + 1) + t);
            }
            break;

            case EaseTypes.BackInOut:{
                t *= 2;
                if(t <= 1.0f) result = t * t * ((e + 1) * t - e);
                else result = (t - 2) * t * ((e + 1) * t + e);
                result /= 2;
            }
            break;

        }

        return result;
    }
}
