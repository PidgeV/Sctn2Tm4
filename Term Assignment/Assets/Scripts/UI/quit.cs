using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//This script is added to the text component of a UI element

public class quit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Text txtStart;
    public Color newColour;
    public Color originalColour;


    void Start()
    {
        txtStart = GetComponent<Text>();
        originalColour = txtStart.color;

    }

    public void OnPointerEnter(PointerEventData e)
    {
        txtStart.color = newColour;
    }

    public void OnPointerClick(PointerEventData e)
    {
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    public void OnPointerExit(PointerEventData e)
    {
        txtStart.color = originalColour;
    }
}