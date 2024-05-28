using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARAnchorManager))]
public class ARDrawManager : MonoBehaviour
{
    // inspector fields
    [Header("Line Properties")]
    [SerializeField] float minLineDistance = 0.1f;
    [SerializeField, Range(0f, 1f)] float width = 0.05f;
    [SerializeField] int cornerVertices = 3;
    [SerializeField] Color color = Color.black;
    [SerializeField] Material material;

    [Header("Floating Line Properties")]
    [SerializeField] float maxPlaneDrawDistance = 5f;
    [SerializeField] float lineCameraOffset = 0.5f;

    [Header("UI")]
    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject crosshairFocused;

    // list to store all generated lines
    List<Line> lines = new List<Line>();

    // current position of camera based on the center of the screen
    Vector3 cameraCenter;
    // cache the previous anchor point of the line
    Vector3 previousAnchorPosition;
    // parent objects to manage generated line objects
    GameObject lineObject;

    // variables for raycasting to detect plane
    ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // boolean to manage if drawing on plane or floating
    bool drawingOnPlane = true;

    // Start is called before the first frame update
    void Start()
    {
        // get components
        raycastManager = GetComponent<ARRaycastManager>();

        // set default previous anchor position
        previousAnchorPosition = Vector3.zero;

        // instantiate parent game objects
        // set line renderer object by creating a new empty game object
        lineObject = new GameObject { name = "Line Renderer Object" };
        lineObject.transform.parent = transform;

        // set default crosshairs to show
        crosshair.SetActive(true);
        crosshairFocused.SetActive(false);

        // test draw line
        // TestLine();
    }

    // Update is called once per frame
    void Update()
    {
        // set camera position
        cameraCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        // use raycast to detect plane
        raycastManager.Raycast(cameraCenter, hits, TrackableType.Planes);

        // check if a plane is available and can be drawn on
        if (PlaneIsAvailable())
        {
            // update drawing on plane boolean
            drawingOnPlane = true;
            // set crosshairs depending on if a plane is detected, and plane is near enough to be drawn on
            crosshair.SetActive(false);
            crosshairFocused.SetActive(true);
            // update focused crosshair position and rotation if plane is detected
            crosshairFocused.transform.SetPositionAndRotation(hits[0].pose.position, hits[0].pose.rotation);
        }
        else
        {
            // update drawing on plane boolean
            drawingOnPlane = false;
            // set crosshairs depending on if a plane is detected, and plane is near enough to be drawn on
            crosshair.SetActive(true);
            crosshairFocused.SetActive(false);
        }

        // check if need to draw a line
        CheckDrawLine();
    }

    // method to draw line for testing purposes
    void TestLine()
    {
        StartDrawLine();
        ContinueDrawLine(lines[0].renderer, new Vector3(0, 0, 1));
        ContinueDrawLine(lines[0].renderer, new Vector3(0, 0, 2));
        ContinueDrawLine(lines[0].renderer, new Vector3(1, 0, 2));
        StopDrawLine();
        StartDrawLine();
        ContinueDrawLine(lines[0].renderer, new Vector3(0, 0, 1));
        ContinueDrawLine(lines[0].renderer, new Vector3(0, 0, -2));
        ContinueDrawLine(lines[0].renderer, new Vector3(-1, 0, -2));
        StopDrawLine();
    }

    // method to check if a drawable plane is available
    bool PlaneIsAvailable()
    {
        return hits.Count > 0 && Vector3.Distance(hits[0].pose.position, transform.position) <= maxPlaneDrawDistance;
    }

    // methods to handle line drawing
    // method to check whether to draw a line
    void CheckDrawLine()
    {
        // check if user is touching the screen, ensure the user is touching the screen
        if (Input.touchCount <= 0) return;

        // get touch input
        Touch touch = Input.GetTouch(0);

        // check touch phase
        // end draw line if touch phase ended
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            StopDrawLine();
            return;
        }

        // start draw line if begin touch
        if (touch.phase == TouchPhase.Began) StartDrawLine();

        // continue drawing if finger is still down
        // use hit position of drawing on plane, otherwise use camera position, and draw the line slightly in front of it
        ContinueDrawLine(lines[0].renderer, drawingOnPlane? hits[0].pose.position : Camera.main.transform.position + (Camera.main.transform.forward * lineCameraOffset));
    }
    
    // method to start drawing a line
    void StartDrawLine()
    {
        // ensure previous line has been completed before starting a new line
        if (previousAnchorPosition != Vector3.zero) return;

        // create a new line
        Line line = new Line(
                width, 
                cornerVertices, 
                color, 
                material, 
                lineObject.transform
            );

        // add line to line renderer list at index 0
        // if line renderer list is empty, just add the item
        if (lines.Count < 1)
            lines.Add(line);
        // otherwise, insert the item to index 0
        else
            lines.Insert(0, line);
    }

    // method to continue drawing line
    void ContinueDrawLine(LineRenderer line, Vector3 currentAnchorPosition)
    {
        // if previous anchor position is still within minimum line distance, do not create a new anchor point for the line
        if (Vector3.Distance(currentAnchorPosition, previousAnchorPosition) < minLineDistance) return;
        
        // elevate the line slightly off the ground by its width to prevent clipping when drawing on a plane
        if (drawingOnPlane) currentAnchorPosition.y += width;

        // draw a new line
        // handle setting first anchor
        if (previousAnchorPosition == Vector3.zero)
        {
            // remove extra default position
            line.positionCount--;
            // create first anchor point
            line.SetPosition(0, currentAnchorPosition);
        }
        // handle continuing line
        else 
        {
            // increment position after drawing line
            line.positionCount++;
            // create a new anchor point
            line.SetPosition(line.positionCount - 1, currentAnchorPosition);
        }

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
