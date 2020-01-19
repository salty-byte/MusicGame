using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class BackGround : MonoBehaviour
{
	void Awake ()
	{
		Expand ();
	}

	void Expand ()
	{
		// カメラのワールドサイズを取得する。
		float ch = Camera.main.orthographicSize * 2f;
		float cw = ch / Screen.height * Screen.width;

		// 画像のワールドサイズを取得する。
		var render = GetComponent<SpriteRenderer> ();
		float sw = render.sprite.bounds.size.x;
		float sh = render.sprite.bounds.size.y;

		// 画像のスケールを設定する。
		transform.localScale = new Vector3 (cw / sw, ch / sh, 1f);
	}
}
