using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイ時の設定を管理するパネル。
/// </summary>
public class PlaySettingPanel : MonoBehaviour
{
	[SerializeField]
	Toggle autoToggle = default;

	[SerializeField]
	SpeedPanel speedPanel = default;

	/// <summary>
	/// 現在のプレイ設定を取得する。
	/// </summary>
	/// <returns>プレイ設定を返す。</returns>
	public PlaySettings GetPlaySettings ()
	{
		var settings = new PlaySettings ();

		bool isAuto = autoToggle.isOn;
		if (isAuto) {
			settings.Mode = PlayMode.Auto;
		} else {
#if UNITY_EDITOR
			settings.Mode = PlayMode.Key;
#elif UNITY_ANDROID
			settings.Mode = PlayMode.Touch;
#else
			settings.Mode = PlayMode.Key;
#endif
		}

		settings.Speed = speedPanel.Speed;
		return settings;
	}
}
