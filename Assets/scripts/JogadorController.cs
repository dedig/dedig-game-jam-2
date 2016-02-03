using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

/// <summary>
/// Jogador movement. recebe o input de teclas (ou de um controle, esperamos) e faz o jogador se mover
/// </summary>
public class JogadorController : MonoBehaviour, IAttackable, IAttacker {

	private Rigidbody2D jogaRigid;

	public float velocJogador;

	private Vector2 vetorQueDefineOOlhar;

	public Animator jogaAttackAnim;

	public float danoAtk;

	public Collider2D strikeTrigger;

	public float vida;

	private float vidaInicial;

	public LifeBar myLifeBar;

	public Transform pesTransform;

	public SpriteRenderer weaponSprite;

	public GameObject projectilePrefab;

	private List<GameObject> pooledProjs;

	public TrailRenderer heavyTrail, tiredTrail;

	private bool isDebuffed = false;

	public string myAtkSound;

	public int numAtkSndVariations;

	// Use this for initialization
	void Start () {
		jogaRigid = GetComponent<Rigidbody2D>();
		vidaInicial = vida;

		pooledProjs = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 movementVector = new Vector2(Input.GetAxisRaw("HorizontalMovement"), Input.GetAxisRaw("VerticalMovement")).normalized;

		jogaRigid.velocity = movementVector * velocJogador;

		jogaAttackAnim.SetBool("andando", movementVector != Vector2.zero);

		Vector2 attackVector = new Vector2(Input.GetAxisRaw("HorizontalAttack"), Input.GetAxisRaw("VerticalAttack"));

		if(attackVector != Vector2.zero){
			vetorQueDefineOOlhar = attackVector;
			jogaAttackAnim.SetBool("atacando", true);
		}else{
			vetorQueDefineOOlhar = movementVector;
			jogaAttackAnim.SetBool("atacando", false);
		}


		UpdateTransformRotationProcedure(transform, vetorQueDefineOOlhar);

		UpdateTransformRotationProcedure(pesTransform, movementVector);

	}

	public void UpdateTransformRotationProcedure(Transform target, Vector2 referenceVector){
		//uns ifs daora pra decidir pra onde rodar e atacar
		if(referenceVector.x > 0){
			if(referenceVector.y > 0){
				target.rotation = Quaternion.Euler(Vector3.forward * -45);
			}else if(referenceVector.y < 0){
				target.rotation = Quaternion.Euler(Vector3.forward * -135);
			}else{
				target.rotation = Quaternion.Euler(Vector3.forward * -90);
			}
		}else if(referenceVector.x < 0){
			if(referenceVector.y > 0){
				target.rotation = Quaternion.Euler(Vector3.forward * 45);
			}else if(referenceVector.y < 0){
				target.rotation = Quaternion.Euler(Vector3.forward * 135);
			}else{
				target.rotation = Quaternion.Euler(Vector3.forward * 90);
			}
		}else{
			if(referenceVector.y > 0){
				target.rotation = Quaternion.Euler(Vector3.zero);
			}else if(referenceVector.y < 0){
				target.rotation = Quaternion.Euler(Vector3.forward * 180);
			}
		}
	}

	public void ApplyDebuff(){
		if(!isDebuffed){
			tiredTrail.enabled = true;
			velocJogador /= 3;
			isDebuffed = true;
			StartCoroutine(RestoreSpeedAfterTime());
		}
	}

	public void ToughenUp(){
		vida *= 1.5f;
		vidaInicial *= 1.5f;
		danoAtk *= 1.5f;
	}

	IEnumerator RestoreSpeedAfterTime(){
		yield return new WaitForSeconds(3);
		if(isDebuffed){
			tiredTrail.enabled = false;
			isDebuffed = false;
			velocJogador *= 3;
		}
	}

	public void SpawnProjectile(){
		GameObject pickedProj = null;
		if(pooledProjs.Count > 0){
			pickedProj = pooledProjs[0];
			pooledProjs.Remove(pickedProj);
			pickedProj.SetActive(true);

		}else{
			pickedProj = (GameObject) Instantiate(projectilePrefab);
		}

		pickedProj.transform.position = transform.position;
		pickedProj.transform.rotation = transform.rotation;
		CajadoProjScript projScript = pickedProj.GetComponent<CajadoProjScript>();
		projScript.oJogador = this;
		projScript.StartMoving();


	}

	public void DespawnProjectile(GameObject theProj){
		pooledProjs.Add(theProj);
		theProj.SetActive(false);
	}
	
	#region IAttackable implementation

	public void SerAtacado (float dano)
	{
		vida -= dano;
		myLifeBar.UpdateMyValue(1 - ((vidaInicial - vida) / vidaInicial));
		if(vida <= 0){
			SoundMan.instancia.EmitirSom("Damage " + Random.Range(1, 4).ToString(), 0.2f, transform.position);
			ParticlesController.instancia.EmitirParticulas("playerDies", (int) dano * 2, transform.position);
			Destroy(gameObject);
			GameManager.instancia.StopTheBattle();
			GameManager.instancia.SlowEndGame(true);
		}else{
			Camera.main.transform.DOShakePosition(0.2f);
			ParticlesController.instancia.EmitirParticulas("sangre", (int) dano * 2, transform.position);
		}
	}

	#endregion

	#region IAttacker implementation

	public void BeginAttackWindow ()
	{
		SoundMan.instancia.EmitirSom("Golpe no Ar", 0.2f, transform.position);
		if(strikeTrigger){
			strikeTrigger.enabled = true;
			heavyTrail.enabled = true;

		}else{
			SpawnProjectile();
		}

		//sound man play sound myAtkSound
	}

	public void EndAttackWindow ()
	{
		if(strikeTrigger){
			strikeTrigger.enabled = false;
			heavyTrail.enabled = false;
		}
	}

	#endregion

	void OnTriggerEnter2D(Collider2D c){
		if(c.tag == "Enemy"){
			c.GetComponent<IAttackable>().SerAtacado(danoAtk);
			string atkSndName = myAtkSound;
			if(numAtkSndVariations > 1){
				atkSndName += Random.Range(1, numAtkSndVariations + 1).ToString();
			}
			SoundMan.instancia.EmitirSom(atkSndName, 0.2f, c.transform.position);
		}
	}
}
