using UnityEngine;

/// <summary>
/// タッチした位置に表示するリング。
/// </summary>
public class TouchRing : MonoBehaviour
{
	public void DestroyRing ()
	{
		Destroy (gameObject);
	}
}
