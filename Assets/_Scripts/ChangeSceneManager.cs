using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ChangeSceneManager 
{
    public void ChangeScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }
}
