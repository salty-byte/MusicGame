using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 追加先のTextコンポーネントを点滅させる。
/// 点滅サイクルは2秒ごと。
/// </summary>
public class Flash : MonoBehaviour
{
	/// <summary>
	/// 点滅対象のTextコンポーネントを格納する。
	/// </summary>
	Text text;

	/// <summary>
	/// 追加先のTextコンポーネントを取得し、点滅アニメーションを実行する。
	/// 点滅アニメーションは、対象のオブジェクトが削除されるまで動作し続ける。
	/// </summary>
	void Start ()
	{
		text = GetComponent<Text> ();
	}

	/// <summary>
	/// 点滅アニメーションを実行する。
	/// 点滅サイクルは2秒ごと。
	/// </summary>
	void Update ()
	{
		Color c = text.color;
		c.a = Mathf.PingPong (Time.time, 1f);
		text.color = c;
	}
}
