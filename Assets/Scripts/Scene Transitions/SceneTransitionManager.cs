using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] float loadDuration = 2.5f;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("LoadingScreen");
        DontDestroyOnLoad(gameObject);
        StartCoroutine(WaitToLoad(sceneName));
    }

    IEnumerator WaitToLoad(string sceneName)
    {
        yield return new WaitForSeconds(loadDuration);
        SceneManager.LoadScene(sceneName);
        Destroy(gameObject);
    }
}
