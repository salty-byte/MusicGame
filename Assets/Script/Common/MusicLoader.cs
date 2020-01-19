using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 楽曲音源データをロードするためのクラス。
/// </summary>
public class MusicLoader
{
	/// <summary>
	/// 楽曲音源データを非同期で読み込む。
	/// OGGファイルのみ対応している。
	/// </summary>
	/// <param name="path">楽曲データを格納しているファイルパス。</param>
	/// <returns>IEnumeratorを返す。ファイル読み込みが正常に終了した場合、AudioClipデータが格納される。</returns>
	public static IEnumerator LoadAudioClipAsync (string path)
	{
		if (string.IsNullOrEmpty (path) || !File.Exists (path)) {
			Debug.Log ($"[LoadMusic] File not found: {path}");
			yield break;
		}

		using (var request = UnityWebRequestMultimedia.GetAudioClip ("file://" + path, AudioType.OGGVORBIS)) {
			yield return request.SendWebRequest ();

			// エラー判定
			if (request.isHttpError || request.isNetworkError) {
				Debug.Log ($"[LoadMusic] Loading error: {request.error}");
				yield break;
			}

			var audioClip = DownloadHandlerAudioClip.GetContent (request);
			yield return audioClip;
		}
	}
}
