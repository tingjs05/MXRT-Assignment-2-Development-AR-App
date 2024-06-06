using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARSession))]
public class ARPreviewManager : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] float interactionDistance = 1f;
    [SerializeField] float interactionRange = 0.5f;
    [SerializeField] LayerMask interactionLayer;

    [Header("References")]
    [SerializeField] ARSession session;

    // Start is called before the first frame update
    void Start()
    {
        if (session == null) 
            session = GetComponent<ARSession>();

        ResetARSession();
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

        // erase lines hit
        foreach (Collider hit in hits)
        {
            
        }
    }
    
    // button methods
    public void ResetARSession()
    {
        session?.Reset();
    }
}
