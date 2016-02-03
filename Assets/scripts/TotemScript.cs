using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TotemScript : MonoBehaviour, IAttackable {

	public bool shielded = false;

	public SpriteRenderer shieldSprite;

	public EnemySummoner mySummoner;

	public AudioSource shieldLoop;

	public List<MagoProtetorController> magosLigados = new List<MagoProtetorController>();

	public void AtualizarLigacaoDoMago(MagoProtetorController oMago){
		if(!magosLigados.Contains(oMago)){
			magosLigados.Add(oMago);
			oMago.ToggleLaser(true);

			if(!shielded){
				ToggleShield(true);
			}
		}
	}

	public void DesligarMago(MagoProtetorController oMago){
		if(magosLigados.Contains(oMago)){
			magosLigados.Remove(oMago);
			oMago.ToggleLaser(false);

			if(magosLigados.Count == 0){
				ToggleShield(false);
			}
		}
	}

	public void ToggleShield(bool ativar){
		if(ativar){
			shieldSprite.DOFade(1, 0.8f);
			shieldLoop.DOFade(0.6f, 0.8f);
		}else{
			shieldSprite.DOFade(0, 0.8f);
			shieldLoop.DOFade(0, 0.8f);
		}

		shielded = ativar;
	}

	#region IAttackable implementation

	public void SerAtacado (float dano)
	{
		if(!shielded){
			mySummoner.TomarDano(dano);
			ParticlesController.instancia.EmitirParticulas("totemDanificado", 15, transform.position);
		}
	}

	#endregion
}
