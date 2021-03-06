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
    public GameObject SFXContainer;

    private bool m_pause = false;
    private BaseBall m_ball;
    private List<BaseBrick> m_bricks;

    private bool m_countdownActive;
    private float m_countdownStartTime;
    private CountdownStates m_countdownState;

    private bool m_restartActive;
    private float m_restartStartTime;

    private GameObject WinVFX;
    private AudioClip WinSFX;
    private AudioClip CountdownSFX;
    private AudioClip CountdownSFX2;

    public void Init(GameObject gameWorldContainer, GameObject countdownContainer, TextMeshProUGUI countdownText, GameObject sfxContainer)
    {
        WinVFX = Resources.Load("VFX/ExplosionWhiteout") as GameObject;
        WinSFX = Resources.Load("Audio/Sound/39068__alienbomb__explosion-1") as AudioClip;
        CountdownSFX = Resources.Load("Audio/Sound/554056__gronkjaer__clockbeep") as AudioClip;
        CountdownSFX2 = Resources.Load("Audio/Sound/369867__samsterbirdies__radio-beep") as AudioClip;

        m_bricks = new List<BaseBrick>();

        GameWorldContainer = gameWorldContainer;
        CountdownContainer = countdownContainer;
        CountdownText = countdownText;
        SFXContainer = sfxContainer;

        initPaddle();
        initBall();
        initGameField();
    }

    public void StartStage()
    {
        StartCountdown();
    }

    public void RestartGame()
    {
        m_bricks = new List<BaseBrick>();

        var ballConfig = GameConfig.Instance.Ball;
        m_ball.Init(ballConfig);
        m_ball.transform.localPosition = new Vector3(ballConfig.StartingPosition.X, ballConfig.StartingPosition.Y, 0);

        var paddleConfig = GameConfig.Instance.Paddle;
        GameInstanceManager.Instance.CurrentPlayer.Paddle.Init(paddleConfig);
        GameInstanceManager.Instance.CurrentPlayer.Paddle.transform.localPosition = new Vector3(paddleConfig.StartingPosition.X, paddleConfig.StartingPosition.Y, 0);

        initGameField();
        StartStage();
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
            var colorIndex = Math.Min(i, GameConfig.Instance.Bricks.Color.Count - 1); //Math.Max(GameConfig.Instance.Bricks.Color.Count - 1 - i, 0);
            var speedBoostIndex = Math.Min(i, GameConfig.Instance.Bricks.SpeedBoost.Count - 1); //Math.Max(GameConfig.Instance.Bricks.SpeedBoost.Count - 1 - i, 0);
            for (var j = 0; j < wallConfig.Size.X; j++)
            {
                var brickObj = GameObject.Instantiate(brickResource) as GameObject;
                brickObj.transform.SetParent(GameWorldContainer.transform, false);
                brickObj.transform.localScale = new Vector3(1, 1, 1);
                brickObj.transform.localPosition = new Vector3(xStart - (xOffset * j), yStart - (yOffset * i), 0);

                var brick = brickObj.GetComponent<BaseBrick>();
                brick.Init();
                brick.SetProperties(GameConfig.Instance.Bricks.Color[colorIndex], GameConfig.Instance.Bricks.SpeedBoost[colorIndex]);

                m_bricks.Add(brick);
            }
        }
    }

    public void ResetBallAndPaddle()
    {
        m_ball.KillBall();
        var ballConfig = GameConfig.Instance.Ball;
        m_ball.Init(ballConfig);
        m_ball.transform.localPosition = new Vector3(ballConfig.StartingPosition.X, ballConfig.StartingPosition.Y, 0);

        var paddleConfig = GameConfig.Instance.Paddle;
        GameInstanceManager.Instance.CurrentPlayer.Paddle.Init(paddleConfig);
        GameInstanceManager.Instance.CurrentPlayer.Paddle.transform.localPosition = new Vector3(paddleConfig.StartingPosition.X, paddleConfig.StartingPosition.Y, 0);

        StartCountdown();
    }

    public void RemoveBrick(BaseBrick brick)
    {
        m_bricks.Remove(brick);
        if(m_bricks.Count == 0)
        {
            m_ball.StopBall();
            var explosionPos = m_ball.gameObject.transform.localPosition;

            //vfx
            var explosionVFX = GameObject.Instantiate(WinVFX) as GameObject;
            explosionVFX.transform.localPosition = explosionPos;
            VFXUtils.SetVFXSortingLayer(explosionVFX, "VFX");

            //sfx
            var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
            var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
            audioObj.Init(WinSFX);

            m_restartActive = true;
            m_restartStartTime = Time.time;
        }
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

            if (m_restartActive)
            {
                restart();
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

            //sfx
            var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
            var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
            audioObj.Init(CountdownSFX);
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

            //sfx
            var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
            var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
            audioObj.Init(CountdownSFX);
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

            //sfx
            var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
            var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
            audioObj.Init(CountdownSFX);

            GameInstanceManager.Instance.CurrentPlayer.Paddle.UnfreezePaddle();
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

            //sfx
            var explosionSFX = GameObject.Instantiate(SFXContainer) as GameObject;
            var audioObj = explosionSFX.GetComponent<SelfDestroyingAudio>();
            audioObj.Init(CountdownSFX2);
        }
    }

    void restart()
    {
        if (Time.time - m_restartStartTime > 2)
        {
            m_restartActive = false;
            RestartGame();
        }
    }
}