using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorial : MonoBehaviour {

 private GameObject Tutorial_Screen;
 private GameObject Tutorial_Plate;
 private GameObject dim_screen;
 private GameObject PressToContinue;
 //private bool tester;

 // Use this for initialization
 void Start () {
  Tutorial_Screen = GameObject.Find("/Tutorial_Screen");
  Tutorial_Plate = GameObject.Find("/Tutorial_Screen/Tutorial_Plate");
  dim_screen = GameObject.Find("/Tutorial_Screen/Dim_Screen");  
  PressToContinue = GameObject.Find("/Tutorial_Screen/PressToContinue");
 }

 void Update() {
  if (Input.GetKeyDown(KeyCode.Space)) {
   print("Enter space");
   Tutorial_Screen.transform.Translate (new Vector3 (0, 0, 5));
   GameObject.Find("/BattleScene/Bottom/Belt").GetComponent<ActionBelt>().Unpause();
  }
 }

 public void showTutorial(){
  Tutorial_Screen.transform.Translate(new Vector3(0, 0, -5));
 }
}
