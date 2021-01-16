using System.Collections;
using Mirror;
using Team_Capture.Core;
using Team_Capture.SceneManagement;
using UnityEngine;
using Logger = Team_Capture.Logging.Logger;

namespace Team_Capture.UI.LoadingScreen
{
    internal class LoadingScreenManager : SingletonMonoBehaviour<LoadingScreenManager>
    {
	    [SerializeField] private GameObject loadingScenePrefab;

	    private static bool isLoading;
	    private static bool isSetup;

	    protected override void SingletonAwakened()
	    {
	    }

	    protected override void SingletonStarted()
	    {
			//Make sure loadingScenePrefab isn't null
			if (loadingScenePrefab == null)
			{
				Logger.Error("LoadingScenePrefab is null!");
				Destroy(gameObject);
				return;
			}

			SceneManager.OnBeginSceneLoading += OnBeginSceneLoading;
			isSetup = true;
	    }

	    protected override void SingletonDestroyed()
	    {
		    if (isSetup)
			    SceneManager.OnBeginSceneLoading -= OnBeginSceneLoading;
	    }

	    private void OnBeginSceneLoading(AsyncOperation operation, string sceneName)
	    {
		    StartCoroutine(OnStartSceneLoadAsync(operation, TCScenesManager.FindSceneInfo(sceneName)));
	    }

	    private IEnumerator OnStartSceneLoadAsync(AsyncOperation sceneLoadOperation, TCScene scene)
	    {
		    if (sceneLoadOperation == null || isLoading || !isSetup || Game.IsGameQuitting)
			    yield return null;

		    isLoading = true;
		    LoadingScreenUI loadingScreenUI =
			    Instantiate(loadingScenePrefab).GetComponent<LoadingScreenUI>();
			loadingScreenUI.Setup(scene);

		    // ReSharper disable once PossibleNullReferenceException
		    while (!sceneLoadOperation.isDone)
		    {
			    loadingScreenUI.SetLoadingBarAmount(Mathf.Clamp01(sceneLoadOperation.progress / .9f));

			    yield return null;
		    }

		    isLoading = false;
	    }
    }
}