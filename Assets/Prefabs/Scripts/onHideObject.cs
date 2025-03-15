using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onHideObject : MonoBehaviour
{
    GameObject targetObject;
    public void onHide()
    {
        targetObject.SetActive(false);
    }
}
