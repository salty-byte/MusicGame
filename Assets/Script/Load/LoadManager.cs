using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// シーン間の遷移を行うクラス。
/// </summary>
public class LoadManager : MonoBehaviour
{
	[SerializeField]
	Text progressText = default;

	[SerializeField]
	Image progressBar = default;

	// Start is called before the first frame update
	void Start ()
	{
		SceneName nextSceneName = NextSceneMem.Instance.NextSceneName;
		GoNextScene (nextSceneName.GetString ());
	}

	/// <summary>
	/// 指定したシーンへ遷移する。
	/// </summary>
	/// <param name="sceneName">繊維先のシーン名</param>
	public void GoNextScene (string sceneName)
	{
		StartCoroutine (LoadScene (sceneName));
	}

	/// <summary>
	/// 遷移先のシーンを非同期で読み込み、その間にローディングアニメーションを表示する。
	/// </summary>
	/// <param name="sceneName">遷移先のシーン名</param>
	/// <returns>iEnumerator</returns>
	IEnumerator LoadScene (string sceneName)
	{
		var async = SceneManager.LoadSceneAsync (sceneName);

		// 読み込みが完了するまでシーン遷移されないようにする。
		async.allowSceneActivation = false;

		// 次のシーンの読み込み。
		// シーン遷移するまでasync.progressは0.9で止まるようなので、
		// async.progressが0.9以上になったらループを抜ける。
		while (async.progress < 0.9f) {
			string percent = (async.progress * 100).ToString ("F0");
			progressText.text = $"{percent}%";
			progressBar.fillAmount = async.progress;
			yield return null;
		}

		// シーン遷移を許可する。
		async.allowSceneActivation = true;
	}
}
