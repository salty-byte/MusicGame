using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 追加する得点を表示するテキストコンポーネント。
/// </summary>
[RequireComponent (typeof (Text))]
public class AddedScoreText : MonoBehaviour
{
	[SerializeField]
	float waitTimeSec = 0.01f;

	Text text;
	Coroutine coroutine;

	// Start is called before the first frame update
	void Start ()
	{
		text = GetComponent<Text> ();
		gameObject.SetActive (false);
	}

	/// <summary>
	/// テキストを更新し、<c>waitTimeSec</c>秒間表示する。
	/// </summary>
	/// <param name="str">表示する文字列。</param>
	public void Show (string str)
	{
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		text.text = str;
		gameObject.SetActive (true);
		coroutine = StartCoroutine (ShowCoroutine ());
	}

	/// <summary>
	/// <c>waitTimeSec</c>秒後に非表示にする。
	/// </summary>
	/// <returns>IEnumerator</returns>
	IEnumerator ShowCoroutine ()
	{
		yield return new WaitForSeconds (waitTimeSec);
		gameObject.SetActive (false);
	}
}
