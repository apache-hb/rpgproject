using System.Collections;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] private SceneReference gameScene;

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadSceneAsync(gameScene.BuildIndex);
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    void OnValidate()
    {
        Assert.IsNotNull(gameScene);
    }
}
