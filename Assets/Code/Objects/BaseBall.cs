using UnityEngine;

public class BaseBall : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public BoxCollider2D HitBox;
    public AudioClip BounceSFX;
    public GameObject DeathVFX;
    public AudioClip DeathSFX;
    public GameObject SFXContainer;

    private float m_defaultSpeed;
    private float m_speedBoost;
    private CollidableObject m_currentTriggeredCollider;

    public void Init(DataBall config)
    {
        m_defaultSpeed = config.StartingSpeed;
        m_speedBoost = 0;
    }

    public void StartBall()
    {
        Vector2 velocity = new Vector2(Random.Range(-.2f,.2f), -1).normalized * m_defaultSpeed;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public void StopBall()
    {
        Vector2 velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public void KillBall()
    {
        //vfx
        var explosionVFX = GameObject.Instantiate(DeathVFX) as GameObject;
        explosionVFX.transform.localPosition = gameObject.transform.localPosition;

        //sfx
        var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
        var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
        audioObj.Init(DeathSFX);

        StopBall();
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
            BoxCollider2D collider = paddle.HitBox;

            float top = collider.offset.y + (collider.size.y / 2f) + paddle.transform.localPosition.y;
            float btm = collider.offset.y - (collider.size.y / 2f) + paddle.transform.localPosition.y;
            float left = collider.offset.x - (collider.size.x / 2f) + paddle.transform.localPosition.x;
            float right = collider.offset.x + (collider.size.x / 2f) + paddle.transform.localPosition.x;
            
            if (gameObject.transform.localPosition.y <= btm)
            {
                //Hit was from below the paddle
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x + paddle.GetMomentumModifier(), velocity.y * -1);
            }
            if (gameObject.transform.localPosition.y >= top)
            {
                //Hit was from above the paddle
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x + paddle.GetMomentumModifier(), velocity.y * -1);
            }
            if (gameObject.transform.localPosition.x <= left)
            {
                //Hit was on left
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2((velocity.x * -1) + paddle.GetMomentumModifier(), velocity.y);
            }
            if (gameObject.transform.localPosition.x >= right)
            {
                //Hit was on right
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2((velocity.x * -1) + paddle.GetMomentumModifier(), velocity.y);
            }

            //sfx
            var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
            var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
            audioObj.Init(BounceSFX);
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
            BoxCollider2D collider = m_currentTriggeredCollider.HitBox;

            float top = collider.offset.y + (collider.size.y / 2f);
            float btm = collider.offset.y - (collider.size.y / 2f);
            float left = collider.offset.x - (collider.size.x / 2f);
            float right = collider.offset.x + (collider.size.x / 2f);

            var brick = m_currentTriggeredCollider.gameObject.GetComponent<BaseBrick>();
            if (brick != null)
            {
                top += brick.transform.localPosition.y;
                btm += brick.transform.localPosition.y;
                left += brick.transform.localPosition.x;
                right += brick.transform.localPosition.x;
            }

            if (gameObject.transform.localPosition.y <= btm)
            {
                //Hit was from below the brick
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
            }
            if (gameObject.transform.localPosition.y >= top)
            {
                //Hit was from above the brick
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y * -1);
            }
            if (gameObject.transform.localPosition.x <= left)
            {
                //Hit was on left
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
            }
            if (gameObject.transform.localPosition.x >= right)
            {
                //Hit was on right
                var velocity = GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * -1, velocity.y);
            }
            
            if (brick != null)
            {
                GameInstanceManager.Instance.CurrentGame.RemoveBrick(brick);
                brick.Kill();
                m_speedBoost = brick.GetSpeedBoost();
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * m_defaultSpeed * m_speedBoost;
            }
            else
            {
                //sfx
                var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
                var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
                audioObj.Init(BounceSFX);
            }

            m_currentTriggeredCollider = null;
        }
    }
}