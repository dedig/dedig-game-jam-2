using UnityEngine;
using System.Collections;

public class SkipIntroAnim : MonoBehaviour {

	bool jaPulou = false;

	public float minTempoEspera;
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown && !jaPulou){
			if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("capaPresent")){
				GetComponent<Animator>().Play("capaLoop", 0, 0);
				jaPulou = true;
			}else{
				jaPulou = true;
			}
		}
	}
}
