
using UnityEngine;

struct Character {
	public GameObject obj;
	public int max_hp;
	public int hp;
	public int atk;
	public int heal;

	public Character(int id) {
		obj = GameObject.Find((id < 3) ?
			"/BattleScene/Mid/Player" : "/BattleScene/Mid/Enemy");
		max_hp = hp = atk = heal = 0;
		if (id < 0 || id > 5) id = 0;
		if (id == 0) {
			obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("branhodor_static");
			max_hp = hp = 180;
			atk = 5;
			heal = 8;
		} else if (id == 1) {
			obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Jon_Snow");
			max_hp = hp = 140;
			atk = 8;
			heal = 7;
		} else if (id == 2) {
			obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Daenerys");
			max_hp = hp = 110;
			atk = 10;
			heal = 10;
		} else if (id == 3) {
			obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Khal_Drogo");
			max_hp = hp = 90;
			atk = 6;
			heal = 3;
		} else if (id == 4) {
			obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("The_Mountain");
			max_hp = hp = 150;
			atk = 8;
			heal = 1;
		} else if (id == 5) {
			obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("White_Walker");
			max_hp = hp = 130;
			atk = 6;
			heal = 10;
		}
	}
	
	public void destroy() {
		GameObject.Destroy(obj);
	}
}

