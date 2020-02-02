using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 楽曲プレイ画面の管理を行う。
/// </summary>
public class GameManager : MonoBehaviour
{
	[SerializeField]
	GameObject canvas = default;

	[SerializeField]
	TouchPoint [] touchPoints = default;

	[SerializeField]
	GameObject notes = default;

	[SerializeField]
	LifeManager lifeManager = default;

	[SerializeField]
	ComboManager comboManager = default;

	[SerializeField]
	EvalText evalText = default;

	[SerializeField]
	Text musicNameText = default;

	[SerializeField]
	NoteManager noteManager = default;

	[SerializeField]
	ScoreManager scoreManager = default;

	[SerializeField]
	GameObject pausePanel = default;

	[SerializeField]
	float setTimeSec = 0.6f;

	[SerializeField]
	PlayMode mode = PlayMode.Touch;

	GameStatus status;
	AudioManager audioManager;

	// Use this for initialization
	void Awake ()
	{
		QualitySettings.vSyncCount = 0;   // VSyncをOFFにする
		Application.targetFrameRate = 60; // ターゲットフレームレートを60に設定

		status = GameStatus.Loading;
	}

	IEnumerator Start ()
	{
		// Updateが実行されないようにする。
		enabled = false;

		// ゲーム設定の反映
		PlaySettings settings = PlayInfo.Instance.Settings;
		mode = settings.Mode;
		setTimeSec = 1.7f - settings.Speed * 0.15f;

		// 楽曲データのロード
		audioManager = AudioManager.Instance;
		yield return StartCoroutine (LoadMusic ());

		// ゲームスタート
		StartGame ();

		// Updateを許可する。
		enabled = true;
	}

	// Update is called once per frame
	void Update ()
	{
		// ノーツの移動処理
		noteManager.MoveDeltaTime (Time.deltaTime);

		// プレイ時やそれ以外での入力処理。
		ProcessCommonInput ();

		// プレイ時の入力処理。
		if (status == GameStatus.Playing) {
			float nowTime = audioManager.GetBGMTime ();
			PrepareNote (nowTime);
			ProcessInput (nowTime);
		}
	}

	/// <summary>
	/// 楽曲データをロードする。
	/// </summary>
	/// <returns>IEnumerator</returns>
	IEnumerator LoadMusic ()
	{
		MusicData md = PlayInfo.Instance.Music;
		musicNameText.text = md.name;
		noteManager.Initialize (md.notes);
		var func = MusicLoader.LoadAudioClipAsync (FileUtils.GetGameDataPath () + "/" + md.audioFileName);
		yield return StartCoroutine (func);
		var audioClip = (AudioClip)func.Current;
		audioManager.SetBGM (audioClip);
	}

	/// <summary>
	/// プレイ中のユーザからの入力を処理する。
	/// 現在のプレイモードによって処理を変える。
	/// </summary>
	/// <param name="time">楽曲の経過時間（秒）。</param>
	void ProcessInput (float time)
	{
		switch (mode) {
		case PlayMode.Auto:
			PlayAuto (time);
			break;
		case PlayMode.Touch:
			PlayTouch (time);
			break;
		case PlayMode.Key:
			PlayKey (time);
			break;
		}
	}

