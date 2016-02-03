using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	public static GameManager instancia;

	public GameObject[] weaponPickups;

	public Text textoFinalText, scoreText, IPText;

	public CanvasGroup finalCanvasGroup;

	public int currentScore = 0;

	public GameObject theServerPrefab;
	
	private GameObject meuServer;

	public GameObject oJogoObject, menuObject;

	public CanvasGroup waiting4PlayersGroup;

	public bool fakePlayer = false;

	void Awake(){
		instancia = this;
	}

	void Start(){
		IPText.text = Network.player.ipAddress.ToString();
		RequestPC.killDatServer();
	}

	public enum estadoPartida{
		noMenu,
		emJogo,
		encerrada
	}

	public estadoPartida estadoAtual = estadoPartida.noMenu;

	public void SlowEndGame(bool invocadorGanhou){
		estadoAtual = estadoPartida.encerrada;
		RequestPC.terminarJogo();
		Time.timeScale = 0.2f;
		Camera.main.transform.DOShakePosition(0.8f, 2);
		StartCoroutine(EncerrarPartida(invocadorGanhou));
	}


	public IEnumerator EncerrarPartida(bool invocadorGanhou){
		yield return new WaitForSeconds(0.35f);
		Time.timeScale = 1;

		SoundMan.instancia.EmitirSom("End Game");

		finalCanvasGroup.DOFade(1, 1.0f);
		finalCanvasGroup.interactable = true;

		if(invocadorGanhou){
			textoFinalText.transform.parent.GetComponent<Image>().color = new Color(1, 0, 0, 0.7f);
			textoFinalText.text = "Defeat";
			//avisa mobile que ganhou
		}else{
			textoFinalText.transform.parent.GetComponent<Image>().color = new Color(0, 1, 1, 0.7f);
			textoFinalText.color = Color.black;
			textoFinalText.text = "Victory";
			//avisa mobile que perdeu
		}
	}

	public void UpdateScore(int addition){
		currentScore += addition;
		scoreText.text = currentScore.ToString();
	}

	public void PickupHasBeenChosen(){
		SoundMan.instancia.EmitirSom("Pegando arma", 0.2f);
		for(int i = 0; i < weaponPickups.Length; i++){
			ParticlesController.instancia.EmitirParticulas
				("pickupDisappear", 15, weaponPickups[i].transform.position);
			Destroy(weaponPickups[i]);
		}
	}

	public void ComecarServidor(){
		if(!meuServer){
			meuServer = (GameObject) Instantiate(theServerPrefab);
			waiting4PlayersGroup.DOFade(1, 0.3f);
			waiting4PlayersGroup.blocksRaycasts = true;

			if(fakePlayer){
				GameObject.Find("enemySummoner").GetComponent<EnemySummoner>().JogadorNovo();
			}

		}else{
			Destroy(meuServer);
		}
	}

	public void ComecarSinglePlayer(){
		EnemySummoner summonerScript = GameObject.Find("enemySummoner").GetComponent<EnemySummoner>();
		summonerScript.autoSpawnEnemies = true;
		IniciarPartida();
		summonerScript.InitAutoSpawn();
	}
	
	public void IniciarPartida(){
		estadoAtual = estadoPartida.emJogo;
		oJogoObject.SetActive(true);
		menuObject.SetActive(false);
	}

	public void RestartMatch(){
		RequestPC.killDatServer();
		Application.LoadLevel(Application.loadedLevel);
	}

	public void QuitGame(){
		Application.Quit();
	}

	public void StopTheBattle(){
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
			if(enemy.name == "oTotem"){
				ParticlesController.instancia.EmitirParticulas("totemDesperation", 40, enemy.transform.position);
			}
			Destroy(enemy);
			ParticlesController.instancia.EmitirParticulas("spawnEffect", 15, enemy.transform.position);
		}

		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
			JogadorController jogaScript = player.GetComponent<JogadorController>();
			jogaScript.enabled = false;
			jogaScript.velocJogador = 0;
		}
	}
}
