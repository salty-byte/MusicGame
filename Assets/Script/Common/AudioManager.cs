using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// ゲーム中のSEやBGMの再生を管理するクラス。
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
	[SerializeField]
	List<AudioClip> SEList = default;

	[SerializeField, Range (0, 1)]
	float bgmVolume = 0.5f;

	[SerializeField, Range (0, 1)]
	float seVolume = 0.5f;

	AudioSource bgmSource;
	List<AudioSource> seSources;
	Dictionary<string, AudioClip> seDict;

	public void Awake ()
	{
		if (this != Instance) {
			Destroy (this);
			return;
		}
		DontDestroyOnLoad (gameObject);

		// AudioSourceの作成
		bgmSource = gameObject.AddComponent<AudioSource> ();
		seSources = new List<AudioSource> ();

		// AudioClip再生用のDictionaryを作成
		seDict = new Dictionary<string, AudioClip> ();
		SEList.ForEach (se => {
			if (!seDict.ContainsKey (se.name)) {
				seDict.Add (se.name, se);
			}
		});
	}

	/// <summary>
	/// SEを再生する。
	/// Dictionaryに<c>seName</c>が存在しない場合は、ArgmentExceptionを投げる。
	/// </summary>
	/// <param name="seName">再生するSE名。</param>
	public void PlaySE (string seName)
	{
		if (!seDict.ContainsKey (seName)) {
			throw new ArgumentException (seName + " not found", nameof (seName));
		}

		AudioSource source = seSources.FirstOrDefault (s => !s.isPlaying);
		if (source == null) {
			source = gameObject.AddComponent<AudioSource> ();
			seSources.Add (source);
		}

		source.clip = seDict [seName];
		source.volume = seVolume;
		source.Play ();
	}

	/// <summary>
	/// 再生中のSEを全て中断する。
	/// </summary>
	public void StopSE ()
	{
		seSources.ForEach (s => s.Stop ());
	}

	/// <summary>
	/// BGMとして使うためのAudioClipを設定する。
	/// </summary>
	/// <param name="audioClip">再生するBGMのAudioClip。</param>
	public void SetBGM (AudioClip audioClip)
	{
		bgmSource.clip = audioClip;
	}

	/// <summary>
	/// BGMとして設定されているAudioClipを再生する。
	/// </summary>
	public void PlayBGM ()
	{
		bgmSource.volume = bgmVolume;
		bgmSource.Play ();
	}

	/// <summary>
	/// 再生中のBGMを中断する。
	/// </summary>
	public void PauseBGM ()
	{
		bgmSource.Pause ();
	}

	/// <summary>
	/// 中断しているBGMを再開する。
	/// </summary>
	public void UnPauseBGM ()
	{
		bgmSource.UnPause ();
	}

	/// <summary>
	/// BGMを停止し、BGMとして設定されているAudioClipをnullにする。
	/// </summary>
	public void StopBGM ()
	{
		bgmSource.Stop ();
		bgmSource.clip = null;
	}

	/// <summary>
	/// 現在のBGM経過時間（秒）を返す。
	/// </summary>
	/// <returns>BGM経過時間（秒）を返す。</returns>
	public float GetBGMTime ()
	{
		return bgmSource.time;
	}
}