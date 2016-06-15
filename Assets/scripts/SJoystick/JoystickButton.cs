using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class JoystickButton : MonoBehaviour, IPointerDownHandler,IPointerClickHandler{
	/*
	 * gameObject's name
	 * a - left
	 * w - up
	 * d - right
	 * s - down
	 * j - roll
	 * k - attack
	 * l - offence
	 */
	public event EventHandler<KeyEventArgs> keyHandler;
	// Use this for initialization
	/// <summary>
	/// start()初始化时会取name，所以name尽量取S值
	/// </summary>
	public String keySrc;
	public String keyDefine;
	private bool _enabled = true;
	public bool enabled{
		get{
			return _enabled;
		}
		set{
			_enabled = value;
		}
	}

	void Start () {
		keyHandler += OnKey;
		//keyDefine = keySrc = gameObject.name.ToUpper();
		//20160609 I dont know the code on top for
		//there should not reset value of kyDefine or keySrc on common
		//it may be setted to avoid no default value when programmer new this component
		//remarked this code anyway
		//keyDefine = "";


		//		if (keySrc == "A") {
		//			keyDefine = "Q";
		//		}
		updateText();
	}
	public void updateText(){
		Text t = gameObject.transform.GetComponentInChildren<Text>();
		t.text = this.keyDefine;
	}
	public String Text{
		get{
			Text t = gameObject.transform.GetComponentInChildren<Text>();
			return t.text;
		}
		set {
			Text t = gameObject.transform.GetComponentInChildren<Text>();
			t.text = value; 
		}
	}
	public void OnKey(object sender, EventArgs e){
		//print ("onkey tick");
	}
	// Update is called once per frame
	void Update () {

	}
	public void OnPointerDown(PointerEventData data){
		//		print ("on pointer donw");
		//if(data.position
		//		print("name"+gameObject.name);

		print ("button onpointer down");
		//actually, this joystickbutton is other offensive, like radio
		//obviouslly, it will diselect others when it is selected
		GameObject panel = (GameObject)gameObject.transform.parent.gameObject;
		for (int i = 0; i < panel.transform.childCount; i++) {
			GameObject child = panel.transform.GetChild (i).gameObject as GameObject; 
			//Image component = child.GetComponent<Image> ();
			//component.color = Color.white;

			this.unSelected(child);	


		}

		//		Image img = gameObject.GetComponent<Image>();
		//		img.color = Color.grey;
		this.selected();

		if (keyHandler != null && _enabled == true) {
			KeyEventArgs args = new KeyEventArgs ();
			//be sure to mark different name by editor
			//or it will be used as identifier on later keydown events
			//seriously
			args.Key = gameObject.name;
			keyHandler(this,args);
		}
		//img.IsRaycastLocationValid
		//Physics.Raycast(

		//		if (Input.GetMouseButtonDown(0)||(Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began))
		//		{
		//			#if IPHONE || ANDROID
		//			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
		//			#else
		//			if (EventSystem.current.IsPointerOverGameObject())
		//			#endif
		//				Debug.Log("当前触摸在UI上");
		//
		//			else 
		//				Debug.Log("当前没有触摸在UI上");
		//		}

		//		if (EventSystem.current.IsPointerOverGameObject()){
		//			
		//		}


	}
	public void OnPointerClick(PointerEventData data){

		//		print("JoystickUI OnPoniterClick");
	}
	/// <summary>
	/// Raises the key or joystick down.
	/// None system default event
	/// </summary>
	/// <param name="buttonId">id or name, depend on coder</param>
	/// <param name="keycodestring">Keycodestring.</param>
	public void OnKeyDown(string buttonId,string keycodestring){
		//print ("joystickbutton test keydown while, canvas is diabled");
		//		if(String.IsNullOrEmpty(keycodestring)){
		//			return;
		//		}

		//to handle union keycode in the keys group
		//remove existing code if multiplute
		if(keycodestring ==  this.keyDefine){
			this.keyDefine = "";
			Text t = gameObject.transform.GetComponentInChildren<Text>();
			t.text = this.keyDefine;
		}

		if (buttonId == gameObject.name) {
			//update ui
			Text t = gameObject.transform.GetComponentInChildren<Text>();
			t.text = keycodestring;
			keyDefine = keycodestring;

			JoystickUIManager.instance.commit ();
		}
	}
	public void selected(){
		Image img = gameObject.GetComponent<Image>();
		//img.color = Color.grey;
		img.color = Color.white;
		//触发其他unselected
		//不在这里触发了，全部放到manager处理
	}
	public void unSelected(GameObject gb){
		Image img = gb.GetComponent<Image>();
		//img.color = Color.white;
		img.color = Color.grey;
	}
	public void unSelected(){
		//this.gameObject
		unSelected (gameObject);
	}

	public class KeyEventArgs : EventArgs
	{
		public string Key{ get; set;}
	}



}
