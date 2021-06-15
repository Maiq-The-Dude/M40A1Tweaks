using BepInEx.Configuration;

namespace Configs
{
	public class Config
	{
		public ConfigEntry<bool> Enable { get; }
		public ConfigEntry<bool> AddTopRail { get; }

		public Config(ConfigFile config)
		{
			Enable = config.Bind(".Enable", nameof(Enable), true, "Enable/disable the mod");
			AddTopRail = config.Bind("Rail", nameof(AddTopRail), true, "Adds a rail mount where the scope used to be");
		}
	}
}
