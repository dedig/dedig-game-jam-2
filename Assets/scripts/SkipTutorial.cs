using UnityEngine;
using System.Collections;

public class SkipTutorial : MonoBehaviour {

	public float minTempoEspera;
	static bool podePular = false;

	// Use this for initialization
	void Start () {
		Invoke ("PermitePulo", minTempoEspera);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKey && podePular){
			GetComponent<Animator>().Play("tutorialDisappear", 0, 0);
			SoundMan.instancia.EmitirSom("Golpe no Ar", 0.2f);
			this.enabled = false;
		}
	}

	void PermitePulo(){
		podePular = true;
	}
}
