using UnityEngine;

namespace SkiaSharp.Unity.HB
{
	[PreferBinarySerialization]
	public class HBFontData : ScriptableObject
	{
		[HideInInspector]
		public byte[] fontBytes;

		[Header("Optional Style Variants")]
		[Tooltip("Used for <b> and weight >= 700.")]
		public HBFontData boldVariant;

		[Tooltip("Used for <i>.")]
		public HBFontData italicVariant;

		[Tooltip("Used for <b><i>.")]
		public HBFontData boldItalicVariant;
	}
}
