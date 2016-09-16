using UnityEngine;

#if !UNITY_5 && !UNITY_4_5 && !UNITY_4_6 && !UNITY_4_7
public class TooltipAttribute : PropertyAttribute
{
	public readonly string text;
	
	public TooltipAttribute(string text)
	{
		this.text = text;
	}
}
#endif
