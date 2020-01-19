public class PlayInfo : SingletonMonoBehaviour<PlayInfo>
{
	public PlaySettings Settings { get; set; }
	public MusicData Music { get; set; }

	void Awake ()
	{
		if (this != Instance) {
			Destroy (this);
			return;
		}
		DontDestroyOnLoad (gameObject);
	}
}
