using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void ChangeScene(string s)
    {
        SceneManager.LoadScene(s);
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
}
