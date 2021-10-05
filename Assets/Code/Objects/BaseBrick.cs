using UnityEngine;

public class BaseBrick : CollidableObject
{
    public SpriteRenderer Sprite;
    public AudioClip DeathSFX;
    public GameObject DeathVFX;
    public GameObject DeathSFXContainer;

    private float m_speedBoost;

    override public void Init()
    {
        base.Init();
    }

    public void SetProperties(DataColor color, float speedBoost)
    {
        Sprite.color = new Color(color.R, color.G, color.B, color.A);
        m_speedBoost = speedBoost;
    }

    public float GetSpeedBoost()
    {
        return m_speedBoost;
    }

    public void Kill()
    {
        //vfx
        var explosionVFX = GameObject.Instantiate(DeathVFX) as GameObject;
        explosionVFX.transform.localPosition = gameObject.transform.localPosition;

        //sfx
        var explosionSFX = GameObject.Instantiate(DeathSFXContainer) as GameObject;
        var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
        audioObj.Init(DeathSFX);

        Destroy(this.gameObject);
    }
}