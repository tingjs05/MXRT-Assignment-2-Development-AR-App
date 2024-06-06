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
        transform.right = new Vector3(-camTransform.forward.z, camTransform.forward.y, camTransform.forward.x);
    }

    public void ShowPopup()
    {
        if (popupMenu == null) { Debug.LogWarning("Popup Menu is not provided! Operation has been cancelled. "); return; }
        popupMenu.image.sprite = image;
        popupMenu.description.text = description;
        popupMenu.menu.SetActive(true);
    }

    void SetCamera()
    {
        camTransform = Camera.main.transform;
    }
}
