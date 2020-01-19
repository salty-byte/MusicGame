using UnityEngine;

/// <summary>
/// スコアの計算と、スコアの出力アニメーションを管理する。
/// </summary>
public class ScoreManager : MonoBehaviour
{
	[SerializeField]
	ScoreText scoreText = default;

	[SerializeField]
	AddedScoreText addedScoreText = default;

	public ulong Score { get; private set; }

	// Use this for initialization
	void Start ()
	{
		Score = 0;
	}

	/// <summary>
	/// 得点を加算したスコアを計算する。
	/// </summary>
	/// <param name="result">ノーツをタップした結果。</param>
	public void CalcScore (NoteResult result)
	{
		ulong point = CalcAddedPoint (result);
		if (point != 0) {
			AddPoint (point);
		}
	}

	/// <summary>
	/// 加算される得点を計算する。
	/// </summary>
	/// <param name="result">ノーツをタップした結果。</param>
	/// <returns>加算される得点。</returns>
	ulong CalcAddedPoint (NoteResult result)
	{
		ulong basePoint = 100;
		switch (result) {
		case NoteResult.Excellent:
			return basePoint * 3;
		case NoteResult.Good:
			return basePoint * 2;
		case NoteResult.Bad:
			return basePoint;
		default:
			return 0;
		}
	}

	/// <summary>
	/// 得点を加算するアニメーションを行う。
	/// </summary>
	/// <param name="point">加算される得点。</param>
	void AddPoint (ulong point)
	{
		Score += point;
		addedScoreText.Show ("+" + point.ToString ());
		scoreText.AddScore (point);
	}
}
