
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBelt : MonoBehaviour {

	// Const.
	private const int _TOL_SIDE_SLOT = 8;
	private const float _KEY_DELAY = 0.1f;
	private const int _BELT_MOV_SPD = 10;

	// State.
	private bool _isPause;
	private bool _beltAnimEnd;
	private float _curBeltTranslateXPos;

	// Coordinates.
	private float _zAct;
	private float _yMid;
	private float _xMin;
	private float _xMax;
	private float _mid_xMin;
	// private float _mid_xMax;
	private float _xSlotSize;
	private float[] _xSlot;
	private float _orb_zAct;
	private float _orb_yAct;
	private float _orb_xSlotSize;

	// Timer.
	private float _timer;
	private float _keyTimer;
	private float _spawnInterval;

	// Characters.
	private Character[] _character;
	// HP Bar.
	private HpBar _hpBar;

	// Actions/Moves.
	private GameObject[] _movPreset;
	private GameObject[] _curMov;
	private int[] _curMovType;
	private ActionShooter _movPopup;

	// Selections.
	private int _movBeltSlct;
	private int _movLstSlct;
	private GameObject[] _movSlctor;

	// Use this for initialization
	void Start () {
		_isPause = false;
		_beltAnimEnd = false;
		// Init coordinates.
		Bounds bounds = GetComponent<SpriteRenderer>().bounds;
		_zAct = bounds.min.z - 0.5f;
		_yMid = (bounds.min.y + bounds.max.y) / 2;
		_xMin = bounds.min.x;
		_xMax = bounds.max.x;
		Bounds midBounds = GameObject.Find("wood_banner_with_sword")
			.GetComponent<SpriteRenderer>().bounds;
		_mid_xMin = midBounds.min.x;
		//_mid_xMax = midBounds.max.x;
		_xSlotSize = Mathf.Abs(_mid_xMin - _xMin) / _TOL_SIDE_SLOT;
		_xSlot = new float[_TOL_SIDE_SLOT * 2];
		float curX = _xMin;
		float xOffset = _xSlotSize * 0.5f;
		for (int i = 0; i < (_xSlot.Length / 2); ++i) {
			_xSlot[i] = curX + xOffset;
			curX += _xSlotSize;
		}
		curX = _xMax;
		for (int i = _xSlot.Length - 1; i >= (_xSlot.Length / 2); --i) {
			_xSlot[i] = curX - xOffset;
			curX -= _xSlotSize;
		}
		Bounds orb0Bounds = GameObject.Find("/BattleScene/Bottom/Pools/Pools_Moves/Moves_0")
			.GetComponent<SpriteRenderer>().bounds;
		Bounds orb1Bounds = GameObject.Find("/BattleScene/Bottom/Pools/Pools_Moves/Moves_1")
			.GetComponent<SpriteRenderer>().bounds;
		_orb_zAct = orb0Bounds.min.z - 0.5f;
		_orb_yAct = (orb0Bounds.min.y + orb0Bounds.max.y) / 2;
		_orb_xSlotSize = orb1Bounds.min.x - orb0Bounds.min.x;
		// Init timers.
		_timer = 0;
		_keyTimer = 0;
		_spawnInterval = 1;
		// Init characters.
		// TODO: Get Character Info.
		int enemyLvl = 3;
		switch (enemyLvl) {
			default:
			case 3:
				enemyLvl = 3;
				GameObject.Find("/Tutorial_Screen").GetComponent<ShowTutorial>().showTutorial();
				_spawnInterval = 2;
				break;
			case 4:
				_spawnInterval = 1.5f;
				break;
			case 5:
				_spawnInterval = 1;
				break;
		}
		_character = new Character[2];
		_character[0] = new Character(2);
		_character[1] = new Character(enemyLvl);
		// Init HP bar.
		_hpBar = GameObject.Find("/BattleScene/Top/HpBar").GetComponent<HpBar>();
		_hpBar.Init();
		_hpBar.playerHp = _hpBar.playerMaxHp = _character[0].max_hp;
		_hpBar.enemyHp = _hpBar.enemyMaxHp = _character[1].max_hp;
		_hpBar.RefreshGraphic();
		// Init actions.
		_movPreset = new GameObject[6];
		_movPreset[0] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/NullMov");
		_movPreset[1] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/SwordMov");
		_movPreset[2] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/HammerMov");
		_movPreset[3] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/ShieldMov");
		_movPreset[4] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/ForesightMov");
		_movPreset[5] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/HealingMov");
		_curMov = new GameObject[_xSlot.Length];
		_curMovType = new int[_xSlot.Length];
		for (int i = 0; i < _xSlot.Length; ++i) {
			_curMov[i] = Instantiate(
				_movPreset[0],
				new Vector3(_xSlot[i], _yMid, _zAct),
				Quaternion.identity);
			_curMov[i].transform.localScale = new Vector3(0.3f, 0.3f, 1);
			_curMovType[i] = 0;
		}
		_movPopup = GameObject.Find("wood_banner_with_sword").GetComponent<ActionShooter>();
		// Init slct.
		_movBeltSlct = 4;
		_movLstSlct = 3;
		_movSlctor = new GameObject[2];
		_movSlctor[0] = Instantiate(
			_movPreset[0],
			new Vector3(_xSlot[_movBeltSlct], _yMid, _zAct - 0.5f),
			Quaternion.identity);
		_movSlctor[0].transform.localScale = new Vector3(0.3f, 0.3f, 1);
		_movSlctor[0].GetComponent<SpriteRenderer>().color = new Color(0x7F, 0x7F, 0, 0.5f);
		_movSlctor[1] = Instantiate(
			_movPreset[0],
			new Vector3(0, _orb_yAct, _orb_zAct - 0.5f),
			Quaternion.identity);
		_movSlctor[1].transform.localScale = new Vector3(0.4f, 0.4f, 1);
		_movSlctor[1].GetComponent<SpriteRenderer>().color = new Color(0x7F, 0x7F, 0, 0.5f);
		_curBeltTranslateXPos = _xSlotSize;
	}
	// Update is called once per frame
	void Update () {
		if (! _isPause) {
			_timer += Time.deltaTime;
			_keyTimer += Time.deltaTime;

			// Select action for use as replacement.
			if (Input.GetKey(KeyCode.Alpha1)) {
				_movLstSlct = 1;
				_movSlctor[1].transform.position = new Vector3(
					- 2 * _orb_xSlotSize, _orb_yAct, _orb_zAct);
			} else if (Input.GetKey(KeyCode.Alpha2)) {
				_movLstSlct = 2;
				_movSlctor[1].transform.position = new Vector3(
					- _orb_xSlotSize, _orb_yAct, _orb_zAct);
			} else if (Input.GetKey(KeyCode.Alpha3)) {
				_movLstSlct = 3;
				_movSlctor[1].transform.position = new Vector3(
					0, _orb_yAct, _orb_zAct);
			} else if (Input.GetKey(KeyCode.Alpha4)) {
				_movLstSlct = 4;
				_movSlctor[1].transform.position = new Vector3(
					_orb_xSlotSize, _orb_yAct, _orb_zAct);
			} else if (Input.GetKey(KeyCode.Alpha5)) {
				_movLstSlct = 5;
				_movSlctor[1].transform.position = new Vector3(
					2 * _orb_xSlotSize, _orb_yAct, _orb_zAct);
			}
			// Select action on belt for replacing.
			if (_keyTimer > _KEY_DELAY) {
				if (Input.GetKey(KeyCode.LeftArrow) && (_movBeltSlct > 0)) {
					_movBeltSlct -= 1;
					_movSlctor[0].transform.Translate(new Vector2(- _xSlotSize, 0));
					_keyTimer = 0;
				} else if (Input.GetKey(KeyCode.RightArrow) && (_movBeltSlct < _TOL_SIDE_SLOT - 1)) {
					_movBeltSlct += 1;
					_movSlctor[0].transform.Translate(new Vector2(_xSlotSize, 0));
					_keyTimer = 0;
				}
			}
			// Replace action.
			if ((Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.UpArrow)) &&
					(_curMovType[_movBeltSlct] != 0)) {
				_ReplaceAction(_movBeltSlct, _movLstSlct);
			}

			if ((_timer > _spawnInterval) && (! _beltAnimEnd)) {
				_curBeltTranslateXPos -= _xSlotSize * Time.deltaTime * _BELT_MOV_SPD;
				if (_curBeltTranslateXPos > 0) {
					for (int i = 0; i < _xSlot.Length / 2; ++i) {
						_curMov[i].transform.Translate(
							new Vector2(_xSlotSize, 0) * Time.deltaTime * _BELT_MOV_SPD);
					}
					for (int i = _xSlot.Length / 2; i < _xSlot.Length; ++i) {
						_curMov[i].transform.Translate(
							new Vector2(- _xSlotSize, 0) * Time.deltaTime * _BELT_MOV_SPD);
					}
				} else {
					for (int i = 0; i < (_xSlot.Length / 2) - 1; ++i) {
						_curMov[i].transform.position = new Vector3(_xSlot[i + 1], _yMid, _zAct);
					}
					for (int i = (_xSlot.Length / 2) + 1; i < _xSlot.Length; ++i) {
						_curMov[i].transform.position = new Vector3(_xSlot[i - 1], _yMid, _zAct);
					}
					_curBeltTranslateXPos = _xSlotSize;
					_beltAnimEnd = true;
				}
			}
			// _beltAnimEnd = true;
			// print((_timer > _spawnInterval) + " : " + _beltAnimEnd);
			if (_beltAnimEnd) {
				_MoveBelt();
				_hpBar.RefreshGraphic();
				_beltAnimEnd = false;
			}
		}
	}

	private void _MoveBelt() {
		// if (_timer > _spawnInterval) {
			// Move action sprite.
			// for (int i = 0; i < _xSlot.Length / 2; ++i) {
			// 	_curMov[i].transform.Translate(new Vector2(_xSlotSize, 0));
			// }
			// for (int i = _xSlot.Length / 2; i < _xSlot.Length; ++i) {
			// 	_curMov[i].transform.Translate(new Vector2(- _xSlotSize, 0));
			// }
			// Chk Act.
			_CheckAction(
				_curMovType[(_xSlot.Length / 2) - 1],
				_curMovType[_xSlot.Length / 2]);
			// Move action object.
			GameObject.Destroy(_curMov[(_xSlot.Length / 2) - 1]);
			GameObject.Destroy(_curMov[_xSlot.Length / 2]);
			for (int i = (_xSlot.Length / 2) - 1; i > 0; --i) {
				_curMov[i] = _curMov[i - 1];
				_curMovType[i] = _curMovType[i - 1];
			}
			for (int i = _xSlot.Length / 2; i < _xSlot.Length - 1; ++i) {
				_curMov[i] = _curMov[i + 1];
				_curMovType[i] = _curMovType[i + 1];
			}
			// !!! PLACEHOLDER ACTION ADDING !!!
			_SpawnActionPlayer(Mathf.RoundToInt(Random.Range(0.5f, 3.49f)));
			if (_character[1].hp < (_character[1].max_hp * 0.3333)) {
				if (Random.Range(0, 1) > 0.5) {
					_SpawnActionEnemy(5);
				} else {
					int tmpAct = Mathf.RoundToInt(Random.Range(0.5f, 5.49f));
					_SpawnActionEnemy((tmpAct != 4) ? tmpAct : 5);
				}
			} else {
				_SpawnActionEnemy(Mathf.RoundToInt(Random.Range(0.5f, 3.49f)));
			}
			// Reset timer.
			_timer = 0;
		// }
	}
	private void _SpawnActionPlayer(int type) {
		if (type < 0 || type > _xSlot.Length) type = 0;
		_curMov[0] = Instantiate(
			_movPreset[type],
			new Vector3(_xSlot[0], _yMid, _zAct),
			Quaternion.identity);
		_curMov[0].transform.localScale = new Vector3(0.3f, 0.3f, 1);
		_curMovType[0] = type;
	}
	private void _SpawnActionEnemy(int type) {
		if (type < 0 || type > _xSlot.Length) type = 0;
		_curMov[_xSlot.Length - 1] = Instantiate(
			_movPreset[type],
			new Vector3(_xSlot[_xSlot.Length - 1], _yMid, _zAct),
			Quaternion.identity);
		_curMov[_xSlot.Length - 1].transform.localScale = new Vector3(0.3f, 0.3f, 1);
		_curMovType[_xSlot.Length - 1] = type;
	}

	// Replace acitons.
	private void _ReplaceAction(int slot, int type) {
		Vector3 curPos = _curMov[slot].transform.position;
		GameObject.Destroy(_curMov[slot]);
		_curMov[slot] = Instantiate(
			_movPreset[type],
			curPos, // new Vector3(_xSlot[slot], _yMid, _zAct),
			Quaternion.identity);
		_curMov[slot].transform.localScale = new Vector3(0.3f, 0.3f, 1);
		_curMovType[slot] = type;
	}

	private void _CheckAction(int pType, int eType) {
		_movPopup.ShooterLoad(pType, 0);
		_movPopup.ShooterLoad(eType, 1);

		_movPopup.EffectLoad(0,0);
		_movPopup.EffectLoad(0,1);
		
		bool pActChk = false;
		bool eActChk = false;
		pActChk = ! (pType == 0 || pType == 4 || pType == 5);
		if (! pActChk) switch(pType) {
			case 4: break;
			case 5:
				_character[0].hp += _character[0].heal;
				if (_character[0].hp > _character[0].max_hp)
					_character[0].hp = _character[0].max_hp;
				_movPopup.EffectLoad(5,1);
				break;
		}
		eActChk = ! (eType == 0 || eType == 4 || eType == 5);
		if (! eActChk) switch(eType) {
			case 4: break;
			case 5:
				_character[1].hp += _character[1].heal;
				if (_character[1].hp > _character[1].max_hp)
					_character[1].hp = _character[1].max_hp;
				_movPopup.EffectLoad(5,0);
				break;
		}
		if (pActChk && ! eActChk){ _PlayerAtk(); _movPopup.EffectLoad(pType,0);}
		else if (! pActChk && eActChk){ _EnemyAtk(); _movPopup.EffectLoad(eType,1);} 
		else {
			if (pType == 1 && eType == 2){_PlayerAtk(); _movPopup.EffectLoad(pType,0);}
			else if (pType == 1 && eType == 3){_EnemyAtk(); _movPopup.EffectLoad(eType,1);}
			else if (pType == 2 && eType == 1){_EnemyAtk(); _movPopup.EffectLoad(eType,1);}
			else if (pType == 2 && eType == 3){_PlayerAtk(); _movPopup.EffectLoad(pType,0);}
			else if (pType == 3 && eType == 1){_PlayerAtk(); _movPopup.EffectLoad(pType,0);}
			else if (pType == 3 && eType == 2){_EnemyAtk(); _movPopup.EffectLoad(eType,1);}
		}

		_movPopup.ShooterShoot();

		if (_character[1].hp <= 0) {
			_hpBar.enemyHp = _character[1].hp = 0;
			_isPause = true;
			GameObject.Find("/Result_Screen").GetComponent<ShowResult>().CheckResult(true);
		} else if (_character[0].hp <= 0) {
			_hpBar.playerHp = _character[0].hp = 0;
			_isPause = true;
			GameObject.Find("/Result_Screen").GetComponent<ShowResult>().CheckResult(false);
		}
	}
	private void _PlayerAtk() {
		_character[1].hp -= _character[0].atk;
		_hpBar.enemyHp = _character[1].hp;
	}
	private void _EnemyAtk() {
		_character[0].hp -= _character[1].atk;
		_hpBar.playerHp = _character[0].hp;
	}

	public void Pause() {
		_isPause = true;
	}
	public void Unpause() {
		_isPause = false;
	}

}

