using UnityEngine;
using System.Collections;
using System.Linq;

public static class PreloadedAssets
{
	public static World[] Worlds { get; private set; }
	public static Map[] Maps { get; private set; }
	public static Gametype[] Gametypes { get; private set; }
	public static Item[] Items { get; private set; }
	public static Team[] Teams { get; private set; }
	public static Character Character { get; private set; }

	public static IEnumerator Load()
	{
		// TODO: Load all preloaded assets
		Worlds = Resources.LoadAll<World> ("Worlds").OrderBy(world => world.menuOrder).ToArray();
		Maps = Resources.LoadAll<Map> ("Maps");
//		Gametypes = Resources.LoadAll<Gametype> ("Gametypes").OrderBy(gametype => gametype.menuOrder).ToArray();
		Items = Resources.LoadAll<Item> ("Items");
		Teams = Resources.LoadAll<Team> ("Teams").OrderBy(team => team.menuOrder).ToArray();
		Character = Resources.Load<Character> ("Character/Character Prefab");

		// Yield if you need to do something over time, like make a WWW request
		yield return null;
	}
}
