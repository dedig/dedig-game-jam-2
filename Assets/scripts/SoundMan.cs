using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundMan : MonoBehaviour {

	public Dictionary<string, AudioSource> soundsDict = new Dictionary<string, AudioSource>();
	
	public AudioSource[] osSons;
	
	public static SoundMan instancia;
	
	void Awake(){
		if(instancia != null){
			Destroy(instancia);
		}
		instancia = this;
		
		for(int i = 0; i < osSons.Length; i++){
			soundsDict.Add(osSons[i].name, osSons[i]);
		}
	}
	
	public void EmitirSom(string sourceName, float pitchVariation){
		soundsDict[sourceName].pitch = 1 + Random.Range(-pitchVariation, pitchVariation);
		soundsDict[sourceName].Play();
	}
	
	public void EmitirSom(string sourceName, float pitchVariation, Vector3 emissionPos){
		soundsDict[sourceName].transform.position = emissionPos;
		soundsDict[sourceName].pitch = 1 + Random.Range(-pitchVariation, pitchVariation);
		soundsDict[sourceName].Play();
	}
	
	public void EmitirSom(string sourceName){
		soundsDict[sourceName].Play();
	}
}
