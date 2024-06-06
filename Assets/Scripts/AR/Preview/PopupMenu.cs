using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : MonoBehaviour
{
    public Image image;
    public Text description;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject[] hideWhenOpen;

    public bool IsOpen { get; private set; } = false;

    void Start()
    {
        SetMenu(false);
    }

    public void SetMenu(bool open)
    {
        // set is open public property
        IsOpen = open;
        // set menu
        menu.SetActive(open);
        // toggle other objects
        foreach (GameObject obj in hideWhenOpen)
        {
            obj.SetActive(!open);
        }
    }
}
