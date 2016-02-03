using UnityEngine;
using System.Collections;

public class ButtonSoundScript : MonoBehaviour {

	public string hoverSound, clickSound;

	public void HoverSound(){
		SoundMan.instancia.EmitirSom(hoverSound);
	}

	public void ClickSound(){
		SoundMan.instancia.EmitirSom(clickSound);
	}
}
