using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static void CreateAsset<T> () where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}

	[UnityEditor.MenuItem("Assets/Create/HazardSoundPack")]
	static void CreateHazardSoundPack()
	{
		ScriptableObjectUtility.CreateAsset<HazardSoundPack> ();
	}
	
	[UnityEditor.MenuItem("Assets/Create/Team")]
	static void CreateTeam()
	{
		ScriptableObjectUtility.CreateAsset<Team> ();
	}
	
	[UnityEditor.MenuItem("Assets/Create/Gametype/Generic Gametype")]
	static void CreateGametype()
	{
		ScriptableObjectUtility.CreateAsset<Gametype> ();
	}
	
	[UnityEditor.MenuItem("Assets/Create/Map")]
	static void CreateMap()
	{
		ScriptableObjectUtility.CreateAsset<Map> ();
	}
	
	[UnityEditor.MenuItem("Assets/Create/World")]
	static void CreateWorld()
	{
		ScriptableObjectUtility.CreateAsset<World> ();
	}

	[UnityEditor.MenuItem("Assets/Create/Helpful Hint")]
	static void CreateHelpfulHint()
	{
		ScriptableObjectUtility.CreateAsset<HelpfulHint> ();
	}
}