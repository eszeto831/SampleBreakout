using UnityEngine;

public class BaseBall : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public BoxCollider2D HitBox;

    private float m_currentSpeed;
    private CollidableObject m_currentTriggeredCollider;

    public void Init(DataBall config)
    {
        m_currentSpeed = config.StartingSpeed;
    }

    public void StartBall()
    {
        Vector2 velocity = new Vector2(Random.Range(-.2f,.2f), -1).normalized * m_currentSpeed;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public void StopBall()
    {
        Vector2 velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var collidableObject = other.gameObject.GetComponentInParent<CollidableObject>();
        if (collidableObject != null)
        {
            m_currentTriggeredCollider = collidableObject;
        }

        var paddle = other.gameObject.GetComponentInParent<BasePaddle>();
        if (paddle != null)
        {
            var paddlePosition = paddle.gameObject.transform.localPosition;
            var paddleWidth = paddle.Sprite.bounds.size.x;
            var paddleHeight = paddle.Sprite.bounds.size.y;
            if (gameObject.transform.localPosition.y <= paddlePosition.y - (paddleHeight / 2))
            {
                //Hit was from below the paddle
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
            }
            if (gameObject.transform.localPosition.y >= paddlePosition.y + (paddleHeight / 2))
            {
                //Hit was from above the paddle
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
            }
            if (gameObject.transform.localPosition.x < paddlePosition.x + (paddleWidth / 2))
            {
                //Hit was on left
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
            }
            if (gameObject.transform.localPosition.x > paddlePosition.x - (paddleWidth / 2))
            {
                //Hit was on right
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
            }
        }

        var deathBoundary = other.gameObject.GetComponentInParent<BoundaryDeath>();
        if (deathBoundary != null)
        {
            GameInstanceManager.Instance.CurrentGame.ResetBall();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }

    public void GameUpdate()
    {
    }

    public void FixedGameUpdate()
    {
        if (m_currentTriggeredCollider != null)
        {
            var triggeredColliderPosition = m_currentTriggeredCollider.gameObject.transform.localPosition;
            var triggeredColliderWidth = m_currentTriggeredCollider.HitBox.bounds.size.x;
            var triggeredColliderHeight = m_currentTriggeredCollider.HitBox.bounds.size.y;
            if (gameObject.transform.localPosition.y <= triggeredColliderPosition.y - (triggeredColliderHeight / 2))
            {
                //Hit was from below the brick
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
            }
            if (gameObject.transform.localPosition.y >= triggeredColliderPosition.y + (triggeredColliderHeight / 2))
            {
                //Hit was from above the brick
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
            }
            if (gameObject.transform.localPosition.x < triggeredColliderPosition.x - (triggeredColliderWidth / 2))
            {
                //Hit was on left
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
            }
            if (gameObject.transform.localPosition.x > triggeredColliderPosition.x + (triggeredColliderWidth / 2))
            {
                //Hit was on right
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
            }

            var brick = m_currentTriggeredCollider.gameObject.GetComponent<BaseBrick>();
            if (brick != null)
            {
                brick.Kill();
            }

            m_currentTriggeredCollider = null;
        }
    }
}