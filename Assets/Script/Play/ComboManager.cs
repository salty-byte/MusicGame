using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コンボ数のカウント、及びコンボテキストの表示・非表示を行う。
/// </summary>
public class ComboManager : MonoBehaviour
{
	[SerializeField]
	Text numText = default;

	[SerializeField]
	Text unitText = default;

	int currentCombo;

	// Use this for initialization
	void Awake ()
	{
		SetActive (false);
		currentCombo = 0;
	}

	/// <summary>
	/// 現在のコンボ数を追加し、表示するコンボテキストを更新する。
	/// </summary>
	public void AddCombo ()
	{
		currentCombo++;
		switch (currentCombo) {
		case 1:
			SetActive (false);
			break;
		case 2:
			numText.text = currentCombo.ToString ();
			SetTextColor ();
			SetActive (true);
			break;
		default:
			numText.text = currentCombo.ToString ();
			if (currentCombo % 100 == 0) {
				SetTextColor ();
			}
			break;
		}
	}

	/// <summary>
	/// コンボテキストを非表示にして、現在のコンボ数を0にする。
	/// </summary>
	public void ResetCombo ()
	{
		SetActive (false);
		currentCombo = 0;
	}

	/// <summary>
	/// コンボテキストの色を設定する。
	/// </summary>
	void SetTextColor ()
	{
		Color color = DecideTextColor ();
		numText.color = color;
		unitText.color = color;
	}

	/// <summary>
	/// コンボテキストの色を現在のコンボ数に応じて変更する。
	/// <list type="table">
	///   <item>  
	///     <term>赤</term>  
	///     <description>100コンボ未満</description>  
	///   </item>  
	///   <item>  
	///     <term>黄</term>  
	///     <description>100コンボ以上、200コンボ未満</description>  
	///   </item>  
	///   <item>  
	///     <term>緑</term>  
	///     <description>200コンボ以上、300コンボ未満</description>  
	///   </item>  
	///   <item>  
	///     <term>青</term>  
	///     <description>300コンボ以上</description>  
	///   </item>  
	/// </list>  
	/// </summary>
	/// <returns>コンボテキストに設定する色</returns>
	Color DecideTextColor ()
	{
		int type = currentCombo / 100;
		switch (type) {
		case 0:
			return Color.red;
		case 1:
			return Color.yellow;
		case 2:
			return Color.green;
		default:
			return Color.blue;
		}
	}

	/// <summary>
	/// コンボテキストの表示・非表示を切り替える。
	/// </summary>
	/// <param name="f">表示する場合true、それ以外はfalse</param>
	void SetActive (bool f)
	{
		numText.gameObject.SetActive (f);
		unitText.gameObject.SetActive (f);
	}
}
