using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationIcon : MonoBehaviour
{
    [SerializeField] Sprite image;
    [SerializeField, Multiline] string description;
    [SerializeField] GameObject popupMenu;

    Transform camTransform;
    Image popupImage;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        // get camera transform
        camTransform = Camera.main.transform;
        // get popup image and text
        if (popupMenu == null) { Debug.LogWarning("Popup Menu is not provided! Operation has been cancelled. "); return; }
        popupImage = popupMenu.GetComponentInChildren<Image>();
        text = popupMenu.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // look at camera
        transform.right = new Vector3(-camTransform.forward.z, camTransform.forward.y, camTransform.forward.x);
    }

    public void ShowPopup()
    {
        if (popupMenu == null) { Debug.LogWarning("Popup Menu is not provided! Operation has been cancelled. "); return; }
        popupImage.sprite = image;
        text.text = description;
        popupMenu.SetActive(true);
    }
}
