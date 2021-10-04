using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePadController : BasePlayerController
{
	private float m_lastHorizontalMovement = 0f;
	private float m_lastVerticalMovement = 0f;

	override public void GameUpdate()
    {
		//do button press actions
    }

	override public void FixedGameUpdate()
	{
        //do movement actions
        if (ControlledPaddle != null)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            ControlledPaddle.Move(moveHorizontal, moveVertical);
        }
    }
}