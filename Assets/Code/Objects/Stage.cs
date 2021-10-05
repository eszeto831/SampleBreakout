using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Stage
{
    private enum CountdownStates
    {
        THREE,
        TWO,
        ONE,
        START
    }

    public GameObject GameWorldContainer;
    public GameScene.Boundary Boundary;
    public GameObject CountdownContainer;
    public TextMeshProUGUI CountdownText;

    private bool m_pause = false;
    private BaseBall m_ball;
    private List<BaseBrick> m_bricks;

    private bool m_countdownActive;
    private float m_countdownStartTime;
    private CountdownStates m_countdownState;

    public void Init(GameObject gameWorldContainer, GameObject countdownContainer, TextMeshProUGUI countdownText)
    {
        m_bricks = new List<BaseBrick>();

        GameWorldContainer = gameWorldContainer;
        CountdownContainer = countdownContainer;
        CountdownText = countdownText;

        initPaddle();
        initBall();
        initGameField();
    }

    public void StartStage()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
        m_countdownActive = true;
        m_countdownStartTime = Time.time;
        m_countdownState = CountdownStates.THREE;
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

        GameInstanceManager.Instance.CurrentPlayer.SetPaddle(paddle);

        var controller = new GamePadController();
        controller.Init(GameInstanceManager.Instance.CurrentPlayer);
        GameInstanceManager.Instance.CurrentPlayer.SetController(controller);
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

        m_ball = ball;
    }

    void initGameField()
    {
        var wallConfig = GameConfig.Instance.Wall;
        var brickCount = wallConfig.Size.X * wallConfig.Size.Y;
        var xOffset = 2f;
        var yOffset = 0.5f;
        var startCentered = (wallConfig.Size.X % 2) == 1;
        var xStart = wallConfig.StartingPosition.X + (Mathf.Floor(wallConfig.Size.X / 2) * xOffset);
        if (!startCentered)
        {
            xStart -= xOffset / 2;
        }
        var yStart = wallConfig.StartingPosition.Y;


        var brickResource = Resources.Load("GameObjectPrefabs/Brick") as GameObject;

        for (var i = 0; i < wallConfig.Size.Y; i++)
        {
            for (var j = 0; j < wallConfig.Size.X; j++)
            {
                var brickObj = GameObject.Instantiate(brickResource) as GameObject;
                brickObj.transform.SetParent(GameWorldContainer.transform, false);
                brickObj.transform.localScale = new Vector3(1, 1, 1);
                brickObj.transform.localPosition = new Vector3(xStart - (xOffset * j), yStart - (yOffset * i), 0);

                var brick = brickObj.GetComponent<BaseBrick>();
                brick.Init();

                m_bricks.Add(brick);
            }
        }
    }

    public void ResetBall()
    {
        m_ball.StopBall();
        var ballConfig = GameConfig.Instance.Ball;
        m_ball.Init(ballConfig);
        m_ball.transform.localPosition = new Vector3(ballConfig.StartingPosition.X, ballConfig.StartingPosition.Y, 0);
        StartCountdown();
    }

    public void GameUpdate()
    {
        if (!m_pause)
        {
            GameInstanceManager.Instance.CurrentPlayer.Controller.GameUpdate();
            m_ball.GameUpdate();
        }
    }

    public void FixedGameUpdate()
    {
        if (!m_pause)
        {
            GameInstanceManager.Instance.CurrentPlayer.Controller.FixedGameUpdate();
            m_ball.FixedGameUpdate();

            if (m_countdownActive)
            {
                countdown();
            }
        }
    }

    void countdown()
    {
        if(m_countdownState == CountdownStates.THREE)
        {
            Debug.LogError("edmond :: 3");
            CountdownContainer.transform.DOKill();
            CountdownText.DOKill();
            CountdownContainer.transform.DOScaleY(1f, 0.2f);
            CountdownText.text = "3";
            CountdownText.alpha = (150f / 255f);
            CountdownText.transform.localScale = new Vector3(1, 1, 1);
            CountdownText.DOFade(0, 0.7f);
            m_countdownState = CountdownStates.TWO;
        }
        else if (m_countdownState == CountdownStates.TWO && Time.time - m_countdownStartTime > 1)
        {
            Debug.LogError("edmond :: 2");
            CountdownContainer.transform.DOKill();
            CountdownContainer.transform.localScale = new Vector3(1, 1, 1);
            CountdownText.DOKill();
            CountdownText.text = "2";
            CountdownText.alpha = (150f / 255f);
            CountdownText.transform.localScale = new Vector3(1, 1, 1);
            CountdownText.DOFade(0, 0.7f);
            m_countdownState = CountdownStates.ONE;
        }
        else if (m_countdownState == CountdownStates.ONE && Time.time - m_countdownStartTime > 2)
        {
            Debug.LogError("edmond :: 1");
            CountdownContainer.transform.DOKill();
            CountdownContainer.transform.localScale = new Vector3(1, 1, 1);
            CountdownText.DOKill();
            CountdownText.text = "1";
            CountdownText.alpha = (150f / 255f);
            CountdownText.transform.localScale = new Vector3(1, 1, 1);
            CountdownText.DOFade(0, 0.7f);
            m_countdownState = CountdownStates.START;
        }
        else if (m_countdownState == CountdownStates.START && Time.time - m_countdownStartTime > 3)
        {
            Debug.LogError("edmond :: start");
            CountdownContainer.transform.DOKill();
            CountdownContainer.transform.localScale = new Vector3(1, 1, 1);
            CountdownText.DOKill();
            CountdownText.text = "Start";
            CountdownText.alpha = (150f / 255f);
            CountdownText.transform.localScale = new Vector3(1, 1, 1);
            CountdownText.DOFade(0, 0.7f);
            CountdownText.transform.DOScaleX(5f, 0.2f).SetDelay(.2f);
            CountdownText.transform.DOScaleY(0f, 0.2f).SetDelay(.2f);
            CountdownContainer.transform.DOScaleY(0f, 0.2f).SetDelay(.3f);
            m_ball.StartBall();
            m_countdownActive = false;
        }
    }
}