using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tja形式の譜面ファイルをこのゲームの譜面形式に変換するためのクラス。
/// </summary>
public class TjaLoader
{
	public TjaLoader ()
	{
	}

	public List<NoteData> Load (string filePath)
	{
		// FileReadTest.txtファイルを読み込む
		string [] data = File.ReadAllLines (filePath);

		int startIdx = 0;
		for (int i = 0; i < data.Length; i++) {
			if (data [i] == "#START") {
				startIdx = i;
				break;
			}
		}

		if (startIdx == 0) {
			throw new System.Exception ("#STARTが存在しません。");
		}

		// ヘッダデータ取得
		for (int i = 0; i < startIdx; i++) {

		}

		// ボディデータ取得
		bool comma = false;
		int current = startIdx + 1;
		float mesure = 1;
		float offset = 1.9f;
		float bpm = 128;
		int dir = 0;
		int counta = 0;
		float currentTime = offset;
		List<NoteData> noteDataList = new List<NoteData> ();
		for (int i = startIdx + 1; i < data.Length; i++) {
			if (data [i].Contains (",")) {
				int allcount = 0;
				for (int j = current; j <= i; j++) {
					string line = data [j].Replace (",", "").Trim ();
					if (line.Length != 0 && line [0] != '#') {
						allcount += line.Length;
					}
					Debug.Log ($"line: {line}");
				}
				Debug.Log ($"{current}, {i}: {allcount}");

				for (int j = current; j <= i; j++) {
					string line = data [j].Replace (",", "").Trim ();
					if (line.Length != 0 && line [0] == '#') {
						string [] values = line.Split (' ');
						switch (values [0]) {
						case "#BPMCHANGE":
							bpm = float.Parse (values [1]);
							Debug.Log ($"bpm: {bpm}");
							break;
						case "#MEASURE":
							string [] h = values [1].Split ('/');
							float h1 = float.Parse (h [0]);
							float h2 = float.Parse (h [1]);
							mesure = h1 / h2;
							Debug.Log ($"mesure: {mesure}");
							break;
						}
					} else if (line.Length != 0) {
						float haku = 4 * mesure;
						float noteTime = 60f / bpm / allcount * haku;
						Debug.Log ($"haku: {haku}, t: {noteTime}");

						char [] chars = line.ToCharArray ();
						for (int k = 0; k < chars.Length; k++) {
							float time = currentTime + noteTime * k;
							switch (chars [k]) {
							case '1':  // 小1
								if (counta % 2 == 0) {
									counta += 2;
									dir = (dir + 3) % 6;
								} else {
									counta = 0;
									dir = (dir + 5) % 6;
								}
								noteDataList.Add (new NoteData (time, dir));
								break;
							case '2':  // 小2
								if (counta % 2 == 0) {
									counta = 1;
									dir = (dir + 5) % 6;
								} else {
									counta += 2;
									dir = (dir + 3) % 6;
								}
								noteDataList.Add (new NoteData (time, dir));
								break;
							case '3':  // 大1
								dir = (dir + 3) % 6;
								noteDataList.Add (new NoteData (time, dir));
								break;
							case '4':  // 大2
								dir = (dir + 3) % 6;
								noteDataList.Add (new NoteData (time, dir));
								break;
							case '6':  // ロングノーツ始まり
								noteDataList.Add (new NoteData (time, 1));
								break;
							case '8':  // ロングノーツ終わり
								noteDataList.Add (new NoteData (time, 0));
								break;
							}
						}
						currentTime += (noteTime * chars.Length);
					}
				}
				current = i + 1;
				comma = true;
			} else if (comma) {
				current = i;
				comma = false;
			}

		}

		return noteDataList;
	}
}
