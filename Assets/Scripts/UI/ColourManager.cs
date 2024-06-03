using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourManager : MonoBehaviour
{
    [SerializeField] float selectedHeight = 130f;
    [SerializeField] float defaultHeight = 100f;
    [SerializeField] ARDrawManager drawManager;
    [SerializeField] RectTransform[] buttons;

    void Start()
    {
        // set first tool as default selected
        SetColor(buttons[0]);
    }

    public void SetColor(RectTransform ctx)
    {
        // ensure draw manager is referenced and context is provided
        if (drawManager == null || ctx == null) return;

        // get reference to image to get color
        Image image = ctx.GetChild(0).GetComponent<Image>();
        // check if an image is found
        if (image == null) return;
        // set color
        drawManager.SetColor(image.color);
        
        // reset all positions
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].sizeDelta = new Vector2(buttons[i].rect.width, defaultHeight);
        }
        // set selected offset
        ctx.sizeDelta = new Vector2(ctx.rect.width, selectedHeight);
    }
}
