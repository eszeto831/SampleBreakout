using UnityEngine;

public class BaseBall : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public BoxCollider2D HitBox;

    private float m_currentSpeed;
    private BaseBrick m_currentTriggeredCollider;

    public void Init(DataBall config)
    {
        m_currentSpeed = config.StartingSpeed;
    }

    public void StartBall()
    {

        Vector2 velocity = new Vector2(Random.Range(-.2f,.2f), -1).normalized * m_currentSpeed;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var brick = other.gameObject.GetComponentInParent<BaseBrick>();
        if (brick != null)
        {
            m_currentTriggeredCollider = brick;
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
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var border = other.gameObject.GetComponentInParent<BaseBorder>();
        if (border != null)
        {
            var borderPosition = border.gameObject.transform.localPosition;
            var borderWidth = border.Sprite.bounds.size.x;
            var borderHeight = border.Sprite.bounds.size.y;
            if (gameObject.transform.localPosition.y <= borderPosition.y - (borderHeight / 2))
            {
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
            }
            if (gameObject.transform.localPosition.y >= borderPosition.y + (borderHeight / 2))
            {
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
            }
            if (gameObject.transform.localPosition.x < borderPosition.x - (borderWidth / 2))
            {
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
            }
            if (gameObject.transform.localPosition.x > borderPosition.x + (borderWidth / 2))
            {
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
            }
        }
    }

    public void GameUpdate()
    {
    }

    public void FixedGameUpdate()
    {
        if (m_currentTriggeredCollider != null)
        {
            var brickPosition = m_currentTriggeredCollider.gameObject.transform.localPosition;
            var brickWidth = m_currentTriggeredCollider.Sprite.bounds.size.x;
            var brickHeight = m_currentTriggeredCollider.Sprite.bounds.size.y;
            var hit = false;
            if (gameObject.transform.localPosition.y <= brickPosition.y - (brickHeight / 2))
            {
                //Hit was from below the brick
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
                hit = true;
            }
            if (gameObject.transform.localPosition.y >= brickPosition.y + (brickHeight / 2))
            {
                //Hit was from above the brick
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
                hit = true;
            }
            if (gameObject.transform.localPosition.x < brickPosition.x - (brickWidth / 2))
            {
                //Hit was on left
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
                hit = true;
            }
            if (gameObject.transform.localPosition.x > brickPosition.x + (brickWidth / 2))
            {
                //Hit was on right
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
                hit = true;
            }
            m_currentTriggeredCollider = null;
        }
    }
}