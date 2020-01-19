using System;
using System.Collections.Generic;

[Serializable]
public class MusicData
{
	public string name;          // 曲名
	public string level;         // 難易度
	public string audioFileName; // 楽曲ファイル名
	public List<NoteData> notes; // ノーツを格納するリストデータ

	public MusicData (string name, string level)
	{
		this.name = name;
		this.level = level;
	}
}
