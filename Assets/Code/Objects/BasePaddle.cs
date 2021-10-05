using UnityEngine;

public class BasePaddle : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public BoxCollider2D HitBox;

    private float m_speed;
    private bool m_frozen;

    public void Init(DataPaddle config)
    {
        Sprite.size = new Vector2(config.Size.X, config.Size.Y);
        HitBox.size = new Vector2(config.Size.X, config.Size.Y);

        m_speed = config.Speed;
        m_frozen = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void UnfreezePaddle()
    {
        m_frozen = false;
    }

    public void Move(float moveHorizontal, float moveVertical)
    {
        if (!m_frozen)
        {
            moveVertical = 0;
            Vector2 velocity = new Vector2(moveHorizontal, moveVertical) * m_speed;
            velocity = MovementUtils.ModifyVelocityWithBounds(velocity, gameObject.transform.position, Sprite);
            GetComponent<Rigidbody2D>().velocity = velocity;

            gameObject.transform.position = MovementUtils.ModifyPositionWithBounds(gameObject.transform.position, Sprite);
        }
    }

    public float GetMomentumModifier()
    {
        return GetComponent<Rigidbody2D>().velocity.x * .25f;
    }
}