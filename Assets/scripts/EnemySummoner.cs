using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

/// <summary>
/// Enemy summoner. o operador do dispositivo movel.
/// representado pelo totem
/// </summary>
public class EnemySummoner : MonoBehaviour {

	public GameObject[] enemyPrefabs;

	public float maxSpawnDistFromCenter = 11;

	public float minAcceptableDistFromPlayer = 3;

	public Transform oJogador;

	public Transform totemTransform;

	public float vida = 400;

	private float vidaInicial;

	public float vidaCritica = 20;

	private bool jaAtivouChacoalhada = false;

	private int minionsAtivos = 0;

	public float fatorPerdaVidaQuandoDesprotegido = 1;

	public LifeBar totemLifeBar;

	private int jogadoresConectados = 0;

	public bool autoSpawnEnemies = false;

	public Image totemPlayerImg;

	void Start(){
		vidaInicial = vida;
		Invoke("GrunhidosDosCara", 5.0f);

	}

	public void InitAutoSpawn(){
		Invoke("SpawnEnemy", 2.0f);
	}

	void Update(){
		if(GameManager.instancia.estadoAtual == GameManager.estadoPartida.emJogo){
			if(minionsAtivos == 0){
				vida -= Time.deltaTime * fatorPerdaVidaQuandoDesprotegido;

				if(!jaAtivouChacoalhada){
					ChecaSeEstaCritico();
				}
				
				if(vida <= 0){
					ParticlesController.instancia.EmitirParticulas("totemDeath", 40, totemTransform.position);
					Destroy(totemTransform.gameObject);
					GameManager.instancia.SlowEndGame(false);
				}
			}else{
				vida += Time.deltaTime;
			}

			totemLifeBar.UpdateMyValue(1 - ((vidaInicial - vida) / vidaInicial), false);
		}
	}

	public void TomarDano(float dano){
		vida -= dano;
		totemLifeBar.UpdateMyValue(1 - ((vidaInicial - vida) / vidaInicial));

		if(vida <= 0){
			ParticlesController.instancia.EmitirParticulas("totemDeath", 40, totemTransform.position);
			SoundMan.instancia.soundsDict["Explosion"].volume = 1f;
			SoundMan.instancia.EmitirSom("Explosion", 0.2f, totemTransform.position);
			Destroy(totemTransform.gameObject);
			minionsAtivos = 0;
			GameManager.instancia.StopTheBattle();
			GameManager.instancia.SlowEndGame(false);
		}else{
			SoundMan.instancia.EmitirSom("Toten Damage " + Random.Range(1, 4), 0.2f, totemTransform.position);

			if(!jaAtivouChacoalhada){
				ChecaSeEstaCritico();
			}
		}
	}
	
	public void JogadorNovo(){
		jogadoresConectados++;
		if(jogadoresConectados == 1){
			GameManager.instancia.IniciarPartida();
			totemPlayerImg.enabled = true;
		}else{
			Debug.Log("Jogador novo! fica fortao ae jogador");
			oJogador.GetComponent<JogadorController>().ToughenUp();
			Image newSymbol = Instantiate(totemPlayerImg);
			newSymbol.transform.SetParent(totemPlayerImg.transform.parent, false);
		}

	}

	public void instanciar(string numeroDoMinion){
		int indiceDoMinion = -1;
		try{
			indiceDoMinion = int.Parse(numeroDoMinion);
		}
		catch{
			return;
		}
		if(indiceDoMinion == 4){
			if(oJogador){
				oJogador.GetComponent<JogadorController>().ApplyDebuff();
			}
		}else{
			SpawnEnemy(indiceDoMinion);
		}
	}

	/// <summary>
	/// Spawns the enemy. versao para invoke
	/// </summary>
	void SpawnEnemy(){
		SpawnEnemy(-1);
	}

	void SpawnEnemy(int indiceDoMinion = -1){
		if(oJogador){
			//spawn numa pos aleatoria
			Vector2 spawnPos = oJogador.position;
			while(Vector2.Distance(spawnPos, oJogador.position) < minAcceptableDistFromPlayer){
				spawnPos = new Vector2(Random.Range(-maxSpawnDistFromCenter, maxSpawnDistFromCenter),
				                       Random.Range(-maxSpawnDistFromCenter, maxSpawnDistFromCenter));
			}
			GameObject spawnedEnemy = null;
			if(indiceDoMinion == -1){
				spawnedEnemy = 
					(GameObject) Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], 
					                         spawnPos, Random.rotation);
			}else{
				spawnedEnemy = 
					(GameObject) Instantiate(enemyPrefabs[indiceDoMinion], 
					                         spawnPos, Random.rotation);
			}
			InimigoController enemyScript = spawnedEnemy.GetComponent<InimigoController>();

			enemyScript.mySummoner = this;

			if(enemyScript.GetType() == typeof(MagoProtetorController)){
				MagoProtetorController magoScript = ((MagoProtetorController) enemyScript);
				magoScript.oTotem = totemTransform.GetComponent<TotemScript>();
				magoScript.fleeFromTransform = oJogador;
				magoScript.myTarget = totemTransform;
			}else{
				enemyScript.myTarget = oJogador;
			}
			ParticlesController.instancia.EmitirParticulas("spawnEffect", 20, spawnPos);
			SoundMan.instancia.EmitirSom("spawnz", 0.2f, spawnPos);
			if(autoSpawnEnemies){
				Invoke("SpawnEnemy", Random.Range(0.8f, 7.0f));
			}

			minionsAtivos++;
		}else{
			this.enabled = false;
		}
	}

	public void EnemyDown(){
		minionsAtivos--;
	}

	void GrunhidosDosCara(){
		if(minionsAtivos > 0){
			SoundMan.instancia.EmitirSom("Monster " + Random.Range(1, 5).ToString(), 0.2f);
		}

		Invoke("GrunhidosDosCara", Random.Range(2.5f, 8.5f));
	}

	public void AtivarMomentoCritico(){
		//avisa o mobile
		RequestPC.shakeDatAss();
		jaAtivouChacoalhada = true;
		ParticlesController.instancia.EmitirParticulas("totemDesperation", 20, totemTransform.position);
		SoundMan.instancia.soundsDict["Explosion"].volume = 0.4f;
		SoundMan.instancia.EmitirSom("Explosion", 0.2f);
	}

	public void ChecaSeEstaCritico(){
		if(vida <= vidaCritica){
			AtivarMomentoCritico();
		}
	}


}
