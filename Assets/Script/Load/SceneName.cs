/// <summary>
/// シーン名の一覧を保持する。
/// </summary>
public enum SceneName
{
	Start,  // Start
	Load,   // Load
	Select, // Select
	Play    // Play
}

/// <summary>
/// SceneNameの文字列を取得するための拡張クラス。
/// SceneNameに更新がある場合は、valuesも更新する必要がある。
/// </summary>
public static class SceneNameExt
{
	/// <summary>
	/// SceneNameに紐づくシーン名を格納する。
	/// 対応するSceneNameと順番が一致している必要がある。
	/// </summary>
	static readonly string [] values = {
		"Start",
		"Load",
		"Select",
		"Play"
	};

	/// <summary>
	/// SceneNameの文字列を取得する。
	/// </summary>
	/// <param name="name">SceneName</param>
	/// <returns>SceneNameの文字列</returns>
	public static string GetString (this SceneName name)
	{
		return values [(int)name];
	}
}
