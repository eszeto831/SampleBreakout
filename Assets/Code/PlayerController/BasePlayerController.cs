using UnityEngine;

public class BasePlayerController
{
	public Player ControlledPlayer;
	public BasePaddle ControlledPaddle;

	virtual public void Init(Player player)
    {
		ControlledPlayer = player;
        ControlledPaddle = player.Paddle;
	}

	virtual public void DestroyController()
	{
	}

    virtual public void GameUpdate()
    {
    }

	virtual public void FixedGameUpdate()
    {
    }
}