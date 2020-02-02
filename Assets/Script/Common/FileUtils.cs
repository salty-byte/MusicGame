using System;
using System.IO;
using UnityEngine;

/// <summary>
/// ファイルの読み書きを行うためのユーティリティクラス。
/// </summary>
public static class FileUtils
{
	/// <summary>
	/// ゲームデータを保存するパスを取得する。
	/// ユーザのプラットフォームによって指定するパスを変更する。
	/// </summary>
	/// <returns>ゲームデータを保存するパス文字列を返す。</returns>
	public static string GetGameDataPath ()
	{
#if UNITY_EDITOR
		return Application.dataPath + "/GameData/";
#elif UNITY_ANDROID
		return Application.persistentDataPath + "/GameData/";
#else
		return Application.dataPath + "/GameData/";
#endif
	}

	/// <summary>
	/// 指定したファイルから文字列データを読み込む。
	/// 一括で読み込むため、サイズの小さいファイルに対して利用すること。
	/// </summary>
	/// <param name="path">読み込むファイルのパス文字列。</param>
	/// <returns>指定したファイルから読み込んだ文字列を返す。</returns>
	public static string ReadText (string path)
	{
		string result = "";
		try {
			using (StreamReader reader = new StreamReader (path)) {
				result = reader.ReadToEnd ();
				reader.Close ();
			}
		} catch (Exception e) {
			Debug.Log (e.Message);
		}

		return result;
	}

	/// <summary>
	/// 文字列データを指定したファイルに書き込む。
	/// 一括で書き込むため、サイズの小さい文字列に対して利用すること。
	/// </summary>
	/// <param name="path">書き込むファイルのパス文字列。</param>
	/// <param name="text">ファイルに書き込む文字列。</param>
	/// <returns>書き込みが成功した場合true、それ以外はfalseを返す。</returns>
	public static bool WriteText (string path, string text)
	{
		try {
			using (StreamWriter writer = new StreamWriter (path, false)) {
				writer.Write (text);
				writer.Flush ();
				writer.Close ();
			}
		} catch (Exception e) {
			Debug.Log (e.Message);
			return false;
		}
		return true;
	}
}
