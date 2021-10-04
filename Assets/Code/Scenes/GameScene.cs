using UnityEngine;

public class GameScene : MonoBehaviour
{
    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax, yMin, yMax;
    }

    public GameObject GameWorldContainer;
    public BoxCollider2D GameWorldBoundary;

    void Start()
	{
        initStage();
    }

    void initStage()
    {
        var stage = new Stage();
        stage.Init(GameWorldContainer);
        GameInstanceManager.Instance.SetCurrentGame(stage);
        createBoundary();
    }

    void createBoundary()
    {
        var boundaryPosition = new Boundary();
        boundaryPosition.xMin = GameWorldBoundary.bounds.size.x / 2 * -1;
        boundaryPosition.xMax = GameWorldBoundary.bounds.size.x / 2;
        boundaryPosition.yMin = GameWorldBoundary.bounds.size.y / 2 * -1;
        boundaryPosition.yMax = GameWorldBoundary.bounds.size.y / 2;
        GameInstanceManager.Instance.CurrentGame.Boundary = boundaryPosition;
    }

    void Update()
	{
        GameInstanceManager.Instance.GameUpdate();
	}

	void FixedUpdate()
    {
        GameInstanceManager.Instance.FixedGameUpdate();
    }
}