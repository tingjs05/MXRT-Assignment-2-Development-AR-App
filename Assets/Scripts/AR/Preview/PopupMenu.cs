using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : MonoBehaviour
{
    public Image image;
    public Text description;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject[] hideWhenOpen;
    [SerializeField] SoundManager soundManager;

    public bool IsOpen { get; private set; } = false;

    void Start()
    {
        SetMenu(false);
    }

    void Update()
    {
        if (IsOpen || menu.activeSelf || !backButton.activeSelf) return;
        backButton.SetActive(false);
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
        // only take these actions when opening menu
        if (!open) return;
        // set back button
        backButton.SetActive(true);
        // play sfx
        soundManager?.PlayClick(2);
    }
}
