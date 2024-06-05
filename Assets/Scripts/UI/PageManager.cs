using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] GameObject body;
    [SerializeField] GameObject wipMessage;
    [SerializeField] GameObject wipPopup;
    [SerializeField] float wipPopupDuration = 2f;
    [SerializeField] BottomNavigationManager navManager;

    Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        // error check
        if (body == null || wipMessage == null || wipPopup == null || navManager == null) return;

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

    public void ShowPopup()
    {
        if (wipPopup == null) return;
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(ShowPopup(wipPopupDuration));
    }

    IEnumerator ShowPopup(float duration)
    {
        wipPopup.SetActive(true);
        yield return new WaitForSeconds(duration);
        coroutine = null;
        wipPopup.SetActive(false);
    }
}   
