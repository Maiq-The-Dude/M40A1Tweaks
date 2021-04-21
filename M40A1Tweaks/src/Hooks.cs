using BepInEx.Configuration;
using BepInEx.Logging;
using Configs;
using UnityEngine;

namespace M40A1Tweaks
{
	public class Hooks
	{
		private readonly Config _configs;
		private readonly ManualLogSource _loggers;
		private readonly ConfigFile _configFile;

		public Hooks(ManualLogSource logger, Config config, ConfigFile configFile)
		{
			_configs = config;
			_loggers = logger;
			_configFile = configFile;

			Hook();
		}

		public void Dispose()
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
				_configFile.Reload();

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
