using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ノーツのスピードを調整する設定パネル。
/// </summary>
public class SpeedPanel : MonoBehaviour
{
	[SerializeField]
	Slider slider = default;

	[SerializeField]
	Text text = default;

	public float Speed { get; private set; }

	// Start is called before the first frame update
	void Start ()
	{
		UpdateValue (slider.value);
		slider.onValueChanged.AddListener (UpdateValue);
	}

	/// <summary>
	/// スライダーの値に応じてスピード値を変更する。
	/// デフォルトのスライダーを利用しているため、スピード値を調整して格納する。
	/// </summary>
	/// <param name="value">スライダーの値</param>
	void UpdateValue (float value)
	{
		Speed = value / 10f;
		text.text = Speed.ToString ();
	}
}
