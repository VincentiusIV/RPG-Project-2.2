using UnityEngine;
using System.Collections;
using UnityEditor;

public class SpriteProcessor : AssetPostprocessor
{
    // Script that automatically changes the texture type of an image to Sprite in the sprites folder

    void OnPostprocessTexture(Texture2D texture)
    {
        string lowerCaseAssetPath = assetPath.ToLower();
        bool isInSpritesDirectory = lowerCaseAssetPath.IndexOf("/sprites/") != -1;

        if(isInSpritesDirectory)
        {
            TextureImporter textureImporter = (TextureImporter) assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
        }
    }
	
}
