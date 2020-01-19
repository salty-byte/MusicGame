using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 楽曲セレクト画面の管理を行うクラス。
/// </summary>
public class SelectManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	[SerializeField]
	PlaySettingPanel settingPanel = default;

	[SerializeField]
	MusicInfo musicInfo = default;

	[SerializeField]
	SelectNode [] nodes = new SelectNode [0];

	public float yp = 4.5f;
	public float xr = 16f;
	public float yr = 4f;

	float worldWidth = 12f;
	float startX;

	int currentListIdx = 0;

	List<MusicData> dataList;
	MusicData selectedMusic;
	bool flickable;

	// Start is called before the first frame update
	void Start ()
	{
		float baseWidth = worldWidth / 3;
		for (int i = 0; i < nodes.Length; i++) {
			nodes [i].Init (-worldWidth + baseWidth * i, yp, xr, yr);
		}

		LoadMusicData ();
		InitMusicData ();
		UpdateSelectedMusic ();
	}

	/// <summary>
	/// タップまたはキー入力開始時の処理を行う。
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerDown (PointerEventData eventData)
	{
		flickable = false;
		startX = GetInputWorldPosition ().x;
	}

	/// <summary>
	/// タップまたはキー入力中の処理を行う。
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag (PointerEventData eventData)
	{
		float diff = GetInputWorldPosition ().x - startX;
		MoveNodesX (diff);
		startX = GetInputWorldPosition ().x;
	}

	/// <summary>
	/// タップまたはキー入力終了時の処理を行う。
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerUp (PointerEventData eventData)
	{
		flickable = true;

		// フリック処理 or スワイプ処理
		float diff = GetInputWorldPosition ().x - startX;
		if (diff > 0.05f) {
			StartCoroutine (DoFlickPlusX (diff));
		} else if (diff < -0.05f) {
			StartCoroutine (DoFlickMinusX (diff));
		} else {
			UpdateSelectedMusic ();
		}
	}

	/// <summary>
	/// 楽曲データを外部ファイルからロードする。
	/// </summary>
	void LoadMusicData ()
	{
		dataList = new List<MusicData> ();
		var dir = new DirectoryInfo (FileUtils.GetGameDataPath ());
		FileInfo [] info = dir.GetFiles ("*.msd");
		foreach (var f in info) {
			string text = FileUtils.ReadText (f.FullName);
			if (!string.IsNullOrEmpty (text)) {
				try {
					var md = JsonUtility.FromJson<MusicData> (text);
					dataList.Add (md);
				} catch {
					Debug.Log ($"楽曲データ[{f.FullName}]の読み込み中にエラーが発生しました。");
				}
			}
		}
	}

	/// <summary>
	/// 楽曲データの初期処理を行う。
	/// </summary>
	void InitMusicData ()
	{
		int count = dataList.Count;
		if (count == 0) {
			throw new Exception ("楽曲データがありません。");
		}

		for (int i = 0; i < nodes.Length; i++) {
			nodes [i].UpdateMusicData (dataList [i % count]);
		}

		currentListIdx = (nodes.Length - 1) % count;
	}

	/// <summary>
	/// 前の楽曲を設定する。
	/// </summary>
	/// <param name="node">楽曲の設定対象となるノード。</param>
	void SetPrevMusicData (SelectNode node)
	{
		currentListIdx = (dataList.Count + currentListIdx - 1) % dataList.Count;
		node.UpdateMusicData (dataList [currentListIdx]);
	}

	/// <summary>
	/// 次の楽曲を設定する。
	/// </summary>
	/// <param name="node">楽曲の設定対象となるノード。</param>
	void SetNextMusicData (SelectNode node)
	{
		currentListIdx = (currentListIdx + 1) % dataList.Count;
		node.UpdateMusicData (dataList [currentListIdx]);
	}

	/// <summary>
	/// x軸方向のフリック処理を行う。
	/// </summary>
	/// <param name="accel">加速度。</param>
	/// <returns>IEnumerator</returns>
	IEnumerator DoFlickPlusX (float accel)
	{
		accel = Mathf.Min (accel, 1f); // 加速制限
		while (flickable) {
			accel -= 0.05f;
			if (accel < 0) {
				UpdateSelectedMusic ();
				break;
			}
			MoveNodesX (accel);
			yield return null;
		}
	}

	/// <summary>
	/// マイナスx軸方向のフリック処理を行う。
	/// </summary>
	/// <param name="accel"><加速度。/param>
	/// <param name="accel"></param>
	/// <returns>IEnumerator</returns>
	IEnumerator DoFlickMinusX (float accel)
	{
		accel = Mathf.Max (accel, -1f); // 加速制限
		while (flickable) {
			accel += 0.05f;
			if (accel > 0) {
				UpdateSelectedMusic ();
				break;
			}
			MoveNodesX (accel);
			yield return null;
		}
	}

	/// <summary>
	/// 選択ノードのx座標の位置を<c>diff</c>分移動する。
	/// </summary>
	/// <param name="diff">x軸方向に移動する距離。</param>
	void MoveNodesX (float diff)
	{
		foreach (var node in nodes) {
			node.MoveX (diff);

			// 画面外に出たノードを再利用する処理。
			if (node.X <= -worldWidth) {
				node.UpdateX (2 * worldWidth + node.X);
				SetNextMusicData (node);
			} else if (node.X >= worldWidth) {
				node.UpdateX (node.X - 2 * worldWidth);
				SetPrevMusicData (node);
			}
		}
	}

	/// <summary>
	/// 選択ノードの位置を更新し、選択されている楽曲を更新する。
	/// </summary>
	void UpdateSelectedMusic ()
	{
		// 中央に一番近いノードを探す。
		float min = worldWidth;
		SelectNode centerNode = null;
		foreach (var node in nodes) {
			float x = Mathf.Abs (node.X);
			if (x < min) {
				min = x;
				centerNode = node;
			}
		}

		if (centerNode != null) {

			// 位置を中央に揃える。
			MoveNodesX (-centerNode.X);

			// 現在選択中の楽曲データを更新する。
			selectedMusic = centerNode.Md;

			// 画面の楽曲データ情報を更新する。
			musicInfo.SetInfo (selectedMusic);
		}
	}

	/// <summary>
	/// 楽曲を決定し、プレイシーンに遷移する。
	/// </summary>
	public void DecideMusic ()
	{
		if (selectedMusic == null) {
			throw new Exception ("楽曲が選択されていません。");
		}

		var playInfo = PlayInfo.Instance;
		playInfo.Music = selectedMusic;
		playInfo.Settings = settingPanel.GetPlaySettings ();
		AudioManager.Instance.PlaySE (SEName.Decide.GetString ());

		NextSceneMem.Instance.NextSceneName = SceneName.Play;
		SceneManager.LoadScene (SceneName.Load.GetString ());
	}

	Vector3 GetInputWorldPosition ()
	{
		return Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}
}

