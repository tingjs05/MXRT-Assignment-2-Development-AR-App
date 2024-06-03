using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Line
{
    public GameObject gameObject { get; private set; }
    public LineRenderer renderer { get; private set; }
    public ARAnchor anchor { get; private set; }
    public MeshCollider collider { get; private set; }

    // constructor to create a new line
    public Line(float width, int cornerVertices, Color color, Material material, Transform parent = null)
    {
        // create new game object
        gameObject = new GameObject { name = "Line" };
        // set parent of game object
        if (parent != null) gameObject.transform.parent = parent;
        // add components
        renderer = gameObject.AddComponent<LineRenderer>();
        anchor = gameObject.AddComponent<ARAnchor>();
        collider = gameObject.AddComponent<MeshCollider>();

        // set line renderer properties
        // do not allow line to loop
        renderer.loop = false;
        // set width
        renderer.startWidth = width;
        renderer.endWidth = width;
        // make corners rounder
        renderer.numCornerVertices = cornerVertices;
        // set color
        renderer.startColor = color;
        renderer.endColor = color;
        // set material
        renderer.material = material;

        // set line game object line layer
        gameObject.layer = LayerMask.NameToLayer("Line");

        // generate mesh collider for line
        Mesh mesh = new Mesh();
        renderer.BakeMesh(mesh);
        collider.sharedMesh = mesh;
    }
}
