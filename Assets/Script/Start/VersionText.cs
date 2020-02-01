using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スタート画面にアプリのリリースバージョンを表示するためのText用クラス。
/// </summary>
[RequireComponent (typeof (Text))]
public class VersionText : MonoBehaviour
{
	// Start is called before the first frame update
	void Start ()
	{
		GetComponent<Text> ().text = $"ver. {Application.version}";
	}
}
