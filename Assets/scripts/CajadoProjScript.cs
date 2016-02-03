using UnityEngine;
using System.Collections;

public class CajadoProjScript : MonoBehaviour {

	public float dano, velocidadeProj;

	public JogadorController oJogador;

	public void StartMoving(){
		GetComponent<Rigidbody2D>().velocity = transform.up * velocidadeProj;
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.tag == "Enemy"){
			c.GetComponent<IAttackable>().SerAtacado(dano);
			ParticlesController.instancia.EmitirParticulas("cajadoProjExplode", 15, transform.position);
			oJogador.DespawnProjectile(gameObject);
			SoundMan.instancia.EmitirSom("Magic Impact", 0.2f, transform.position);
		}else if(c.tag == "GameController"){
			ParticlesController.instancia.EmitirParticulas("cajadoProjExplode", 15, transform.position);
			oJogador.DespawnProjectile(gameObject);
		}
	}

}