	/// <summary>
	/// プレイ中だけでなく、ポーズ中のユーザ入力を処理する。
	/// 主にPCからのキー入力を処理するため、スマホ用には不要な処理。
	/// </summary>
	void ProcessCommonInput ()
	{
		// pause or unpause
		if (Input.GetKeyDown (KeyCode.Space)) {
			OnPause ();
		}

		// restart game when paused
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (status == GameStatus.Paused) {
				RestartGame ();
			}
		}
	}

	/// <summary>
	/// オートプレイ時の処理
	/// </summary>
	/// <param name="time">楽曲の経過時間（秒）。</param>
	void PlayAuto (float time)
	{
		if (noteManager.GetAuto (time) != -1) {
			Note [] allNotes = notes.GetComponentsInChildren<Note> ();
			Determine (NoteResult.Excellent, allNotes [0]);
		}
	}

	/// <summary>
	/// スマホプレイ時の処理
	/// </summary>
	/// <param name="time">楽曲の経過時間（秒）。</param>
	void PlayTouch (float time)
	{
		if (Input.touchCount > 0) {
			foreach (var touch in Input.touches) {
				if (touch.phase != TouchPhase.Began) {
					continue;
				}

				Vector3 tp = Camera.main.ScreenToWorldPoint (touch.position);
				Collider2D collider2d = Physics2D.OverlapPoint (tp);

				if (collider2d) {
					GameObject obj = collider2d.transform.gameObject;

					foreach (var p in touchPoints) {
						if (p.gameObject == obj) {
							DetermineTap (p.Id, time);
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// PCプレイ時の処理
	/// </summary>
	/// <param name="time">楽曲の経過時間（秒）。</param>
	void PlayKey (float time)
	{
		// TapPoint1
		if (Input.GetKeyDown (KeyCode.S)) {
			DetermineTap (touchPoints [0].Id, time);
		}

		// TapPoint2
		if (Input.GetKeyDown (KeyCode.D)) {
			DetermineTap (touchPoints [1].Id, time);
		}

		// TapPoint3
		if (Input.GetKeyDown (KeyCode.F)) {
			DetermineTap (touchPoints [2].Id, time);
		}

		// TapPoint4
		if (Input.GetKeyDown (KeyCode.J)) {
			DetermineTap (touchPoints [3].Id, time);
		}

		// TapPoint5
		if (Input.GetKeyDown (KeyCode.K)) {
			DetermineTap (touchPoints [4].Id, time);
		}

		// TapPoint6
		if (Input.GetKeyDown (KeyCode.L)) {
			DetermineTap (touchPoints [5].Id, time);
		}
	}

	/// <summary>
	/// ノーツをタッチした際の処理を行う。
	/// </summary>
	/// <param name="id">タッチポイントのID。</param>
	/// <param name="time">楽曲の経過時間（秒）。</param>
	void DetermineTap (int id, float time)
	{
		(var result, var note) = noteManager.Judge (id, time);
		Determine (result, note);
	}

	/// <summary>
	/// ノーツのタッチ結果を元に、得点計算や画面へのエフェクト表示を行う。
	/// </summary>
	/// <param name="result">ノーツのタッチ結果。</param>
	/// <param name="note">タッチしたノーツ。</param>
	public void Determine (NoteResult result, Note note)
	{
		if (result == NoteResult.Unknown) {
			return;
		}

		scoreManager.CalcScore (result);
		CreateTouchNoteEffect (note, result);
		switch (result) {
		case NoteResult.Excellent:
		case NoteResult.Good:
			comboManager.AddCombo ();
			break;
		case NoteResult.Bad:
			comboManager.ResetCombo ();
			break;
		case NoteResult.Miss:
			AddLife (-1);
			comboManager.ResetCombo ();
			break;
		}
	}

	/// <summary>
	/// ライフ数を追加する。
	/// </summary>
	/// <param name="point">追加するライフ数。ライフを減らす場合、負の数を与える。</param>
	void AddLife (int point)
	{
		bool result = lifeManager.AddCurrentLife (point);
		if (!result && status == GameStatus.Playing) {
			EndGame (false);
		}
	}

	/// <summary>
	/// ノーツのタッチ結果によって演出を行う。
	/// </summary>
	/// <param name="note">タッチしたノーツ。</param>
	/// <param name="result">ノーツのタッチ結果。</param>
	void CreateTouchNoteEffect (Note note, NoteResult result)
	{
		// [Excellent > Good > Average > Miss]
		switch (result) {
		case NoteResult.Excellent:
			audioManager.PlaySE (SEName.Excellent.GetString ());
			ShowEvalText ("Excellent");
			break;
		case NoteResult.Good:
			audioManager.PlaySE (SEName.Good.GetString ());
			ShowEvalText ("Good");
			break;
		case NoteResult.Bad:
			ShowEvalText ("Bad");
			break;
		case NoteResult.Miss:
			ShowEvalText ("Miss");
			break;
		}
		Destroy (note.gameObject);
	}

	/// <summary>
	/// ノーツのタッチ判定結果をテキストとして表示する。
	/// </summary>
	/// <param name="eval">評価を表す文字列。</param>
	void ShowEvalText (string eval)
	{
		evalText.SetNewText (eval);
	}

	/// <summary>
	/// 現在の楽曲時刻からノーツの準備をする。
	/// </summary>
	/// <param name="time">現在の楽曲時刻（秒）。</param>
	void PrepareNote (float time)
	{
		noteManager.PrepareNote (time, setTimeSec);
	}

	/// <summary>
	/// ゲームの開始処理。
	/// </summary>
	void StartGame ()
	{
		audioManager.PlayBGM ();
		status = GameStatus.Playing;
	}

	/// <summary>
	/// ボタンやキー入力でポーズ処理が呼ばれた時の処理。
	/// 現在のゲームステータスによって処理を変える。
	/// ゲームがプレイ中の場合、一時停止する。
	/// ゲームが一時停止中の場合、一時停止を解除する。
	/// それ以外の場合、何もしない。
	/// </summary>
	public void OnPause ()
	{
		if (status == GameStatus.Playing) {
			PauseGame ();
		} else if (status == GameStatus.Paused) {
			UnPauseGame ();
		}
	}

	/// <summary>
	/// ゲームを一時停止する。
	/// TODO タップリングは停止させないように処理を変える。
	/// </summary>
	void PauseGame ()
	{
		status = GameStatus.Paused;
		audioManager.PauseBGM ();
		Time.timeScale = 0f;
		pausePanel.SetActive (true);
	}

	/// <summary>
	/// ゲームの一時停止を解除する。
	/// </summary>
	void UnPauseGame ()
	{
		pausePanel.SetActive (false);
		audioManager.UnPauseBGM ();
		Time.timeScale = 1f;
		status = GameStatus.Playing;
	}

	/// <summary>
	/// 曲を最初からやり直す。
	/// シーン移動を利用して、最初の状態にする。
	/// </summary>
	public void RestartGame ()
	{
		GoNextScene (SceneName.Play);
	}

	/// <summary>
	/// ゲームの終了処理。
	/// </summary>
	/// <param name="type">タップ時の判定。</param>
	void EndGame (bool isClear)
	{
		if (isClear) {
			CreateEndGameText ("CLEAR!!");
		} else {
			CreateEndGameText ("FAILED");
		}

		status = GameStatus.Finished;
		audioManager.StopBGM ();

		StartCoroutine (GoResultSceneDelayed (3f));
	}

	/// <summary>
	/// 楽曲プレイ終了時に表示するテキストを作成する。
	/// </summary>
	/// <param name="str">テキストに設定する文字列。</param>
	/// <returns>作成したテキストオブジェクト。</returns>
	GameObject CreateEndGameText (string str)
	{
		GameObject obj = (GameObject)Resources.Load ("Prefabs/EndGameText");
		GameObject endText = Instantiate (obj, obj.transform.position, Quaternion.identity, canvas.transform);
		endText.GetComponent<Text> ().text = str;
		return endText;
	}

	/// <summary>
	/// 曲選択画面へ遷移する。
	/// </summary>
	public void GoSelectScene ()
	{
		GoNextScene (SceneName.Select);
	}

	/// <summary>
	/// ゲーム結果画面へ遷移する。
	/// </summary>
	void GoResultScene ()
	{
		GoNextScene (SceneName.Select); // TODO Resultシーンを作成する。
	}

	/// <summary>
	/// <c>time</c>秒後にゲーム結果画面へ遷移する。
	/// </summary>
	/// <param name="time">遷移するまでの待機時間（秒）。</param>
	/// <returns></returns>
	IEnumerator GoResultSceneDelayed (float time)
	{
		yield return new WaitForSeconds (time);
		GoResultScene ();
	}

	/// <summary>
	/// 現在のシーンから次のシーンへ移動する。
	/// </summary>
	/// <param name="sceneName">移動先のシーン名。</param>
	void GoNextScene (SceneName sceneName)
	{
		// 一時停止時からのシーン移動を考慮する。
		if (status == GameStatus.Paused) {
			Time.timeScale = 1f;
		}

		QualitySettings.vSyncCount = 1; // VSyncを元に戻す。
		Application.targetFrameRate = -1; // ターゲットフレームレートを元に戻す。

		NextSceneMem.Instance.NextSceneName = sceneName;
		SceneManager.LoadScene (SceneName.Load.GetString ());
	}
}
