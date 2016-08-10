using UnityEngine;
using System.Collections;
using System.Linq;

public static class Assets 
{
	public static Character characterPrefab;
	public static Team[] Teams;
	public static Gametype[] Gametypes;
	public static Item[] Items;
	public static World[] Worlds;
	public static Map[] Maps;
	public static HazardSoundPack HazardSounds;
	public static HelpfulHint[] HelpfulHints;

	private static bool _loaded = false;
	public static bool Loaded 
	{
		get
		{
			return _loaded;
		}
		private set
		{
			_loaded = value;
		}
	}

	public static IEnumerator Load()
	{
		Worlds = Resources.LoadAll<World> ("Worlds").OrderBy(world => world.menuOrder).ToArray();
		Maps = Resources.LoadAll<Map> ("Maps");
		characterPrefab = Resources.Load<Character> ("Character/Character");
		Teams = Resources.LoadAll<Team> ("Teams").OrderBy(team => team.menuOrder).ToArray();
		Gametypes = Resources.LoadAll<Gametype> ("Gametypes").OrderBy(gametype => gametype.menuOrder).ToArray();
		Items = Resources.LoadAll<Item> ("Items");
		HazardSounds = Resources.Load<HazardSoundPack> ("Hazard Sounds");
		HelpfulHints = Resources.LoadAll<HelpfulHint> ("Helpful Hints");

		Loaded = true;

		yield return null;
	}
}
