
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour {

	// Text fields.
	private TextMesh _playerTxt;
	private TextMesh _enemyTxt;
	// Bar sprite transformer.
	private Transform _playerBar;
	private Transform _enemyBar;
	// Default x-coord.
	private float _playerDefaultXScale;
	// private float _playerDefaultXPos;
	private float _enemyDefaultXScale;
	// private float _enemyDefaultXPos;
	// HP val.
	public int playerMaxHp;
	public int enemyMaxHp;
	public int playerHp;
	public int enemyHp;

	// Use this for initialization
	void Start () {
		//
	}

	// Update is called once per frame
	void Update () {
		//
	}

	// Custom init.
	public void Init() {
		// Init txt field.
		_playerTxt = GameObject.Find("/BattleScene/Top/HpBar/PlayerHp/TxtBase/HpNum")
			.GetComponent<TextMesh>();
		_enemyTxt = GameObject.Find("/BattleScene/Top/HpBar/EnemyHp/TxtBase/HpNum")
			.GetComponent<TextMesh>();
		// Init bar sprite.
		_playerBar = GameObject.Find("/BattleScene/Top/HpBar/PlayerHp/Health")
			.transform;
		_enemyBar = GameObject.Find("/BattleScene/Top/HpBar/EnemyHp/Health")
			.transform;
		// Init default x-ccord.
		_playerDefaultXScale = _playerBar.localScale.x;
		// _playerDefaultXPos = _playerBar.position.x;
		_enemyDefaultXScale = _enemyBar.localScale.x;
		// _enemyDefaultXPos = _enemyBar.position.x;
		// Init HP val.
		playerMaxHp = 1;
		enemyMaxHp = 1;
		playerHp = 1;
		enemyHp = 1;
	}

	// Refresh graphic.
	public void RefreshGraphic() {
		_playerTxt.text = "" + playerHp;
		_enemyTxt.text = "" + enemyHp;
		Vector3 tmpVec = _playerBar.localScale;
		tmpVec.x = _playerDefaultXScale * ((float) playerHp / playerMaxHp);
		_playerBar.localScale = tmpVec;
		// tmpVec = _playerBar.position;
		// tmpVec.x = _playerDefaultXPos -
		// 	(_playerDefaultXPos * (1 - ((float) playerHp / playerMaxHp)));
		//_playerBar.position = tmpVec;
		tmpVec = _enemyBar.localScale;
		tmpVec.x = _enemyDefaultXScale * ((float) enemyHp / enemyMaxHp);
		_enemyBar.localScale = tmpVec;
		// tmpVec = _enemyBar.position;
		// tmpVec.x = _enemyDefaultXPos - 
		// 	(_enemyDefaultXPos * (1 - ((float) playerHp / playerMaxHp)));
		//_enemyBar.position = tmpVec;
	}

}

