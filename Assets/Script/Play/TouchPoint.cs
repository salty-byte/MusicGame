using UnityEngine;

/// <summary>
/// タッチする位置。
/// </summary>
public class TouchPoint : MonoBehaviour
{
	[SerializeField]
	int id = default;

	/// <summary>
	/// タッチの位置ID。
	/// </summary>
	public int Id { get => id; }
}
