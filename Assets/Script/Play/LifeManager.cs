using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 楽曲プレイ中のライフを管理する。
/// ライフの計算および、ライフゲージの表示を行う。
/// </summary>
public class LifeManager : MonoBehaviour
{
	[SerializeField]
	Text lifeNumText = default;

	[SerializeField]
	Image lifeGauge = default;

	[SerializeField]
	float maxLife = 10f;

	float currentLife;

	void Awake ()
	{
		currentLife = maxLife;
		UpdateLife ();
	}

	/// <summary>
	/// 現在のライフに<c>add</c>分加算する。
	/// 減算する場合は、負の数を与えること。
	/// </summary>
	/// <param name="add">加算する数値。</param>
	/// <returns>加算後のライフが0より大きい場合true、それ以外はfalseを返す。</returns>
	public bool AddCurrentLife (int add)
	{
		currentLife += add;
		if (currentLife < 0) {
			currentLife = 0;
		} else if (currentLife > maxLife) {
			currentLife = maxLife;
		}
		UpdateLife ();
		return currentLife > 0;
	}

	/// <summary>
	/// ライフ表示を更新する。
	/// </summary>
	void UpdateLife ()
	{
		lifeNumText.text = currentLife.ToString ();
		lifeGauge.fillAmount = currentLife / maxLife;
	}
}
