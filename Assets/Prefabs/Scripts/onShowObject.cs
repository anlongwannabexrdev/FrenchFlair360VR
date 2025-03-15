using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onShowObject : MonoBehaviour
{
    public GameObject targetObject;

    void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); // Đảm bảo ban đầu GameObject bị tắt
        }

    }

    public void ToggleVisibility()
    {
        targetObject.SetActive(true);
    }
}
