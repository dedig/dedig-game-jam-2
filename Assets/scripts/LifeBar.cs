using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

/// <summary>
/// Life bar. controla uma barra de vida, a va
/// hahaha entao, ela encolhe quando vc perde vida e tals...
/// eh isso ai
/// a ideia eh fazer aqueles efeitin de quando... vc perde vida
/// </summary>
public class LifeBar : MonoBehaviour {

	private float lastValue = 1;

	public Image shrinkingLifeBar;
	public Image lifeLossEffect;

	public void UpdateMyValue(float newValue, bool flashLostLife = true){

		shrinkingLifeBar.fillAmount = newValue;
		lifeLossEffect.fillAmount = lastValue;
		lifeLossEffect.color = Color.yellow;
		lifeLossEffect.DOFade(0, 0.4f);
		lastValue = newValue;
	}
}
