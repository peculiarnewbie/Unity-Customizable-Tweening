using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ease : MonoBehaviour
{
    float t = 1.0f;

    float e = 3.0f;

    float result;

    public float Eas(Type ease){

        float amp = e;
        float per = 0.3f;

        switch(ease){

            // e defaults to 3

            case Type.PolyIn:{
                result = Mathf.Pow(t, e);
            }
            break;

            case Type.PolyOut:{
                result = 1 - Mathf.Pow(1 - t, e);
            }
            break;

            case Type.PolyInOut:{
                t *= 2;
                if(t <= 1.0f) result = Mathf.Pow(t, e);
                else result = (2 - Mathf.Pow(2 - t, e));
                result /= 2;
            }
            break;

            // e defaults to 2

            case Type.ExpIn:{
                result = Mathf.Pow(e, -10 * (1 - t));
            }
            break;

            case Type.ExpOut:{
                result = 1 - Mathf.Pow(e, -10 * (t));
            }
            break;

            case Type.ExpInOut:{
                t *= 2;
                if(t <= 1.0f) result = Mathf.Pow(e, -10 * (1 - t));
                else result = 2 - Mathf.Pow(e, -10 * (t - 1));
                result /= 2;
            }
            break;

            case Type.SinIn:{
                if(t==1) return 1;
                else result = 1 - Mathf.Cos(t * Mathf.PI / 2);
            }
            break;

            case Type.SinOut:{
                result = Mathf.Sin(t * Mathf.PI / 2);
            }
            break;

            case Type.SinInOut:{
                result = (1 - Mathf.Cos(Mathf.PI * t)) / 2;
            }
            break;

            case Type.CircleIn:{
                result = 1 - Mathf.Sqrt(1 - t * t);
            }
            break;

            case Type.CircleOut:{
                result = Mathf.Sqrt(1 - (1 - t) * t);
            }
            break;

            case Type.CircleInOut:{
                t *= 2;
                if(t <= 1.0f) result = 1 - Mathf.Sqrt(1 - t * t);
                else result = Mathf.Sqrt(1 - (t - 2) * t) + 1;
                result /= 2;
            }
            break;

            // e defaults to 1

            case Type.ElasticIn:{
                var s = Mathf.Asin(1 / (amp = Mathf.Max(1, amp))) * (per /= 2 * Mathf.PI);

                result = amp * Mathf.Pow(2, -10 * -(1 - t)) * Mathf.Sin((s - t) / per);
            }
            break;

            case Type.ElasticOut:{
                var s = Mathf.Asin(1 / (amp = Mathf.Max(1, amp))) * (per /= 2 * Mathf.PI);

                result = amp * Mathf.Pow(2, -10 * -(1 - t)) * Mathf.Sin((s - t) / per);
            }
            break;


            // from AE expression
            case Type.AlternateElastic:{
                float ampl = 0.05f;
                float freq = 4.0f;
                float decay = 8.0f;

                result = ampl * Mathf.Sin(freq * t * 2 * Mathf.PI) / Mathf.Exp(decay * t);
            }
            break;
        }

        return result;
    }

}

public enum Type{
    PolyIn,
    PolyOut,
    PolyInOut,

    ExpIn,
    ExpOut,
    ExpInOut,

    SinIn,
    SinOut,
    SinInOut,

    CircleIn,
    CircleOut,
    CircleInOut,

    ElasticIn,
    ElasticOut,
    ElasticInOut,

    AlternateElastic,

}
