using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//This script is added to the text component of a UI element

public class button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Text txtStart;
    public Color newColour;     
    public Color originalColour;

    public string LoadLevel;

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


        print("Load: " + LoadLevel);
        if (!String.IsNullOrEmpty(LoadLevel))
        {
            SceneManager.UnloadSceneAsync(0);
            SceneManager.LoadScene(LoadLevel);
        }
    }

    public void OnPointerExit(PointerEventData e)
    {
        txtStart.color = originalColour;
    }
}