using HarmonyLib;

namespace lvalonmeme
{
	public static class PInfo
	{
		// each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
		public const string GUID = "llbol.meme.meme";
		public const string Name = "Everyone Is Here";
		public const string version = "0.0.32";
		public static readonly Harmony harmony = new(GUID);

	}
}
