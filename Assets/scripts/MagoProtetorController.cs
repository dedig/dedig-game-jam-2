using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MagoProtetorController : InimigoController {

	public Transform fleeFromTransform;

	public float fleeRange;

	public TotemScript oTotem;

	public LineRenderer myLaser;

	// Update is called once per frame
	void Update () {
		if(myTarget){
			//vamos olhar pro alvo e chegar perto o suficiente para atacar
			Vector3 deltaPos = myTarget.position - transform.position;
			
			transform.rotation = 
				Quaternion.Euler(Vector3.forward * (Mathf.Rad2Deg * 
				                                    Mathf.Atan2(deltaPos.y, deltaPos.x) - 90));
			
			if(Vector2.Distance(myTarget.position, transform.position) <= attackRange){
				oTotem.AtualizarLigacaoDoMago(this);
					//liga o laser

				//atualiza o laser!
				myLaser.SetPosition(0, transform.position);
				myLaser.SetPosition(1, oTotem.transform.position);

				if(fleeFromTransform){
					if(Vector2.Distance(fleeFromTransform.position, transform.position) >= fleeRange){
						myRigid.velocity = Vector2.Lerp(myRigid.velocity, Vector2.zero, Time.deltaTime * moveSpeed);
					}else{
						myRigid.velocity = (transform.position - fleeFromTransform.position).normalized * moveSpeed;
					}
				}

			}else{
				oTotem.DesligarMago(this);
				myRigid.velocity = transform.up * moveSpeed;
			}
		}
	}

	public void ToggleLaser(bool ativar){
		myLaser.enabled = ativar;
	}

	public override void SerAtacado(float dano){

		base.SerAtacado(dano);

		if(curVida <= 0){
			if(oTotem.magosLigados.Count == 1 && oTotem.magosLigados.Contains(this)){
				SoundMan.instancia.soundsDict["Broken Shield"].DOKill();
				SoundMan.instancia.EmitirSom("Broken Shield", 0.2f, oTotem.transform.position);
				SoundMan.instancia.soundsDict["Broken Shield"].DOPitch(0.05f, 1.35f);
			}
			oTotem.DesligarMago(this);
		}


	}

}
