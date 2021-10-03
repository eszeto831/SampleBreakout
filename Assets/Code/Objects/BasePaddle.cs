using UnityEngine;

public class BasePaddle : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public BoxCollider2D HitBox;

    public void Init(DataPaddle config)
    {
        Sprite.size = new Vector2(config.Size.X, config.Size.Y);
        HitBox.size = new Vector2(config.Size.X, config.Size.Y);
    }
}