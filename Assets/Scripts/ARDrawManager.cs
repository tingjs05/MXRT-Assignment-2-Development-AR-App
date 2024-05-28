using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEditor;

[RequireComponent(typeof(ARPlaneManager), typeof(ARRaycastManager))]
public class ARDrawManager : MonoBehaviour
{
    // inspector fields
    [Header("Drawing Line")]
    [SerializeField] float minLineDistance = 0.1f;

    [Header("Line Properties")]
    [Range(0f, 1f)]
    [SerializeField] float width = 0.1f;
    [SerializeField] int cornerVertices = 3;
    [SerializeField] Color color = Color.black;

    [Header("UI")]
    [SerializeField] GameObject crosshair;

    // lists
    // list to store all generated line renderers
    List<LineRenderer> lineRenderers = new List<LineRenderer>();
    // list to store hit planes from raycasts
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // cache the previous anchor point of the line
    Vector3 previousAnchorPosition;
    // object to store all the line renderer components
    GameObject lineRendererObject;

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
        // set line renderer object by creating a new empty game object
        lineRendererObject = new GameObject(name = "Line Renderer Object");
        lineRendererObject.transform.parent = transform;

        // hide crosshair by default
        crosshair.SetActive(false);

        StartDrawLine();
        ContinueDrawLine(lineRenderers[0], new Vector3(0, 0, 1));
        ContinueDrawLine(lineRenderers[0], new Vector3(0, 0, 2));
        ContinueDrawLine(lineRenderers[0], new Vector3(1, 0, 2));
        StopDrawLine();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // use raycast to detect plane
    //     raycastManager.Raycast(
    //             Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)), 
    //             hits, TrackableType.Planes
    //         );
    //     // check if a plane is detected
    //     if (hits.Count > 0)
    //         // show crosshair if can draw line
    //         crosshair.SetActive(true);
    //     else
    //         // hide crosshair if cannot draw
    //         crosshair.SetActive(false);

    //     // check if need to draw a line
    //     CheckDrawLine();
    // }

    // methods to handle line drawing
    // method to check whether to draw a line
    void CheckDrawLine()
    {
        // check if can draw
        if (!CanDraw) return;
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

        // ensure there are surfaces hit by raycast
        if (hits.Count <= 0) return;

        // start draw line if begin touch
        if (touch.phase == TouchPhase.Began) StartDrawLine();

        // continue drawing if finger is still down
        ContinueDrawLine(lineRenderers[0], hits[0].pose.position);
    }
    
    // method to start drawing a line
    void StartDrawLine()
    {
        // do not start drawing a new line if the previous line is not completed
        if (previousAnchorPosition != Vector3.zero) return;

        // create new line renderer
        LineRenderer line = lineRendererObject.AddComponent<LineRenderer>();
        // set line renderer properties
        line.loop = false;
        line.startWidth = width;
        line.endWidth = width;
        line.startColor = color;
        line.endColor = color;
        line.numCornerVertices = cornerVertices;
        line.material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");

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
        if (Vector3.Distance(currentAnchorPosition, previousAnchorPosition) < minLineDistance) return;

        // draw a new line
        // handle setting first anchor
        if (previousAnchorPosition == Vector3.zero)
        {
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
