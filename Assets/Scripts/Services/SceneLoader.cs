using System;
using UnityEngine.SceneManagement;
namespace Services
{
    public class SceneLoader : IService
    {

        public void LoadScene(string name, Action onLoad = null)
        {
            var operation = SceneManager.LoadSceneAsync(name);
            if (operation.isDone)
                onLoad?.Invoke();
        }

    }
}
