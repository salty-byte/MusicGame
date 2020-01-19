using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// セレクト画面の選択項目となるノード。
/// </summary>
public class SelectNode : MonoBehaviour
{
	Text title;
	float radiusX;
	float radiusY;
	float baseY;

	/// <summary>
	/// ノードのx座標。
	/// </summary>
	public float X { get; private set; }

	/// <summary>
	/// ノードに格納されている楽曲データ。
	/// </summary>
	public MusicData Md { get; private set; }

	public void Awake ()
	{
		title = GetComponentInChildren<Text> ();
	}

	/// <summary>
	/// ノードの初期化を行う。
	/// <c>x</c>、<c>y</c>で移動の基準となる位置を与える。
	/// <c>radiusX</c>、<c>radiusY</c>で移動の軸となる楕円の横幅/縦幅を与える。
	/// </summary>
	/// <param name="x">初期x座標。</param>
	/// <param name="y">y座標の基準位置。</param>
	/// <param name="radiusX">移動の軸となる楕円の横幅。</param>
	/// <param name="radiusY">移動の軸となる楕円の縦幅。</param>
	public void Init (float x, float y, float radiusX, float radiusY)
	{
		X = x;
		baseY = y;
		this.radiusX = radiusX;
		this.radiusY = radiusY;

		// 初期ノードの位置とスケールを反映する。
		MoveX (0);
	}

	/// <summary>
	/// 指定した値分、ノードのx座標を移動する。
	/// </summary>
	/// <param name="diff">x軸方向に移動する値。</param>
	public void MoveX (float diff)
	{
		UpdateX (X + diff);
	}

	/// <summary>
	/// ノードのx座標を与えて、ノードの位置とスケールを更新する。
	/// </summary>
	/// <param name="x">ノードのx座標。</param>
	public void UpdateX (float x)
	{
		X = x;
		SetPositionX (X);
		SetScale ((radiusX - Mathf.Abs (X) * 0.5f) / radiusX);
	}

	/// <summary>
	/// ノードのx座標を指定した値に設定し、y座標を計算して適用する。
	/// y座標は楕円の方程式から計算する。
	/// </summary>
	/// <param name="x">ノードのx座標。</param>
	void SetPositionX (float x)
	{
		float da = x / radiusX;
		float y = baseY - radiusY * (1f - da * da);
		transform.position = new Vector3 (x, y, 0);
	}

	/// <summary>
	/// ノードの表示スケールを指定した値に設定する。
	/// 2Dのため、x軸、y軸に対して適用する。z軸は1fの固定値になる。
	/// </summary>
	/// <param name="scale">変更後のスケール。</param>
	void SetScale (float scale)
	{
		transform.localScale = new Vector3 (scale, scale, 1f);
	}

	/// <summary>
	/// ノードが保持している楽曲データを更新する。
	/// 画面に表示する楽曲名と、内部の楽曲データを更新する。
	/// </summary>
	/// <param name="md">MusicData。</param>
	public void UpdateMusicData (MusicData md)
	{
		title.text = md.name;
		Md = md;
	}
}
