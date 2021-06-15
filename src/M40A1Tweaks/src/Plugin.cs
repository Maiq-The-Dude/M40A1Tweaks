using BepInEx;
using Configs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace M40A1Tweaks
{
	[BepInPlugin("maiq.M40A1Tweaks", "M40A1Tweaks", "1.1.0")]
	[BepInProcess("h3vr.exe")]
	public class Plugin : BaseUnityPlugin
	{
		private readonly Config _configs;

		public Plugin()
		{
			_configs = new Config(Config);

			Hook();

			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}

		private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Config.Reload();
		}

		private void OnDestroy()
		{
			Unhook();
		}

		private void Hook()
		{
			On.FistVR.BoltActionRifle.Awake += BoltActionRifle_Awake;
		}

		private void Unhook()
		{
			On.FistVR.BoltActionRifle.Awake -= BoltActionRifle_Awake;
		}

		private void BoltActionRifle_Awake(On.FistVR.BoltActionRifle.orig_Awake orig, FistVR.BoltActionRifle self)
		{
			orig(self);

			if (self.ObjectWrapper.ItemID == "M40A1")
			{
				Config.Reload();

				if (_configs.Enable.Value)
				{
					// Delete amplifier/scope/phys
					GameObject.Destroy(self.transform.Find("3xSight").gameObject);
					GameObject.Destroy(self.transform.Find("Scope").gameObject);
					GameObject.Destroy(self.transform.Find("Phys/Phys_Capsule (1)").gameObject);

					// Add rail to where scope was
					if (_configs.AddTopRail.Value)
					{
						var bottomMount = self.AttachmentMounts[1];
						if (bottomMount != null)
						{
							var topMount = GameObject.Instantiate(bottomMount, self.transform);
							topMount.name = "_RailMount_Top";

							topMount.transform.localPosition = new Vector3(0, 0.058f, 0.1343f);
							topMount.transform.rotation = Quaternion.identity;

							self.AttachmentMounts.Add(topMount);
						}
					}
				}
			}
		}
	}
}