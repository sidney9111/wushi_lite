using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEngine.EventSystems;
using System;
//using System.ComponentModel;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.EventSystems;
//namespace UnityEngine
//{
/// <summary>
/// 需要在Resources目录下存在btn_blue等几个图片
/// </summary>
public class JoystickRadio : MonoBehaviour,IPointerDownHandler {
	[Serializable]
	public class Point {
		public Color color;
		public Vector3 offset;
	}
	public String keySrc;
	//public int frequency = 1;
	//public String bbb;
	private bool _isPressed;
	public bool isPressed{
		get{ return _isPressed;}
		set{ _isPressed = value;}
	}
	public event EventHandler<RadioEventArgs> keyHandler;
	//	public string[] toolbarTexts = {"Align", "Copy", "Randomize", "Add noise"};
	private string privatestatus;

	//	[WrapperlessIcall]
	//	[MethodImpl (MethodImplOptions.InternalCall)]
	//	get;
	//	[WrapperlessIcall]
	//	[MethodImpl (MethodImplOptions.InternalCall)]
	//	set;
	// Use this for initialization
	private bool xCheckbox = true;
	private bool yCheckbox = true;
	private bool zCheckbox = true;
	private bool _isSelect;
	public bool isSelect{
		get{ 
			return _isSelect;

		}		
	}
	void Start () {
		Button btn = gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (delegate {
			if(keyHandler!=null){
				RadioEventArgs arg = new RadioEventArgs();
				arg.Key = gameObject.name;
				keyHandler(this,arg);
			}
		});

		//SpriteRenderer render = gameObject.GetComponent<SpriteRenderer> ();
		//render.material.shader = Shader.Find ("Sample/mohu");
		this.unSelected();
		Text t = gameObject.GetComponentInChildren<Text> ();
		t.color = Color.white;

		if (keySrc != null) {
			if (keySrc == "JS") {
				t.text = "Joystick | Keyboard";
			} else if (keySrc =="NOR"){
				t.text = "Normal iOS";
			}
		}

		if (_isPressed) {

			this.setPressed(true);
		}
	}

	void OnGUI (){
		CreateAxisCheckboxes ("bbb");

		//Source transform
		GUILayout.BeginHorizontal();
		GUILayout.Label("Align to: \t");
		//source = EditorGUILayout.ObjectField(source, typeof(Transform)) as Transform;
		GUILayout.EndHorizontal();
	}
	/// <summary>
	/// Draws the 3 axis checkboxes (x y z)
	/// </summary>
	/// <param name="operationName"></param>
	private void CreateAxisCheckboxes(string operationName)
	{
		#if UNITY_EDITOR
		GUILayout.Label(operationName + " on axis", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal();
		xCheckbox = GUILayout.Toggle(xCheckbox, "X");
		yCheckbox = GUILayout.Toggle(yCheckbox, "Y");
		zCheckbox = GUILayout.Toggle(zCheckbox, "Z");
		GUILayout.EndHorizontal();

		EditorGUILayout.Space();
		#endif
	}
	// Update is called once per frame
	void Update () {

	}

	public void OnPointerDown(PointerEventData data){
		print ("radio joystick on pointer down");
		GameObject panel = (GameObject)gameObject.transform.parent.gameObject;
		for (int i = 0; i < panel.transform.childCount; i++) {
			GameObject child = panel.transform.GetChild (i).gameObject as GameObject; 
			//Image component = child.GetComponent<Image> ();
			//component.color = Color.white;

			this.unSelected();//todo>?? not sure this funtional

		}
		//		Image img = gameObject.GetComponent<Image>();
		//		img.color = Color.grey;
		this.selected();
		if (keyHandler != null) {
			RadioEventArgs args = new RadioEventArgs ();

			args.Key = gameObject.name;
			keyHandler(this,args);
		}

	}
	public void setPressed(bool bol){
		if (bol == true) {
			//Button btn = gameObject.GetComponent<Button> ();
			Image bg = gameObject.GetComponent<Image>();
			bg.sprite = Resources.Load<UnityEngine.Sprite>("btn_blue");
			privatestatus = "blue";
		} else {
			Image bg = gameObject.GetComponent<Image>();

			bg.sprite = Resources.Load<UnityEngine.Sprite>("btn_darkblue");
			privatestatus = "dark";
		}
	}
	public class RadioEventArgs : EventArgs
	{
		public string Key{ get; set;}
	}
	public void selected(){
		_isSelect = true;
		Image bg = gameObject.GetComponent<Image>();
		bg.sprite = Resources.Load<UnityEngine.Sprite>("btn_bannerblue");

	}
	public void unSelected(){
		_isSelect = false;
		if (privatestatus == "blue") {
			Image bg = gameObject.GetComponent<Image>();
			bg.sprite = Resources.Load<UnityEngine.Sprite>("btn_blue");
		} else {
			Image bg = gameObject.GetComponent<Image>();
			bg.sprite = Resources.Load<UnityEngine.Sprite>("btn_darkblue");
		}
	}

}
//}


