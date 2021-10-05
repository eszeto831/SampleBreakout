using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameInstanceManager
{
    public Camera MainCamera;
    public Player CurrentPlayer;
    public Stage CurrentGame;

    private static GameInstanceManager m_instance;

    public static GameInstanceManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameInstanceManager();
            }
            return m_instance;
        }
    }

    public GameInstanceManager()
    {

    }

    public void InitCamera(Camera camera)
    {
        MainCamera = camera;
    }

    public void InitPlayer()
    {
        CurrentPlayer = new Player();
    }

    public void SetCurrentGame(Stage currentGame)
    {
        CurrentGame = currentGame;
    }

    public void GameUpdate()
    {
        if (CurrentGame != null)
        {
            CurrentGame.GameUpdate();
        }
    }

    public void FixedGameUpdate()
    {
        if (CurrentGame != null)
        {
            CurrentGame.FixedGameUpdate();
        }
    }
}