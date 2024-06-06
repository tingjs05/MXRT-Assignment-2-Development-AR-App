using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPreviewManager : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] float interactionDistance = 1f;
    [SerializeField] float interactionRange = 0.5f;
    [SerializeField] LayerMask interactionLayer;

    [Header("References")]
    [SerializeField] ARSession session;
    
    public static event System.Action resetSession;

    // Start is called before the first frame update
    void Start()
    {
        if (session == null) 
            session = GetComponent<ARSession>();
    }

    // Update is called once per frame
    void Update()
    {
        // handle inputs by users
        HandleInputs();
    }

    void HandleInputs()
    {
        // check if user is touching the screen, ensure the user is touching the screen
        if (Input.touchCount <= 0) return;
        // get touch input
        Touch touch = Input.GetTouch(0);
        // try to interact with objects
        Interact(touch);
    }

    // methods to handle interactions
    void Interact(Touch touch)
    {
        // only interact when touch phase just begins
        if (touch.phase != TouchPhase.Began) return;

        // erase lines within range through colliders
        Collider[] hits = Physics.OverlapSphere(Camera.main.transform.position + (Camera.main.transform.forward * interactionDistance), interactionRange, interactionLayer);
        // check if there are any collisions
        if (hits.Length <= 0) return;

        // interact with information icon
        foreach (Collider hit in hits)
        {
            // get information icon component
            InformationIcon icon = hit.GetComponent<InformationIcon>();
            // ensure icon is not null
            if (icon == null) continue;
            // if gotten icon, show popup
            icon.ShowPopup();
            // only show one icon
            return;
        }
    }
    
    // button methods
    public void ResetARSession()
    {
        session?.Reset();
        resetSession?.Invoke();
    }
}
