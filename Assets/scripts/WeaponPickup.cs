using UnityEngine;
using System.Collections;
/// <summary>
/// uma arma que, quando escolhida, e imediatamente equipada pelo jogador, e as outras armas desaparecem
/// </summary>
public class WeaponPickup : MonoBehaviour {

	public string nomeDoBoolDaArma;
	public int numVariacoesDeSomAtk;
	public float danoAtk;
	public Vector2 strikeTriggerOffset;
	public float strikeTriggerRadius;

	public Vector2 trailPos;

	void OnTriggerEnter2D(Collider2D c){
		if(c.tag == "Player"){
			JogadorController jogaScript = c.GetComponent<JogadorController>();
			jogaScript.jogaAttackAnim.SetBool(nomeDoBoolDaArma, true);
			jogaScript.danoAtk = danoAtk;
			jogaScript.strikeTrigger.transform.localPosition = strikeTriggerOffset;
			jogaScript.myAtkSound = nomeDoBoolDaArma;
			jogaScript.numAtkSndVariations = numVariacoesDeSomAtk;
			if(strikeTriggerRadius == 0){
				jogaScript.strikeTrigger = null;
			}else{
				((CircleCollider2D) jogaScript.strikeTrigger).radius = strikeTriggerRadius;
			}

			jogaScript.weaponSprite.sprite = GetComponent<SpriteRenderer>().sprite;

			jogaScript.heavyTrail.transform.localPosition = trailPos;

			GameManager.instancia.PickupHasBeenChosen();
		}
	}
}
