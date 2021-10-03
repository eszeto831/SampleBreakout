using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour
{
	public float Lifetime;

	void Start ()
	{
		Init();
	}

	public void Init()
	{
		StartCoroutine(WaitAndDestroy(Lifetime));
	}

	private IEnumerator WaitAndDestroy(float lifetime)
	{
		yield return new WaitForSeconds(lifetime);

        GameObject.Destroy(this.gameObject);
	}
}
