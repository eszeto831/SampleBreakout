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
        m_speedBoost = 1;
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
        VFXUtils.SetVFXSortingLayer(explosionVFX, "VFX");

        //sfx
        var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
        var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
        audioObj.Init(DeathSFX);

        StopBall();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //check for brick/wall collision, only allow one collision with brick/wall per fixed update loop to prevent weird bounces
        //wall gets priority
        var collidableObject = other.gameObject.GetComponentInParent<CollidableObject>();
        if (collidableObject != null)
        {
            if (!(m_currentTriggeredCollider != null && m_currentTriggeredCollider is BoundaryWall))
            {
                m_currentTriggeredCollider = collidableObject;
            }
        }

        //check for paddle collision
        var paddle = other.gameObject.GetComponentInParent<BasePaddle>();
        if (paddle != null)
        {
            BoxCollider2D collider = paddle.HitBox;

            float top = collider.offset.y + (collider.size.y / 2f) + paddle.transform.localPosition.y;
            float btm = collider.offset.y - (collider.size.y / 2f) + paddle.transform.localPosition.y;
            float left = collider.offset.x - (collider.size.x / 2f) + paddle.transform.localPosition.x;
            float right = collider.offset.x + (collider.size.x / 2f) + paddle.transform.localPosition.x;

            var directionModifier = getDirectionChange(top, btm, left, right);
            directionModifier.y = -1;//paddle should bounce ball back up even if the ball hits the sides
            var velocity = GetComponent<Rigidbody2D>().velocity;
            GetComponent<Rigidbody2D>().velocity = new Vector2((velocity.x) + paddle.GetMomentumModifier(), velocity.y).normalized * directionModifier * m_defaultSpeed * m_speedBoost;

            //sfx
            var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
            var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
            audioObj.Init(BounceSFX);
        }

        //check for bottom out of bounds collision
        var deathBoundary = other.gameObject.GetComponentInParent<BoundaryDeath>();
        if (deathBoundary != null)
        {
            GameInstanceManager.Instance.CurrentGame.ResetBallAndPaddle();
        }
    }

    Vector2 getDirectionChange(float collidedObjTop, float collidedObjBot, float collidedObjLeft, float collidedObjRight)
    {
        var directionChange = Vector2.one;
        if (gameObject.transform.localPosition.y <= collidedObjBot)
        {
            //Hit was from below
            directionChange.x = 1;
            directionChange.y = -1;
        }
        if (gameObject.transform.localPosition.y >= collidedObjTop)
        {
            //Hit was from above
            directionChange.x = 1;
            directionChange.y = -1;
        }
        if (gameObject.transform.localPosition.x <= collidedObjLeft)
        {
            //Hit was on left
            directionChange.x = -1;
            directionChange.y = 1;
        }
        if (gameObject.transform.localPosition.x >= collidedObjRight)
        {
            //Hit was on right
            directionChange.x = -1;
            directionChange.y = 1;
        }
        return directionChange;
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }

    public void GameUpdate()
    {
    }

    public void FixedGameUpdate()
    {
        //brick and wall collision logic
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

            var directionModifier = getDirectionChange(top, btm, left, right);

            if (brick != null)
            {
                GameInstanceManager.Instance.CurrentGame.RemoveBrick(brick);
                brick.Kill();
                m_speedBoost = brick.GetSpeedBoost();
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * directionModifier * m_defaultSpeed * m_speedBoost;
            }
            else
            {
                //hit wall
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * directionModifier;

                //sfx
                var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
                var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
                audioObj.Init(BounceSFX);
            }
            m_currentTriggeredCollider = null;
        }
    }
}