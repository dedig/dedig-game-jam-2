using UnityEngine;
using System.Collections;

public class InimigoController : MonoBehaviour, IAttackable {

	public float attackRange;

	public float maxVida;

	protected float curVida;

	public float moveSpeed;

	public Transform myTarget;

	public Animator inimigoAnim;

	protected Rigidbody2D myRigid;

	public EnemySummoner mySummoner;

	public int scoreAward;



	// Use this for initialization
	protected void Start () {
		myRigid = GetComponent<Rigidbody2D>();
		curVida = Random.Range(maxVida / 1.2f, maxVida * 1.5f);
		moveSpeed = Random.Range(moveSpeed / 1.5f, moveSpeed * 1.5f);

	}

	#region IAttackable implementation

	public virtual void SerAtacado (float dano)
	{
		curVida -= dano;
		ParticlesController.instancia.EmitirParticulas("sangre", (int) dano * 2, transform.position);


		if(curVida <= 0){
			//ooooou
			ParticlesController.instancia.EmitirParticulas("minionDeath", (int) maxVida / 2, transform.position);
			SoundMan.instancia.EmitirSom("Damage " + Random.Range(1, 4).ToString(), 0.2f, transform.position);
			mySummoner.EnemyDown();
			GameManager.instancia.UpdateScore(scoreAward);
			Destroy(gameObject);
		}
	}

	#endregion
	
}
