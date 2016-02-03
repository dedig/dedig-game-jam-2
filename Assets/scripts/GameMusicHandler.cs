using UnityEngine;
using System.Collections;
/// <summary>
/// Game music handler. esse script mete o dj e faz uma musica entrar dps da outra
/// </summary>
public class GameMusicHandler : MonoBehaviour {

	public AudioSource firstMusic;

	public AudioSource secondMusic;

	// Use this for initialization
	void Start () {
		firstMusic.Play();
		Invoke("PlaySecondMusic", firstMusic.clip.length);
	}

	void PlaySecondMusic(){
		secondMusic.Play();
	}

}
