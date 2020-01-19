using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 楽曲セレクト画面で、現在選択されている楽曲の情報を表示する。
/// </summary>
public class MusicInfo : MonoBehaviour
{
	[SerializeField]
	Text nameText = default;

	[SerializeField]
	Text levelText = default;

	/// <summary>
	/// 楽曲情報を設定する。
	/// </summary>
	/// <param name="md">設定する楽曲データ。</param>
	public void SetInfo (MusicData md)
	{
		nameText.text = md.name;
		levelText.text = md.level;
	}
}
