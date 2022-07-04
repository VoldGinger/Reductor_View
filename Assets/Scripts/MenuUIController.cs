using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    private SceneLoader _sceneLoader;
    private const string MainSceneName = "Work Scene";

    private void Start()
    {
        _sceneLoader = ServiceLocator.Instance.GetService<SceneLoader>();
        _startButton.onClick.AddListener(LoadMainScene);
        _exitButton.onClick.AddListener(Quit);
    }
    private void Quit()
    {
        Application.Quit();
    }
    private void LoadMainScene()
    {
        _sceneLoader.LoadScene(MainSceneName);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(LoadMainScene);
        _exitButton.onClick.RemoveListener(Quit);
    }





}
