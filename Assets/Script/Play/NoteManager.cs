using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// ノーツの生成やノーツの移動処理を行うクラス。
/// </summary>
public class NoteManager : MonoBehaviour
{
	[SerializeField]
	float excellentTimeSec = 0.1f;

	[SerializeField]
	float goodTimeSec = 0.15f;

	[SerializeField]
	float badTimeSec = 0.2f;

	[SerializeField]
	float missTimeSec = 0.25f;

	[SerializeField]
	GameObject notes = default;

	[SerializeField]
	TouchPoint [] touchPoints = default;

	[SerializeField]
	GameObject [] onpu = default;

	GameObject noteObj;
	Vector3 [] startPos;
	Vector3 [] endPos;
	int currentIdx;
	int autoIdx;

	List<NoteData> noteDataList;

	void Awake ()
	{
		noteObj = (GameObject)Resources.Load ("Prefabs/Note");

		startPos = new Vector3 [onpu.Length];
		for (int i = 0; i < onpu.Length; i++) {
			startPos [i] = onpu [i].transform.position;
		}

		endPos = new Vector3 [touchPoints.Length];
		for (int i = 0; i < touchPoints.Length; i++) {
			endPos [i] = touchPoints [i].transform.position;
		}
	}

	/// <summary>
	/// ノーツを<c>deltaTime</c>（秒）分移動する。
	/// </summary>
	/// <param name="deltaTime">移動時間（秒）。</param>
	public void MoveDeltaTime (float deltaTime)
	{
		Note [] ns = notes.GetComponentsInChildren<Note> ();
		foreach (var n in ns) {
			if (n.IsMoving) {
				n.MoveDeltaTime (deltaTime);
			}
		}
	}

	/// <summary>
	/// ノーツ生成の元となるリストを設定する。
	/// </summary>
	/// <param name="noteDataList">ノーツ生成の元となるリスト。</param>
	public void Initialize (List<NoteData> noteDataList)
	{
		this.noteDataList = noteDataList;
	}

	/// <summary>
	/// 該当するノーツを準備する。
	/// </summary>
	/// <param name="time">楽曲の経過時間（秒）</param>
	/// <param name="setTimeSec">タッチポイントまでの移動にかかる時間（秒）。</param>
	public void PrepareNote (float time, float setTimeSec)
	{
		NoteData data = GetNextNote (time + setTimeSec);
		if (data != null) {
			CreateNote (data, setTimeSec);
		}
	}

	/// <summary>
	/// <c>time</c>を元に、次に表示するノーツを取得する。
	/// </summary>
	/// <param name="time">楽曲の経過時間（秒）。</param>
	/// <returns>次に表示するノーツ情報。</returns>
	public NoteData GetNextNote (float time)
	{
		if (currentIdx >= noteDataList.Count) {
			return null;
		}

		if (noteDataList [currentIdx].Time <= time) {
			return noteDataList [currentIdx++];
		}
		return null;
	}

	/// <summary>
	/// <c>time</c>を元に、オートプレイ時のタップ位置を返す。
	/// </summary>
	/// <param name="time">楽曲の経過時間（秒）。</param>
	/// <returns>タップ位置。</returns>
	public int GetAuto (float time)
	{
		if (autoIdx >= noteDataList.Count) {
			return -1;
		}

		if (noteDataList [autoIdx].Time <= time) {
			return noteDataList [autoIdx++].EndDir;
		}
		return -1;
	}

	/// <summary>
	/// ノーツを作成する。
	/// TODO 毎回オブジェクトを生成するのはよろしくない気がするので、使い終わったノーツを使い回せるようにする。
	/// </summary>
	/// <param name="data">ノーツ情報。</param>
	/// <param name="setTimeSec">タッチポイントまでの移動にかかる時間（秒）。<</param>
	public void CreateNote (NoteData data, float setTimeSec)
	{
		GameObject obj = Instantiate (noteObj, startPos [data.StartDir], Quaternion.identity, notes.transform);
		obj.name = noteObj.name;

		Note note = obj.GetComponent<Note> ();
		note.SetStart (data, setTimeSec, endPos [data.EndDir], missTimeSec);
	}

	/// <summary>
	/// タップ判定を実施し、判定結果を返す。
	/// </summary>
	/// <param name="dir">タップ位置</param>
	/// <param name="time">現在の楽曲再生時間</param>
	/// <returns>タップ判定結果を返す。</returns>
	public (NoteResult result, Note note) Judge (int dir, float time)
	{
		Note [] allNotes = notes.GetComponentsInChildren<Note> ();
		Note target = allNotes.FirstOrDefault (e => e.Data.EndDir == dir);
		if (target != null) {
			float diff = Math.Abs (target.Data.Time - time);
			if (diff < excellentTimeSec) {
				return (NoteResult.Excellent, target);
			}
			if (diff < goodTimeSec) {
				return (NoteResult.Good, target);
			}
			if (diff < badTimeSec) {
				return (NoteResult.Bad, target);
			}
			if (diff < missTimeSec) {
				return (NoteResult.Miss, target);
			}
		}
		return (NoteResult.Unknown, null);
	}
}
