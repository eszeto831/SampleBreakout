using UnityEngine;

public class GameConfig
{
	public DataGameConfig Config;

	public string GameVersion;

	public DataWall Wall;
    public DataBall Ball;
    public DataPaddle Paddle;
    public DataBricks Bricks;

    private static GameConfig m_Instance;

	public static GameConfig Instance
	{
		get
		{
			if (m_Instance == null) 
			{
				m_Instance = new GameConfig ();
			}
			return m_Instance;
		}
	}

	public GameConfig()
    {

    }

	public void ParseConfig()
	{
		TextAsset json = Resources.Load("config") as TextAsset;
		string jsonText = json.text;
		Config = JsonUtility.FromJson<DataGameConfig> (jsonText);
		GameVersion = Config.GameVersion;
        Wall = Config.Wall;
        Ball = Config.Ball;
        Paddle = Config.Paddle;
        Bricks = Config.Bricks;
    }
    
}