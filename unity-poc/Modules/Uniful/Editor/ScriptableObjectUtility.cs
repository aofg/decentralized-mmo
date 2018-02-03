using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace Uniful {
	public static class ScriptableObjectUtility
	{
		/// <summary>
		//	This makes it easy to create, name and place unique new ScriptableObject asset files.
		/// </summary>
		public static T CreateAsset<T>(string defaultPath = "", bool forceDefault = false, bool focusOnAsset = false) where T : ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T> ();

			string path = AssetDatabase.GetAssetPath (Selection.activeObject);
			if (path == "" || forceDefault)
				path = defaultPath;
			else if (Path.GetExtension(path) != "")
			{
				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");

			AssetDatabase.CreateAsset(asset, assetPathAndName);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			if (focusOnAsset)
			{
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = asset;
			}

			return asset;
		}
	}
}