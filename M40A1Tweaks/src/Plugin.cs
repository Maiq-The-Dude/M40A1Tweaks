using Configs;
using Deli.Setup;
using UnityEngine.SceneManagement;

namespace M40A1Tweaks
{
	public class Plugin : DeliBehaviour
	{
		private readonly Config _configs;
		private readonly Hooks _hooks;

		public Plugin()
		{
			_configs = new Config(Config);
			_hooks = new Hooks(Logger, _configs, Config);
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}

		private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Config.Reload();
		}

		private void OnDestroy()
		{
			_hooks?.Dispose();
		}
	}
}