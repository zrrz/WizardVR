using UnityEngine;
using System.Collections;
using UnityEditor;

public class SpellDataEditor {
  [MenuItem("Assets/Create/SpellData")]
  public static SpellData Create()
  {
    SpellData asset = ScriptableObject.CreateInstance<SpellData>();

    AssetDatabase.CreateAsset(asset, "Assets/Spells/SpellData.asset");
    AssetDatabase.SaveAssets();
    return asset;
  }
}
