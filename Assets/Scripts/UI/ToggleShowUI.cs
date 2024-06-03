using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ToggleShowUI : MonoBehaviour
{
    [SerializeField] float fadeDelay = 2f;
    [SerializeField, Range(0f, 1f)] float fadeSpeed = 0.5f;
    [SerializeField] Sprite showIcon;
    [SerializeField] Sprite hideIcon;
    [SerializeField] GameObject[] objectsToHide;
    [SerializeField] Image[] objectsToFade;

    // public property to show if icons are currently hidden
    public bool IsHidden { get; private set; } = false;
    // image component
    Image image;

    // private variables for calculations
    Color color;
    float currentAlpha;
    float timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        // get image component
        image = GetComponent<Image>();
        // set default sprite
        image.sprite = hideIcon;
        // set time elasped
        timeElapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // do not run if IU is not hidden
        if (!IsHidden) return;

        // increment time elapsed
        timeElapsed += Time.deltaTime;
        // check if user touches the screen, if so, reset values
        if (Input.touchCount > 0)
        {
            currentAlpha = 1f;
            timeElapsed = 0f;
        }
        
        // do not update alpha if already at 0, or already at 1
        if (currentAlpha < 0f || currentAlpha > 1f) return;
        
        // set the alpha of all objects to fade
        foreach (Image objImage in objectsToFade)
        {
            SetAlpha(objImage, currentAlpha);
        }
        // set new image color
        SetAlpha(image, currentAlpha);

        // check time elapsed
        if (timeElapsed < fadeDelay) return;
        // gradually lower the alpha to fade icons after fade delay
        currentAlpha -= fadeSpeed * Time.deltaTime;
    }

    void SetAlpha(Image img, float a)
    {
        color = img.color;
        color.a = a;
        img.color = color;
    }

    public void ToggleUI()
    {
        // toggle is hidden
        IsHidden = !IsHidden;
        // set icon
        image.sprite = IsHidden ? showIcon : hideIcon;
        // set active of UI objects
        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(!IsHidden);
        }
        // reset alpha value of icon
        currentAlpha = 1f;
        // reset the alpha of all objects to fade
        foreach (Image objImage in objectsToFade)
        {
            SetAlpha(objImage, currentAlpha);
        }
        // reset new image color
        SetAlpha(image, currentAlpha);
        // reset time elasped
        timeElapsed = 0f;
    }
}
