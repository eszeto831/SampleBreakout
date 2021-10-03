using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentScene : MonoBehaviour
{
	public Camera MainCamera;
	public GameObject ModalBlocker;
	public GameObject DialogLayer;
	public AudioSource MainBGM;


    void Start()
	{
		Application.runInBackground = true;

        GameConfig.Instance.ParseConfig();
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