public class Player
{
    public BasePaddle Paddle;
    public BasePlayerController Controller;

    public void SetController(BasePlayerController controller)
    {
        Controller = controller;
    }

    public void SetPaddle(BasePaddle paddle)
    {
        Paddle = paddle;
    }
}