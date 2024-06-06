using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationIcon : MonoBehaviour
{
    [SerializeField] Sprite image;
    [SerializeField, Multiline] string description;
    [SerializeField] PopupMenu popupMenu;
    Transform camTransform;

    // Start is called before the first frame update
    void Start()
    {
        // get camera transform
        camTransform = Camera.main.transform;
        // subscribe to event
        ARPreviewManager.resetSession += SetCamera;
    }

    // Update is called once per frame
    void Update()
    {
        // look at camera
        transform.up = -camTransform.forward;
        transform.forward = -camTransform.up;
    }

    public void ShowPopup()
    {
        // ensure popup menu is not null
        if (popupMenu == null) { Debug.LogWarning("Popup Menu is not provided! Operation has been cancelled. "); return; }
        // do not run if menu is already open
        if (popupMenu.IsOpen) return;
        // set images and show menu
        popupMenu.image.sprite = image;
        popupMenu.description.text = description;
        popupMenu.SetMenu(true);
    }

    void SetCamera()
    {
        camTransform = Camera.main.transform;
    }
}
