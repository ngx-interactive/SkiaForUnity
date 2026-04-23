#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using SkiaSharp.Unity.HB;

public static class HBFontDataCreator
{
	private static readonly string[] FontExtensions = { ".ttf", ".otf", ".woff", ".woff2" };

	[MenuItem("Assets/Create/Skia For Unity/HB Font Data", true)]
	static bool ValidateCreate()
	{
		var obj = Selection.activeObject;
		if (obj == null) return false;
		string path = AssetDatabase.GetAssetPath(obj);
		if (string.IsNullOrEmpty(path)) return false;
		string ext = Path.GetExtension(path).ToLowerInvariant();
		return System.Array.IndexOf(FontExtensions, ext) >= 0;
	}

	[MenuItem("Assets/Create/Skia For Unity/HB Font Data")]
	static void Create()
	{
		CreateFromSelection(showErrorDialog: true);
	}

	[MenuItem("Assets/Skia For Unity/Create HB Font Data", true)]
	static bool ValidateCreateFromContextMenu()
	{
		return ValidateCreate();
	}

	[MenuItem("Assets/Skia For Unity/Create HB Font Data", false, 2000)]
	static void CreateFromContextMenu()
	{
		CreateFromSelection(showErrorDialog: false);
	}

	static void CreateFromSelection(bool showErrorDialog)
	{
		var selected = Selection.activeObject;
		if (!IsFontFile(selected))
		{
			if (showErrorDialog)
			{
				EditorUtility.DisplayDialog(
					"HB Font Data",
					"Select a font asset (.ttf, .otf, .woff, .woff2) in the Project window first, then run Create > Skia For Unity > HB Font Data.",
					"OK");
			}
			return;
		}

		string path = AssetDatabase.GetAssetPath(selected);
		var asset = CreateFromPath(path);
		if (asset != null)
		{
			Selection.activeObject = asset;
			EditorGUIUtility.PingObject(asset);
		}
	}

	public static bool IsFontFile(Object obj)
	{
		if (obj == null) return false;
		string path = AssetDatabase.GetAssetPath(obj);
		if (string.IsNullOrEmpty(path)) return false;
		string ext = Path.GetExtension(path).ToLowerInvariant();
		return System.Array.IndexOf(FontExtensions, ext) >= 0;
	}

	public static HBFontData CreateFromPath(string fontPath)
	{
		if (string.IsNullOrEmpty(fontPath) || !File.Exists(fontPath))
		{
			Debug.LogError($"HBFontDataCreator: Invalid font path '{fontPath}'.");
			return null;
		}

		string dir = Path.GetDirectoryName(fontPath);
		string name = Path.GetFileNameWithoutExtension(fontPath);
		string assetPath = Path.Combine(dir, name + " HBFont.asset");

		var existing = AssetDatabase.LoadAssetAtPath<HBFontData>(assetPath);
		if (existing != null) return existing;

		var fontData = ScriptableObject.CreateInstance<HBFontData>();
		fontData.fontBytes = File.ReadAllBytes(fontPath);
		AssetDatabase.CreateAsset(fontData, assetPath);
		AssetDatabase.SaveAssets();
		return fontData;
	}
}
#endif
