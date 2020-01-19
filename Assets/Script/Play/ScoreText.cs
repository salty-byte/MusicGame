using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 現在のスコアを表示するためのスクリプト。
/// </summary>
[RequireComponent (typeof (Text))]
public class ScoreText : MonoBehaviour
{
	[SerializeField]
	float animationTimeSec = 1;

	float currentAnimationTimeSec;
	ulong oldScore;
	ulong newScore;
	bool isAction;
	Text scoreText;

	// Use this for initialization
	void Start ()
	{
		oldScore = 0;
		newScore = 0;
		isAction = false;
		scoreText = GetComponent<Text> ();
		scoreText.text = ToScoreText (0);
	}

	/// <summary>
	/// 得点をスコアに追加し、アニメーションを実施する。
	/// </summary>
	/// <param name="point">追加される得点。</param>
	public void AddScore (ulong point)
	{
		if (!isAction) {
			oldScore = newScore;
			newScore += point;
			currentAnimationTimeSec = animationTimeSec;
			StartCoroutine (AddScoreAnimation ());
		} else {
			newScore += point;
			currentAnimationTimeSec += animationTimeSec;
		}
	}

	/// <summary>
	/// 得点が追加された時のアニメーションを実施する。
	/// </summary>
	/// <returns>IEnumerator</returns>
	IEnumerator AddScoreAnimation ()
	{
		for (var diff = 0.0f; diff <= currentAnimationTimeSec; diff += Time.deltaTime) {
			float rate = diff / currentAnimationTimeSec;
			UpdateText (rate);
			yield return new WaitForEndOfFrame ();
		}
		UpdateText (1);
		isAction = false;
	}

	/// <summary>
	/// スコアテキストを更新する。
	/// </summary>
	/// <param name="rate">得点が加算される前の旧スコアから新スコアまでの割合。0.0 to 1.0.</param>
	void UpdateText (float rate)
	{
		var score = (ulong)((newScore - oldScore) * rate + oldScore);
		scoreText.text = ToScoreText (score);
	}

	/// <summary>
	/// スコアを文字列に変換する。
	///    0:0000000000
	/// 1020:0000001020
	/// </summary>
	/// <param name="score">変換したいスコア。</param>
	/// <returns>スコアテキストように変換された文字列。</returns>
	string ToScoreText (ulong score)
	{
		return score.ToString ("D10");
	}
}
