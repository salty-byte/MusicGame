using UnityEngine;

/// <summary>
/// プレイ中のノーツの初期化と移動を行う。
/// </summary>
public class Note : MonoBehaviour
{
	float remainTimeSec;
	float movedTimeSec;
	Vector3 startPos;
	Vector3 endPos;

	public bool IsMoving { get; private set; }
	public NoteData Data { get; set; }

	/// <summary>
	/// ノーツの初期化を行う。
	/// </summary>
	/// <param name="data">ノーツの詳細情報</param>
	/// <param name="timeSec">スタート地点からゴール地点までにかかる時間（秒）</param>
	/// <param name="endPos">ゴール地点の位置</param>
	/// <param name="averageTimeSec">入力受付の有効時間（秒）</param>
	public void SetStart (NoteData data, float timeSec, Vector3 endPos, float averageTimeSec)
	{
		// ノーツの初期データを適用する。
		Data = data;
		startPos = transform.position;
		this.endPos = endPos;
		remainTimeSec = timeSec + averageTimeSec;

		// ノーツのゴール地点を伸ばす。
		// これをしないと、Excellentのタイミングでノーツが止まる。
		Vector3 norm = (endPos - startPos);
		Vector3 absPos = norm * (averageTimeSec / timeSec);
		this.endPos += absPos;

		// 移動を可能にする。
		IsMoving = true;
	}

	/// <summary>
	/// ノーツをdeltaTime移動する。
	/// </summary>
	/// <param name="deltaTime">経過した時間（s）</param>
	public void MoveDeltaTime (float deltaTime)
	{
		movedTimeSec += deltaTime;
		if (movedTimeSec >= remainTimeSec) {
			transform.position = endPos;
			// FIXME Findはあまり使いたくないところ。
			GameObject.Find ("GameManager").GetComponent<GameManager> ().Determine (NoteResult.Miss, this);
			IsMoving = false;
		} else {
			float rate = movedTimeSec / remainTimeSec;
			UpdatePosition (rate);
		}
	}

	/// <summary>
	/// ノーツの位置を更新する。
	/// </summary>
	/// <param name="rate">スタート地点からゴール地点までを1とした時の現在の移動割合</param>
	void UpdatePosition (float rate)
	{
		transform.position = Vector3.Lerp (startPos, endPos, rate);
	}
}
