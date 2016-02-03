using UnityEngine;
using System.Collections;

public class SoldadoAtacanteController : InimigoController, IAttacker {

	public float attackDamage, timeBetweenAtks;

	protected float timeSinceLastAtk;

	public Collider2D strikeTrigger;

	public string myAtkSound;
	
	public int numAtkSndVariations;

	// Use this for initialization
	new void Start () {
		base.Start();
		attackDamage = Random.Range(attackDamage / 1.3f, attackDamage * 1.3f);
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastAtk += Time.deltaTime;

		if(myTarget){
			//vamos olhar pro alvo e chegar perto o suficiente para atacar
			Vector3 deltaPos = myTarget.position - transform.position;
			
			transform.rotation = 
				Quaternion.Euler(Vector3.forward * (Mathf.Rad2Deg * 
				                                    Mathf.Atan2(deltaPos.y, deltaPos.x) - 90));
			
			if(Vector2.Distance(myTarget.position, transform.position) <= attackRange){
				if(timeSinceLastAtk >= timeBetweenAtks){
					timeSinceLastAtk = 0;
					inimigoAnim.Play("attack", 0, 0); //taca le pau
				}
				
				myRigid.velocity = Vector2.Lerp(myRigid.velocity, Vector2.zero, Time.deltaTime * moveSpeed);
				inimigoAnim.SetBool("andando", false);
			}else{
				myRigid.velocity = transform.up * moveSpeed;
				inimigoAnim.SetBool("andando", true);
			}
		}
	}

	public void BeginAttackWindow(){
		SoundMan.instancia.EmitirSom("Golpe no Ar", 0.2f, transform.position);
		strikeTrigger.enabled = true;
	}

	public void EndAttackWindow(){
		strikeTrigger.enabled = false;
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.tag == "Player"){
			c.GetComponent<IAttackable>().SerAtacado(attackDamage);
			string atkSndName = myAtkSound;
			if(numAtkSndVariations > 1){
				atkSndName += Random.Range(1, numAtkSndVariations + 1).ToString();
			}
			SoundMan.instancia.EmitirSom(atkSndName, 0.2f, c.transform.position);
		}
	}
}
