using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameObject GameWorldContainer;

    public Stage Stage;

    void Start()
	{
        initStage();
    }

    void initStage()
    {
        initPaddle();
        initBall();
        initGameField();
    }

    void initPaddle()
    {
        var paddleConfig = GameConfig.Instance.Paddle;

        var paddleResource = Resources.Load("GameObjectPrefabs/Paddle") as GameObject;
        var paddleObj = GameObject.Instantiate(paddleResource) as GameObject;
        paddleObj.transform.SetParent(GameWorldContainer.transform, false);
        paddleObj.transform.localScale = new Vector3(1, 1, 1);
        paddleObj.transform.localPosition = new Vector3(paddleConfig.StartingPosition.X, paddleConfig.StartingPosition.Y, 0);

        var paddle = paddleObj.GetComponent<BasePaddle>();
        paddle.Init(paddleConfig);
    }

    void initBall()
    {
        var ballConfig = GameConfig.Instance.Ball;

        var ballResource = Resources.Load("GameObjectPrefabs/Ball") as GameObject;
        var ballObj = GameObject.Instantiate(ballResource) as GameObject;
        ballObj.transform.SetParent(GameWorldContainer.transform, false);
        ballObj.transform.localScale = new Vector3(1, 1, 1);
        ballObj.transform.localPosition = new Vector3(ballConfig.StartingPosition.X, ballConfig.StartingPosition.Y, 0);

        var ball = ballObj.GetComponent<BaseBall>();
        ball.Init(ballConfig);
    }

    void initGameField()
    {
        var wallConfig = GameConfig.Instance.Wall;
        var brickCount = wallConfig.Size.X * wallConfig.Size.Y;
        var xOffset = 2f;
        var yOffset = 0.5f;
        var startCentered = (wallConfig.Size.X % 2) == 1;
        var xStart = wallConfig.StartingPosition.X + (Mathf.Floor(wallConfig.Size.X / 2) * xOffset);
        if(!startCentered)
        {
            xStart -= xOffset / 2;
        }
        var yStart = wallConfig.StartingPosition.Y;


        var brickResource = Resources.Load("GameObjectPrefabs/Brick") as GameObject;

        for(var i = 0; i < wallConfig.Size.Y; i++)
        {
            for (var j = 0; j < wallConfig.Size.X; j++)
            {
                var brickObj = GameObject.Instantiate(brickResource) as GameObject;
                brickObj.transform.SetParent(GameWorldContainer.transform, false);
                brickObj.transform.localScale = new Vector3(1, 1, 1);
                brickObj.transform.localPosition = new Vector3(xStart - (xOffset * j), yStart - (yOffset * i), 0);

                var brick = brickObj.GetComponent<BaseBrick>();
                brick.Init();
            }
        }
    }

	void Update()
	{

	}

	void FixedUpdate()
	{

	}
}