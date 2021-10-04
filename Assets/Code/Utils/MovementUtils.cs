using UnityEngine;

public class MovementUtils
{
    public static Vector2 ModifyVelocityWithBounds(Vector2 velocity, Vector3 currentPosition, SpriteRenderer sprite)
    {
        var halfWidth = sprite.bounds.size.x / 2;
        var halfHeight = sprite.bounds.size.y / 2;
        var boundary = GameInstanceManager.Instance.CurrentGame.Boundary;
        if (boundary != null)
        {
            if (currentPosition.x + halfWidth >= boundary.xMax)
            {
                velocity.x = Mathf.Min(0, velocity.x);
            }
            if (currentPosition.x - halfWidth <= boundary.xMin)
            {
                velocity.x = Mathf.Max(0, velocity.x);
            }
            if (currentPosition.y >= boundary.yMax)
            {
                velocity.y = Mathf.Min(0, velocity.y);
            }
            if (currentPosition.y <= boundary.yMin)
            {
                velocity.y = Mathf.Max(0, velocity.y);
            }
        }
        return velocity;
    }

    public static Vector3 ModifyPositionWithBounds(Vector3 currentPosition, SpriteRenderer sprite)
    {
        var halfWidth = sprite.bounds.size.x / 2;
        var halfHeight = sprite.bounds.size.y / 2;
        var boundary = GameInstanceManager.Instance.CurrentGame.Boundary;
        if (boundary != null)
        {
            if (currentPosition.x + halfWidth >= boundary.xMax)
            {
                currentPosition.x = boundary.xMax - halfWidth;
            }
            if (currentPosition.x - halfWidth <= boundary.xMin)
            {
                currentPosition.x = boundary.xMin + halfWidth;
            }
            if (currentPosition.y + halfHeight >= boundary.yMax)
            {
                currentPosition.y = boundary.yMax - halfHeight;
            }
            if (currentPosition.y - halfHeight <= boundary.yMin)
            {
                currentPosition.y = boundary.yMin + halfHeight;
            }
        }
        return currentPosition;
    }
}