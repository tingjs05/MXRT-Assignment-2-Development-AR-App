using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] GameObject body;
    [SerializeField] GameObject wipMessage;
    [SerializeField] BottomNavigationManager navManager;

    // Start is called before the first frame update
    void Start()
    {
        // error check
        if (body == null || wipMessage == null || navManager == null) return;

        // set default active
        body.SetActive(true);
        wipMessage.SetActive(false);

        // subscribe to event
        navManager.PageChange += ShowPage;
    }

    void ShowPage(BottomNavigationManager.NavPages page)
    {
        if (body == null || wipMessage == null) return;
        body.SetActive(page == BottomNavigationManager.NavPages.HOME);
        wipMessage.SetActive(page != BottomNavigationManager.NavPages.HOME);
    }
}   
