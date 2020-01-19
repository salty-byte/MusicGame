using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ノーツをタップした時の評価を表示する。
/// </summary>
[RequireComponent (typeof (Text))]
public class EvalText : MonoBehaviour
{
	[SerializeField]
	float setTimeSec = 1;

	float time;
	Text evalText;

	// Use this for initialization
	void Start ()
	{
		time = setTimeSec;
		evalText = gameObject.GetComponent<Text> ();
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update ()
	{
		time -= Time.deltaTime;
		if (time < 0) {
			gameObject.SetActive (false);
		} else if (time < setTimeSec / 3) {
			float rate = time / setTimeSec;
			UpdateColorAlpha (rate);
		}
	}

	/// <summary>
	/// 透明度を更新する。
	/// <c>rate</c>が0に近ければ近いほど透明、
	/// 1に近ければ近いほど非透明になる。
	/// </summary>
	/// <param name="rate">非透明率。0 to 1.</param>
	void UpdateColorAlpha (float rate)
	{
		Color color = evalText.color;
		color.a = Mathf.Lerp (0, 1, rate);
		evalText.color = color;
	}

	/// <summary>
	/// 表示するテキストを更新し、非透明状態で画面に表示する。
	/// </summary>
	/// <param name="eval">画面に表示する文字列。</param>
	public void SetNewText (string eval)
	{
		evalText.text = eval;
		time = setTimeSec;
		gameObject.SetActive (true);
		UpdateColorAlpha (1);
	}
}
