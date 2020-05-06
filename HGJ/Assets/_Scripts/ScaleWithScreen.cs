using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithScreen : MonoBehaviour
{
    void Update()
    {
#if UNITY_EDITOR || UNITY_IOS
        transform.localScale = new Vector3(Screen.width / 330f, Screen.height / 630f, 1);
#endif
    }
}
