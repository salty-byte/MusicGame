using UnityEngine;

/// <summary>
/// タッチした位置にパーティクル（リング）を表示するための管理クラス。
/// </summary>
public class TouchManager : SingletonMonoBehaviour<TouchManager>
{
	GameObject ringObj;

	// Use this for initialization
	public void Awake ()
	{
		if (this != Instance) {
			Destroy (this);
			return;
		}
		DontDestroyOnLoad (gameObject);
		ringObj = (GameObject)Resources.Load ("Prefabs/Ring");
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Vector3 clickPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			clickPosition.z = 0.9f;
			CreateRing (clickPosition);
		}
	}

	/// <summary>
	/// Prefabからタッチリングを生成する。
	/// </summary>
	/// <param name="position">画面に出力する位置。</param>
	void CreateRing (Vector3 position)
	{
		Instantiate (ringObj, position, Quaternion.identity);
	}
}
