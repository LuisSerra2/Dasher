using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ChangeSceneManager 
{
    public void ChangeScene(string scene)
    {
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync(scene);
    }
}
