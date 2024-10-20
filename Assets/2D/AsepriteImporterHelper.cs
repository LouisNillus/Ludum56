using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class AsepriteImporterHelper : AssetPostprocessor
{
    void OnPreprocessAsset()
    {
        if (assetImporter.importSettingsMissing)
        {
            AsepriteImporter aseprite_importer = assetImporter as AsepriteImporter;
            if (aseprite_importer != null)
            {
                aseprite_importer.pivotAlignment = SpriteAlignment.Center;
                aseprite_importer.spritePixelsPerUnit = 32;

                TextureImporterPlatformSettings webgl_settings = aseprite_importer.GetImporterPlatformSettings(BuildTarget.WebGL);
                webgl_settings.overridden = true;
                webgl_settings.format = TextureImporterFormat.ASTC_4x4;
                webgl_settings.maxTextureSize = 64;
                webgl_settings.compressionQuality = 100;

                aseprite_importer.SetImporterPlatformSettings(webgl_settings);
            }
        }
    }
}
