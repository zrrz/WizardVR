using UnityEditor;
using UnityEngine;

#if !UNITY_5 && !UNITY_4_5 && !UNITY_4_6 && !UNITY_4_7
[CustomPropertyDrawer(typeof(TooltipAttribute))]
public class TooltipDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		var atr = (TooltipAttribute) attribute;
		var content = new GUIContent(label.text, atr.text);
		EditorGUI.PropertyField(position, prop, content);
	}
}
#endif
