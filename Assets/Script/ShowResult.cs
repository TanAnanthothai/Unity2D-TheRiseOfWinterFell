using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowResult : MonoBehaviour {

	private GameObject Result_Screen;
	private GameObject[] Result;
	private GameObject dim_screen;
	private GameObject[] Texts;
	//private bool tester;

	// Use this for initialization
	void Start () {
		Result = new GameObject[2];
		Texts = new GameObject[2];
		Result_Screen = GameObject.Find("/Result_Screen");
		Result[0] = GameObject.Find("/Result_Screen/Text_box/Text_box_victory");
		Result[1] = GameObject.Find("/Result_Screen/Text_box/Text_box_defeated");
		dim_screen = GameObject.Find("/Result_Screen/Dim_Screen");		
		Texts[0] = GameObject.Find("/Result_Screen/Result_Plate");
		Texts[1] = GameObject.Find("/Result_Screen/Text");
		for(int i=0;i<2;i++){
			Result[i].SetActive(false);
		}
		//tester = true;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetKey(KeyCode.P)){
			//Result_Screen = Instantiate(dim_screen,new Vector3(0,0),Quaternion.identity);
			Result[0].SetActive(true);
			Result[1].SetActive(false);
		}
		if(Input.GetKey(KeyCode.E)){
			//Destroy(Result_Screen);
			Result[0].SetActive(false);
			Result[1].SetActive(true);
		}*/
	}

	public void CheckResult(bool win){
		// if (win){
		// 	Result[0].SetActive(true);
		// 	Result[1].SetActive(false);
		// } else {
		// 	Result[0].SetActive(false);
		// 	Result[1].SetActive(true);
		// }
		Result[0].SetActive(win);
		Result[1].SetActive(! win);
		Result_Screen.transform.Translate(new Vector3(0, 0, -5));
	}

}
