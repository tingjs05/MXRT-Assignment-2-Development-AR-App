using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARPlaneManager), typeof(ARRaycastManager))]
public class ARDrawManager : MonoBehaviour
{
    // inspector fields
    [SerializeField] float minLineDistance = 0.1f;

    // lists
    // list to store all generated line renderers
    List<LineRenderer> lineRenderers = new List<LineRenderer>();

    // position of the center of the screen
    Vector3 center;
    // cache the previous anchor point of the line
    Vector3 previousAnchorPosition;

    // boolean to control whether this script can draw
    [HideInInspector] public bool CanDraw = true;

    // componenets
    ARPlaneManager planeManager;
    ARRaycastManager raycastManager;

    // Start is called before the first frame update
    void Start()
    {
        // get componenets
        planeManager = GetComponent<ARPlaneManager>();
        raycastManager = GetComponent<ARRaycastManager>();

        // set default previous anchor position
        previousAnchorPosition = Vector3.zero;
        // set center of screen in world space
        center = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // methods to handle touch detection
    void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        // method will be called when touch is detected
        EnhancedTouch.Touch.onFingerDown += CheckDrawLine;
    }

    void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        // method will not no longer be called
        EnhancedTouch.Touch.onFingerDown -= CheckDrawLine;
    }

    // methods to handle line drawing
    // method to check whether to draw a line
    void CheckDrawLine(EnhancedTouch.Finger finger)
    {
        // check if can draw
        if (!CanDraw) return;
        // only detect one touch input at one time
        if (finger.index != 0) return;

        // check touch phase
        // end draw line if touch phase ended
        if (!finger.isActive)
        {
            StopDrawLine();
            return;
        }
        // start draw line if begin touch
        if (finger.currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Began) StartDrawLine();

        // handle continued touch
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(center, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            ContinueDrawLine(lineRenderers[0], hits[0].pose.position);
        }
    }
    
    // method to start drawing a line
    void StartDrawLine()
    {
        // do not start drawing a new line if the previous line is not completed
        if (previousAnchorPosition != Vector3.zero) return;

        // create new line renderer
        LineRenderer line = new LineRenderer();

        // add line to line renderer list at index 0
        // if line renderer list is empty, just add the item
        if (lineRenderers.Count < 1)
            lineRenderers.Add(line);
        // otherwise, insert the item to index 0
        else
            lineRenderers.Insert(0, line);
    }

    // method to continue drawing line
    void ContinueDrawLine(LineRenderer line, Vector3 currentAnchorPosition)
    {
        // if previous anchor position is still within minimum line distance, do not run the code
        if (Vector3.Distance(currentAnchorPosition, previousAnchorPosition) >= minLineDistance) return;

        // draw a new line
        // create a new anchor point
        line.SetPosition(line.positionCount, currentAnchorPosition);
        // increment position after drawing line
        line.positionCount++;

        // cache current anchor position
        previousAnchorPosition = currentAnchorPosition;
    }

    // method to stop drawing line
    void StopDrawLine()
    {
        // reset default previous anchor position
        previousAnchorPosition = Vector3.zero;
    }
}
