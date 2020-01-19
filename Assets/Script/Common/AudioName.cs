/// <summary>
/// SE名の一覧を保持する。
/// </summary>
public enum SEName
{
	ClickStart, // SE_Click_Start
	Decide,     // SE_Decide
	Excellent,  // SE_Excellent
	Good        // SE_Good
}

/// <summary>
/// SENameの文字列を取得するための拡張クラス。
/// SENameに更新がある場合は、valuesも更新する必要がある。
/// </summary>
public static class SENameExt
{
	/// <summary>
	/// SENameに紐づくシーン名を格納する。
	/// 対応するSENameと順番が一致している必要がある。
	/// </summary>
	static readonly string [] values = {
		"SE_Click_Start",
		"SE_Decide",
		"SE_Excellent",
		"SE_Good"
	};

	/// <summary>
	/// SENameの文字列を取得する。
	/// </summary>
	/// <param name="name">SceneName</param>
	/// <returns>SENameの文字列</returns>
	public static string GetString (this SEName name)
	{
		return values [(int)name];
	}
}
