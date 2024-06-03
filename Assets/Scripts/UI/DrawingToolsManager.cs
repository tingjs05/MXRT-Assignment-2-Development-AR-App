using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawingToolsManager : MonoBehaviour
{
    [SerializeField] Vector3 selectedOffset;
    [SerializeField] ARDrawManager drawManager;
    [SerializeField] GameObject[] tools;

    Vector3[] defaultOffset;

    void Start()
    {
        // set default offset
        defaultOffset = tools.Select(x => x.transform.position).ToArray();
        // set first tool as default selected
        Selected(0);
        SetCanDraw(true);
    }

    public void SetCanDraw(bool canDraw)
    {
        if (drawManager == null) return;
        drawManager.CanDraw = canDraw;
    }

    public void Selected(int index)
    {
        if (index >= tools.Length) return;

        // reset all positions
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].transform.position = defaultOffset[i];
        }

        // set selected offset
        tools[index].transform.position = tools[index].transform.position + selectedOffset;
    }
}
