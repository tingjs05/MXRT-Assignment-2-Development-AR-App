using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BottomNavigationManager : MonoBehaviour
{
    [SerializeField] float selectedOffset = 75f;
    [SerializeField] RectTransform selectedHighlight;
    [SerializeField] RectTransform[] icons;
    [SerializeField] Color defaultColor, selectedColor;

    // array to store image components
    Image[] images;

    // handle communication with other scripts
    public enum NavPages
    {
        BOOKING, 
        ATTRACTIONS, 
        HOME, 
        INFORMATION, 
        GAMES
    }

    public event Action<NavPages> PageChange;

    void Start()
    {
        // set images array
        images = new Image[icons.Length];

        for (int i = 0; i < icons.Length; i++)
        {
            images[i] = icons[i].GetComponent<Image>();
        }

        // select default
        Select(2);
    }

    public void Select(int index)
    {
        // ensure index is within range
        if (index >= icons.Length) return;

        // set all icon positions
        for (int i = 0; i < icons.Length; i++)
        {
            // reset icon if not icon to select
            if (i != index)
            {
                icons[i].localPosition = new Vector3(icons[i].localPosition.x, 0, icons[i].localPosition.z);
                images[i].color = defaultColor;
                continue;
            }

            // set icon localPosition
            icons[i].localPosition = new Vector3(icons[i].localPosition.x, selectedOffset, icons[i].localPosition.z);
            images[i].color = selectedColor;
            // set localPosition of selected highllight
            selectedHighlight.localPosition = new Vector3(icons[i].localPosition.x, selectedHighlight.localPosition.y, selectedHighlight.localPosition.z);
        }

        // invoke page changed event
        PageChange?.Invoke((NavPages) index);
    }
}
