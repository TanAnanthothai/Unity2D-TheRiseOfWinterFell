using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionShooter : MonoBehaviour {

	public Transform LeftShooter;
	public Transform RightShooter;
	private GameObject[] ActionToShoot;
	private GameObject[] shell; //<< To handle ActionToShoot's scaling problem.
	
	private float speed;
	private bool loaded;
	private bool pHeal;
	private bool eHeal;

	public Transform LEffectShooter;
	public Transform REffectShooter;
	public GameObject Player;
	public GameObject Enemy;
	private GameObject[] EfToShoot;
	private GameObject[] EfShooter;
	//private int tester; //<< function tester

	// Use this for initialization
	void Start () {
		ActionToShoot = new GameObject[2]; //0 = left, 1 = right.
		shell = new GameObject[2];
		EfToShoot = new GameObject[3];
		EfShooter = new GameObject[2];
		pHeal = eHeal = false;
		for(int i=0;i<2;i++){
			ActionToShoot[i] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/NullMov");
			EfToShoot[i] = GameObject.Find("/BattleScene/BattleEffect/Blank");
		}
		EfToShoot[2] = GameObject.Find("/BattleScene/BattleEffect/Heal");
		speed = 5f;

		//tester = 2; //<< function tester
	}
	
	// Update is called once per frame
	void Update () {
		// if(Input.GetKey(KeyCode.K) && loaded != true){ /// <<<< fix the shooting trigger here  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		// 	//_ShooterLoad(tester,0); //<< function tester
			
		// 	shell[0] = Instantiate(ActionToShoot[0],LeftShooter.position,LeftShooter.rotation);
		// 	shell[1] = Instantiate(ActionToShoot[1],RightShooter.position,RightShooter.rotation);
		// 	for(int i=0;i<2;i++){
		// 	shell[i].transform.localScale = new Vector3(0.4f,0.4f);
		// 	}		
		// 	loaded = true;
		// }

		// if(loaded == true){
		// 	shell[0].transform.Translate(new Vector3(-0.1f,1) * Time.deltaTime* speed);
		// 	shell[1].transform.Translate(new Vector3(0.1f,1) * Time.deltaTime* speed);
		// 	Vector3 shotPos = Camera.main.WorldToScreenPoint(shell[0].transform.position);
		// 	if(shotPos.y >= Screen.height/1.5){
		// 		for(int i=0;i<2;i++){
		// 			Destroy(shell[i]);
		// 		}
		// 		loaded = false;
		// 	}
		// }
		if(loaded == true){
			shell[0].transform.Translate(new Vector3(-0.1f,1) * Time.deltaTime* speed);
			shell[1].transform.Translate(new Vector3(0.1f,1) * Time.deltaTime* speed);
			Vector3 shotPos = Camera.main.WorldToScreenPoint(shell[0].transform.position);
			if(shotPos.y >= Screen.height/1.5){
				for(int i=0;i<2;i++){
					Destroy(shell[i]);
					Destroy(EfShooter[i]);
				}
				loaded = false;
				pHeal = eHeal = false;
			}
		}
	}

	public void ShooterLoad(int type, int side){  // <<<<<<<<<<<<< change what to shoot
		switch(type){
			case 0: ActionToShoot[side] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/NullMov");break;
			case 1: ActionToShoot[side] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/SwordMov");break;
			case 2: ActionToShoot[side] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/HammerMov");break;
			case 3: ActionToShoot[side] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/ShieldMov");break;
			case 4: ActionToShoot[side] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/ForesightMov");break;
			case 5: ActionToShoot[side] = GameObject.Find("/BattleScene/Bottom/Action_Line/Preset/HealingMov");break;
		}
	}
	public void ShooterShoot() {
		if(loaded != true){ /// <<<< fix the shooting trigger here  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			//_ShooterLoad(tester,0); //<< function tester
			
			shell[0] = Instantiate(ActionToShoot[0],LeftShooter.position,LeftShooter.rotation);
			shell[1] = Instantiate(ActionToShoot[1],RightShooter.position,RightShooter.rotation);
			for(int i=0;i<2;i++){
				shell[i].transform.localScale = new Vector3(0.4f,0.4f);
			}

			EfShooter[0] = Instantiate(EfToShoot[0],REffectShooter.transform.position,REffectShooter.rotation);
			EfShooter[1] = Instantiate(EfToShoot[1],LEffectShooter.transform.position,LEffectShooter.rotation);
			EfShooter[0].transform.localScale = new Vector3(-1f,1f);

			if(pHeal == true && eHeal!=true){
				EfShooter[0] = Instantiate(EfToShoot[2],Enemy.transform.position,Enemy.transform.rotation);
				EfShooter[0].transform.position = new Vector3(EfShooter[0].transform.position.x,EfShooter[0].transform.position.y+1,Enemy.transform.position.z-0.01f);
				EfShooter[0].transform.localScale = new Vector3(1.5f,1.5f);
			}else if(eHeal == true && pHeal != true){
				EfShooter[0] = Instantiate(EfToShoot[2],Player.transform.position,Player.transform.rotation);
				EfShooter[0].transform.position = new Vector3(EfShooter[0].transform.position.x,EfShooter[0].transform.position.y+1,Player.transform.position.z-0.01f);
				EfShooter[0].transform.localScale = new Vector3(1.5f,1.5f);
			}else;
			loaded = true;
		}
	}

	public void EffectLoad(int type, int side){
		switch(type){
			case 0: EfToShoot[side] = GameObject.Find("/BattleScene/BattleEffect/Blank");break;
			case 1: EfToShoot[side] = GameObject.Find("/BattleScene/BattleEffect/Slash");break;
			case 2: EfToShoot[side] = GameObject.Find("/BattleScene/BattleEffect/Blunt_Spark");break;
			case 3: EfToShoot[side] = GameObject.Find("/BattleScene/BattleEffect/Bash");break;
			case 4: EfToShoot[side] = GameObject.Find("/BattleScene/BattleEffect/Blank");break;
			case 5: EfToShoot[side] = GameObject.Find("/BattleScene/BattleEffect/Heal");
				if(side==0) pHeal = true;
				else eHeal = true;
				break;
		}
	}

}
