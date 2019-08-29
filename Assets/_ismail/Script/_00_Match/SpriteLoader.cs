using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class SpriteLoader
{
    public static IEnumerator LoadSpriteIE(Image img, string _path)
    {
        WWW localFile;
        Texture texture;
        Sprite sprite;

        localFile = new WWW(_path);
        yield return localFile;

        texture = localFile.texture;
        sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        //img.sprite = sprite;
        img.GetComponent<MatchPart>().SetFrontSprite(sprite);
    }
}
