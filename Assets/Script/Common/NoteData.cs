using System;
using UnityEngine;

/// <summary>
/// ノーツデータを保持するクラス。
/// 譜面データとしてjson形式で外部ファイルに出力できるように、<c>Serializable</c>にしている。
/// </summary>
[Serializable]
public class NoteData
{
	[SerializeField]
	float time;  // sec

	[SerializeField]
	int startDir;

	[SerializeField]
	int endDir;

	public float Time { get => time; }  // sec
	public int StartDir { get => startDir; }
	public int EndDir { get => endDir; }

	public NoteData (float t, int d)
	{
		time = t;
		startDir = d;
		endDir = d;
	}
}
