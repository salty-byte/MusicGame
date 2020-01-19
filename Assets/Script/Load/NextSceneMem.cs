/// <summary>
/// 遷移先のシーン名を保持するためのクラス。
/// シーンが移動しても削除されない。
/// </summary>
public class NextSceneMem : SingletonMonoBehaviour<NextSceneMem>
{
	public SceneName NextSceneName { get; set; }

	public void Awake ()
	{
		if (this != Instance) {
			Destroy (this);
			return;
		}
		DontDestroyOnLoad (gameObject);

		// デフォルトはスタート画面名に設定する。
		NextSceneName = SceneName.Start;
	}
}
