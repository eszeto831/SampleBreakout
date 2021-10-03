using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameInstanceManager
{
	public Camera MainCamera;


    private static GameInstanceManager m_Instance;

	public static GameInstanceManager Instance
	{
		get
		{
			if (m_Instance == null) 
			{
				m_Instance = new GameInstanceManager ();
			}
			return m_Instance;
		}
	}

	public GameInstanceManager()
	{

	}

	public void InitCamara(Camera camera)
	{
		MainCamera = camera;
	}
}