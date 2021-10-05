using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentScene : MonoBehaviour
{
	public Camera MainCamera;
	public GameObject ModalBlocker;
	public GameObject DialogLayer;
	public AudioSource MainBGM;
    public TextMeshProUGUI VersionNumber;

    void Start()
	{
		Application.runInBackground = true;

        GameConfig.Instance.ParseConfig();
        VersionNumber.text = "v " + GameConfig.Instance.GameVersion;

        GameInstanceManager.Instance.InitCamera(MainCamera);
        GameInstanceManager.Instance.InitPlayer();

        SceneManager.LoadScene("GameScene");

    }

	void Awake() 
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	void Update()
	{

	}
}