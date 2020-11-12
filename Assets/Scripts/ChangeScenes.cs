using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public GameObject transitionObject;
    public void ChangeScene(string s)
    {
        Instantiate(transitionObject).GetComponent<FadeOut>().scene = s;
        //SceneManager.LoadScene(s);
    }

    // ignore this please, I know this shouldn't be here
    public void ActivateGameObject(GameObject g)
    {
        g.SetActive(true);
    }

    public void DisableGameObject(GameObject g)
    {
        g.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
