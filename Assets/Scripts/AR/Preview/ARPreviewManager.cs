using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARSession))]
public class ARPreviewManager : MonoBehaviour
{
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
        
    }

    public void ResetARSession()
    {
        session?.Reset();
    }
}
