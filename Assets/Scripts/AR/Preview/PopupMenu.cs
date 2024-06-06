using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : MonoBehaviour
{
    public Image image;
    public Text description;
    public GameObject menu;

    public void CloseMenu()
    {
        menu.SetActive(false);
    }
}
