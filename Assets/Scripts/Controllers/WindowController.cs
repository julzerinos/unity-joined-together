using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    private GameObject _window;

    private void Awake()
    {
        _window = transform.Find("Background").gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            _window.SetActive(!_window.activeSelf);
    }
}