using UnityEngine;
using System.Collections;

public class RequestPC : MonoBehaviour {
	
	static bool shake;
	static bool endGame;
	static bool killServer;
	private int jogadoresConectados;
	private string alvo;
	public GameObject instanciador;

	void Start () {
		endGame = false;
		killServer = false;
		instanciador = GameObject.Find("enemySummoner");
		StartCoroutine(requestInfo ());
	}
		
	public static void terminarJogo(){
		endGame = true;
	}

	public static void shakeDatAss(){
		shake = true;
	}

	public static void killDatServer(){
		Debug.Log("mandou killar");
		killServer = true;
	}

	IEnumerator requestInfo(){
		while (true) {
			alvo="pc";
			if(shake){
				shake = false;
				alvo = "shake";
			}
			if(endGame){
				alvo="endGame";

			}
			if(killServer){
				alvo="kill";
			}

			//receber 
			yield return new WaitForSeconds (0.35f);
			WWWForm form = new WWWForm();
			form.AddField("Jogador","PC");
			WWW www = new WWW ("localhost:46370/"+alvo,form); //server
			yield return www;

			//tratamento do retorno
			string reciever = www.text;
			if(reciever!=""){
				switch(reciever)
				{
				case "Fabii":
					Debug.Log("Acabou");
					//acabou o jogo
					break;
				
				default:
					string[] c = reciever.Split(new char[]{','});
					foreach(string s in c){
						if(s.StartsWith("*")){
							Debug.Log("Jogadores Online "+s.Replace("*",""));
							int  jogsAux =   int.Parse(s.Replace("*",""));
							if(jogadoresConectados < jogsAux){
								instanciador.GetComponent<EnemySummoner>().JogadorNovo();
								jogadoresConectados = jogsAux;
							}
						}
						else{
							if(s !="" || s!="endGame" || s !="mexecacoisa"){
								Debug.Log(s);
								instanciador.GetComponent<EnemySummoner>().instanciar(s);
							}
						}
					}
					break;
				}
			}

		}
	}

}
