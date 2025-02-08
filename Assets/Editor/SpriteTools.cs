using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  public class SpriteTools
  {
    [MenuItem("Sprites/Set pivots")]
    private static void SetPivots()
    {
      Texture2D[] textures = GetSelectedTextures();

      Selection.objects = new Object[0];

      foreach (Texture2D texture in textures)
      {
        string path = AssetDatabase.GetAssetPath(texture);
        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        SetPivotAsInFirstSprite(textureImporter);
      }
    }

    private static void SetPivotAsInFirstSprite(TextureImporter textureImporter) =>
      SetPivot(textureImporter, textureImporter.spritesheet[0].pivot);

    public static void SetPivot(TextureImporter textureImporter, Vector2 pivot)
    {
      textureImporter.isReadable = true;
      var newData = new List<SpriteMetaData>();
      foreach (SpriteMetaData spriteMetaData in textureImporter.spritesheet)
      {
        SpriteMetaData metaData = spriteMetaData;
        metaData.alignment = 9;
        metaData.pivot = pivot;
        newData.Add(metaData);
      }

      textureImporter.spritesheet = newData.ToArray();

      textureImporter.isReadable = false;
      textureImporter.Save();
    }
    
    public static Texture2D[] GetSelectedTextures()
    {
      return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets)
        .Cast<Texture2D>()
        .ToArray();
    }
  }

  public static class Extensions
  {
    public static TextureImporter Save(this TextureImporter importer)
    {
      EditorUtility.SetDirty(importer);
      importer.SaveAndReimport();
      return importer;
    }
  }
}