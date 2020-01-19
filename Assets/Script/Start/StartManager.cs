using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// スタート画面の管理を行うクラス。
/// </summary>
public class StartManager : MonoBehaviour
{
	[SerializeField]
	float waitTimeSec = 1;

	/// <summary>
	/// 立ち上がりwaitTimeSec（秒）待ってからタップ判定を行う。
	/// </summary>
	/// <returns>IEnumerator</returns>
	IEnumerator Start ()
	{
		enabled = false;
		yield return new WaitForSeconds (waitTimeSec);
		enabled = true;
	}

	// Update is called once per frame
	void Update ()
	{
		// スマホ/PC共に判定できるようにGetMouseButtonDownを使う。
		if (Input.GetMouseButtonDown (0)) {
			GoNextScene ();
		}
	}

	/// <summary>
	/// 次のシーンへ遷移する。
	/// </summary>
	void GoNextScene ()
	{
		AudioManager.Instance.PlaySE (SEName.ClickStart.GetString ());
		NextSceneMem.Instance.NextSceneName = SceneName.Select;
		SceneManager.LoadScene (SceneName.Load.GetString ());
	}
}
