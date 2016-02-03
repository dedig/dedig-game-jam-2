using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlesController : MonoBehaviour {

	public Dictionary<string, ParticleSystem> particlesDict = new Dictionary<string, ParticleSystem>();

	public ParticleSystem[] asParticulas;

	public static ParticlesController instancia;

	void Awake(){
		if(instancia != null){
			Destroy(instancia);
		}
		instancia = this;

		for(int i = 0; i < asParticulas.Length; i++){
			particlesDict.Add(asParticulas[i].name, asParticulas[i]);
		}
	}

	public void EmitirParticulas(string systemName, int numPartsParaEmitir){
		particlesDict[systemName].Emit(numPartsParaEmitir);
	}

	public void EmitirParticulas(string systemName, int numPartsParaEmitir, Vector3 emissionPos){
		particlesDict[systemName].transform.position = emissionPos;
		particlesDict[systemName].Emit(numPartsParaEmitir);
	}

	public void PlayParticleSystem(string systemName){
		particlesDict[systemName].Play();
	}
}
