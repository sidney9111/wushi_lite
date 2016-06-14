using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SControls;
public class JoystickUIManager : MonoBehaviour {

	Dictionary<string,string> lst =new Dictionary<string,string>();
	private string curr = "";
	/// <summary>
	/// model of touch or joystick
	/// </summary>
	private string model = "touch";
	private static JoystickUIManager _instance;
	public string Model{get{ return model;}}
	private string[] availableKeys = {"A","W","S","D", //up,down,left,right 
	"J","K","L",//roll ,attack, hit, 3 type of wushi action
	"JS","NOR"//joystick and normal mode
	};
	private Dictionary<string,string> keycodeMapping;
	//没办法，好像没找到keycode和keystring之间的映射
	//所以自己写一个吧
	private bool _isNew;//
	public bool isNew{
		get{ 
			return _isNew;
		}
	}
	// Use this for initialization
	void Start () {
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			Transform t =gameObject.transform.GetChild (i);	
			JoystickButton script = t.GetComponent<JoystickButton> ();
			if (script != null) {
				script.keyHandler += OnCustomerKeySelected;
			}

			JoystickRadio radio = t.GetComponent<JoystickRadio> ();
			if (radio != null) {
				radio.keyHandler += OnRadioSelected;
			}
		}
		_instance = this;
		Hide ();
		//20160609 hide or show function is for customrize only
		//the code convert function is on a different channel(parse function)


		Button btn = gameObject.GetComponentInChildren<Button>();
		EventTriggerListener.Get(btn.gameObject).onUp = onPointerUp;

		//depends on joystick is connected to current ios
		STouch touch = new STouch();
		if (touch.isTouchPlatForm==false) {
			for (int i = 0; i < gameObject.transform.childCount; i++) {
				Transform t = gameObject.transform.GetChild (i);
				JoystickRadio radio = t.GetComponent<JoystickRadio> ();
				if (radio != null && radio.keySrc == "JS") {
					//print ("manager js select");
					radio.isPressed = true;
				}
			}
		} else {
			for (int i = 0; i < gameObject.transform.childCount; i++) {
				Transform t = gameObject.transform.GetChild (i);
				JoystickRadio radio = t.GetComponent<JoystickRadio> ();
				if (radio != null && radio.keySrc == "NOR") {
					//print ("manager nor select");
					//radio.selected ();//这里select不能 
					radio.isPressed = true;
				}
			}
		}
		this.initByPlayerPrefs ();
		this.initKeycodeMapping ();
		Debug.Log ("jsm start event");
		StartCoroutine(CheckForControllers());

	}

	private bool connected = false;
	IEnumerator CheckForControllers()
	{
		while(true)
		{
			var controllers = Input.GetJoystickNames();
			print ("while count=" + controllers.Length.ToString());
			if (!connected && controllers.Length > 0)
			{
				connected = true;
				Debug.Log("jsm Connected");
			}
			else if (connected && controllers.Length == 0)
			{
				connected = false;
				Debug.Log("jsm Disconnected");
			}
			yield return new WaitForSeconds(1f);
		}
	}


	private void onPointerUp(GameObject obj){
		print ("manager on pointer up");
		Button btn = obj.GetComponent<Button> ();
		btn.Select ();
		//button 不select了，直接用shader置灰，参考自CSDN
		//http://blog.csdn.net/onerain88/article/details/12197277
		//只能在gnui用，貌似
		//汗颜啊，群众的力量还是无敌的
		//自发光
		//http://forum.china.unity3d.com/thread-265-1-1.html
		//20160511会写没用啊，到底怎么用
		//Button btn = obj as Button;
		//btn.spriteState = Button.o
	}

	// Update is called once per frame
	void Update () {
		
		if (Input.anyKeyDown == true) {
			print ("joystickuimanager anykey down");
			Canvas canvas = gameObject.GetComponent<Canvas>();
			if (canvas.enabled == false) {
				print ("test joystickuimanager canvas enabled == false, this value may control by programmer using show or hide function from script");
				return;
			}
			if(Input.GetKeyDown(KeyCode.Joystick8Button4)){
				//L
				print("press l");
				undoKeyButton();
				return;
			}
			if(Input.GetKeyDown(KeyCode.JoystickButton4)
				|| Input.GetKeyDown(KeyCode.LeftArrow)){//即使是leftarrow "Input.inputstring"还是null
				//L
				print("press 0l");
				undoKeyButton();
				return;
			}
			if (Input.GetKeyDown (KeyCode.Joystick8Button5)) {//暂时没有知道Joystick8是什么（JoystickButton5是普通的摇杆）
				//R
				print("press r");
				doKeyButton();
				return;
			}
			if (Input.GetKeyDown (KeyCode.JoystickButton5)
				|| Input.GetKeyDown(KeyCode.RightArrow)) {//即使是rightarrow "Input.inputstring"还是null
				//R
				print("press 0r");
				doKeyButton();
				return;
			}
			if (Input.GetKeyDown (KeyCode.Tab)) {
				doKeyButton ();
				return;
			}
			if (Input.inputString == "%" || Input.inputString == "~"
				|| Input.inputString == "{" || Input.inputString == "}") {
				return;
			}


			//传递到子元素，显示用---------------------------------------------------------
			//~只有能改变的时候才改变~
			for (int i = 0; i < gameObject.transform.childCount; i++) {
				Transform t =gameObject.transform.GetChild (i);	
				JoystickButton script = t.GetComponent<JoystickButton> ();
				//script.keyHandler +=OnCustomerKeySelected;
				if (script != null) {
					string pressedString = this.pressedString;
					if (string.IsNullOrEmpty (pressedString) == false) {//容易出错，要细心处理，只有不等于null的时候才触发
						script.OnKeyDown (curr, pressedString.ToUpper ());

					}
				}
				//script.OnKeyDown(curr,Input.inputString.ToUpper());
			}
			//--------------------------------------------------------------------------
			if(isMouseDown()==true){
				return;
			}
			//if (curr == "joystick") {
			//如果manager是摇杆状态，不可用手点击（
			//但实际情况是，这个界面本来就不应该接受touchcount）
			//也就是手点击，无论manager是什么情况，都不应出现，除了debug的时候
			if (Input.touchCount > 0) {
				return;	
			}

			//}

			print ("test joystickuimanager update function");
			//print (Input.inputString);//joystick的时候，会返回null ,print null会出错
			//			if (Input.inputString.ToUpper() == KeyCode.A.ToString()) {
			//				print ("tick a");
			//			}
			//


			if(string.IsNullOrEmpty(curr)){
				print ("joystick manager, unrecognize curr id'");
				return;
			}
			//			if(string.IsNullOrEmpty(Input.inputString)){
			//				print ("joystick manager, unrecognize inputstring, especially test for joystick");
			//				return;
			//			}


			if (lst.ContainsKey (curr)) {
				print("contain curr" + curr);
				print ("Input string = "+Input.inputString);
				//lst [curr] = Input.inputString.ToUpper ();
				lst[curr] = this.pressedString.ToUpper();
			} else {
				print("contain curr not" + curr);
				print ("Input string = "+Input.inputString);
				//lst.Add(curr,Input.inputString.ToUpper());
				lst.Add(curr,this.pressedString.ToUpper());
			}

		}
	}
	private string pressedString{
		get{ 
			//if(Input.GetKeyDown(KeyCode

			if (Input.GetKeyDown (KeyCode.Joystick8Button0)) {
				print("joystick manager press a");
				return "A";
			} else if (Input.GetKeyDown (KeyCode.Joystick8Button1)) {
				print("joystick manager press b");
				return "B";
			} else if (Input.GetKeyDown (KeyCode.Joystick8Button2)) {
				print("joystick manager press x");
				return "X";
			} else if (Input.GetKeyDown (KeyCode.Joystick8Button3)) {
				print("joystick manager press y");
				return "Y";
			} else if (Input.GetKeyDown (KeyCode.JoystickButton0)) {
				print("joystick manager press a");
				return "A";
			} else if (Input.GetKeyDown (KeyCode.JoystickButton1)) {
				print("joystick manager press b");
				return "B";
			} else if (Input.GetKeyDown (KeyCode.JoystickButton2)) {
				print("joystick manager press x");
				return "X";
			} else if (Input.GetKeyDown (KeyCode.JoystickButton3)) {
				print("joystick manager press y");
				return "Y";
			} else {


				//				if(string.IsNullOrEmpty(Input.inputString)){
				//					//print("joystick manager press null");	
				//				}else{
				//					print("joystick manager press string " + Input.inputString);
				return Input.inputString;	
				//				}

			}
		}
	}
	private int getKeyButtonIndex(string key){
		//		if (key == null) {
		//			return -1;
		//		}
		for (int i = 0; i < gameObject.transform.childCount; i++) {

			Transform t = gameObject.transform.GetChild (i);	
			JoystickButton script = t.GetComponent<JoystickButton> ();
			//script.keyHandler +=OnCustomerKeySelected;
			JoystickRadio radio = t.GetComponent<JoystickRadio>();
			if (script != null || radio!=null){
				if (script != null && script.keySrc == key) {
					return i;
				}
				if (radio != null && t.name == key) {
					return i;
				}
				//return -1;
			}else{
				return -1;
			}
		}
		return -1;
	}
	//	private void doKeyButtonSub(Transform tran){
	//		JoystickButton btn = tran.GetComponent<JoystickButton> ();
	//		if (btn != null) {
	//			curr = btn.keySrc;
	//			btn.selected ();
	//		}
	//
	//		JoystickRadio radio = tran.GetComponent<JoystickRadio> ();
	//		if (radio != null) {
	//			radio.selected ();
	//		}
	//
	//	}
	private void doKeyButton(){
		if (curr == null) {
			Transform t = gameObject.transform.GetChild (0);
			//doKeyButtonSub (t);
			JoystickUIManager.instance.selected(t);
		} else {
			int index = getKeyButtonIndex (curr);
			print ("cur="+curr);
			print ("first index=" + index);
			if (index == gameObject.transform.childCount - 1) {
				index = 0;
			} else {
				index += 1;
			}
			print ("HitstucjYUnabager index="+index);
			Transform t = gameObject.transform.GetChild (index);
			//doKeyButtonSub (t);
			JoystickUIManager.instance.selected(t);
		}
	}
	//	private void undoKeyButtonSub(Transform tran){
	//		JoystickButton btn = tran.GetComponent<JoystickButton> ();
	//		if (btn != null) {
	//			curr = btn.keySrc;
	//			btn.unSelected ();
	//		}
	//
	//		JoystickRadio radio = tran.GetComponent<JoystickRadio> ();
	//		if (radio != null) {
	//			radio.unSelected ();
	//		}
	//	}
	private void undoKeyButton(){//下一个，并不是undo
		if (curr == null) {
			int max = gameObject.transform.childCount - 1;
			Transform t = gameObject.transform.GetChild (max);
			//undoKeyButtonSub (t);
			JoystickUIManager.instance.selected(t);
		} else {
			int index = getKeyButtonIndex (curr);
			if (index == 0) {
				index = gameObject.transform.childCount - 1;
			} else {
				index -= 1;
			}
			print ("cur=" + curr);
			print ("index =" + index);
			Transform t = gameObject.transform.GetChild (index);
			//undoKeyButtonSub (t);
			JoystickUIManager.instance.selected(t);

		}
	}
	public void OnCustomerKeySelected(object sender,JoystickButton.KeyEventArgs e){
		curr = e.Key;
		//JoystickButton也是有disselected,在JoystickButton处理
		print ("manager cur key = " + curr);
	}
	public void OnRadioSelected(object sender,JoystickRadio.RadioEventArgs e){
		model = e.Key;
		//JoystickRadio有disselected，在这里处理
		for(int i=0;i<gameObject.transform.childCount;i++){
			Transform tran = gameObject.transform.GetChild (i);
			JoystickRadio radio = tran.GetComponent<JoystickRadio> ();
			if (radio != null) {
				if (tran.name == e.Key) {
					radio.setPressed (true);
				} else {
					radio.setPressed (false);
				}
			}
		}

	}
	private Transform getChild(string id){
		for(int i=0;i<gameObject.transform.childCount;i++)
		{
			Transform t= gameObject.transform.GetChild (i);
			if (t.GetInstanceID().ToString() == id) {
				return t;
			}
		}
		return null;
	}
	private Transform getChildByKey(string key){
		
		//if (this.gameObject == null) {
		if(this._isNew==true){
			return null;
		}
		for(int i=0;i<gameObject.transform.childCount;i++)
		{
			Transform t= gameObject.transform.GetChild (i);
			JoystickButton script = t.GetComponent<JoystickButton> ();

			if (script!=null && script.keySrc == key) {
				return t;
			}

			JoystickRadio radio = t.GetComponent<JoystickRadio> ();
			if (radio != null && radio.keySrc == key) {
				return t;
			}
		}
		return null;
	}
	/// <summary>
	/// Parse the specified code.
	/// 这个方法在update多次调用，所以以后要做个静态缓存吧
	/// </summary>
	/// <param name="code">Code.</param>
	public KeyCode parse(KeyCode code){
		//print("key parse code=" + code.ToString());
		if (Input.anyKeyDown) {
			
			//print ("System.Enum.GetName(KeyCode" + code.ToString ());
		}
		//Remark on 20160609 by Sidney
		//the child button or radio need to be named as 'A','S'
		Transform t = getChildByKey (code.ToString());
		if (t == null) {
			//todo> need a feedback? or just return the original code?
			return code;
		}
		JoystickButton script = t.GetComponent<JoystickButton> ();
		KeyCode c;
		string key = string.Empty; 
		if (string.IsNullOrEmpty (script.keyDefine) == false) {
			//c = (KeyCode)System.Enum.Parse (typeof(KeyCode), script.keyDefine);
			key = script.keyDefine;
		} else {
			//c = (KeyCode)System.Enum.Parse (typeof(KeyCode), script.keySrc);
			//key = script.keySrc;
			key = "F12";//F12无用，所以暂时用F12作为null返回值
		}
		if (keycodeMapping.ContainsKey (key)) {
			c = (KeyCode)System.Enum.Parse (typeof(KeyCode), keycodeMapping [key]);
		} else {
			c = (KeyCode)System.Enum.Parse (typeof(KeyCode), key);
		}

		//return KeyCode.lef
		if(Input.anyKeyDown){
			print ("result from keyDefine " + script.keyDefine);
			print ("result " + c.ToString ());
		}
		//print ("return c code=" + c.ToString ());
		return c;
	}
	public static JoystickUIManager instance {

		get{ 
			if (_instance == null) {
				_instance = new JoystickUIManager ();
				_instance._isNew = true;

			}
			return _instance;
		}
		//set;
	}
	public void selected(Transform tran){
		//触发unselected其他
		//并且设置curr
		for(int i=0;i<gameObject.transform.childCount;i++){
			Transform child = gameObject.transform.GetChild (i);
			if (child.name == tran.name) {
				JoystickButton btn = child.GetComponent<JoystickButton> ();
				if (btn != null) {
					curr = btn.keySrc;
					btn.selected ();

				}
				JoystickRadio radio = child.GetComponent<JoystickRadio> ();
				if (radio != null) {
					curr = tran.name;
					radio.selected ();
				}
			} else {
				JoystickButton btn = child.GetComponent<JoystickButton> ();
				if (btn != null) {
					btn.unSelected ();
				}
				JoystickRadio radio = child.GetComponent<JoystickRadio> ();
				if (radio != null) {
					radio.unSelected ();
				}
			}
		}
	}
	public void Show(){
		Canvas canvas = gameObject.GetComponent<Canvas>();
		canvas.enabled = true;
	}
	public void Hide(){
		//gameObject
		Canvas canvas = gameObject.GetComponent<Canvas>();
		canvas.enabled = false;
	}

	public bool isMouseDown(){
		if (Input.GetMouseButtonDown (0) == true
			|| Input.GetMouseButtonDown(1)==true) {//左键和右键
			return true;
		} else {
			return false;
		}
	}

	public bool commit(){
		for(int i=0;i<gameObject.transform.childCount;i++){
			Transform child = gameObject.transform.GetChild (i);
			JoystickButton btn = child.GetComponent<JoystickButton> ();
			if (btn != null) {
				if(isContain(availableKeys,btn.keySrc)){
					PlayerPrefs.SetString (btn.keySrc , btn.keyDefine);
				}
			}

			JoystickRadio radio = child.GetComponent<JoystickRadio> ();
			if (radio != null) {
				if(isContain(availableKeys,radio.keySrc)){
					PlayerPrefs.SetString (radio.keySrc , radio.isSelect.ToString() );
				}

			}

		}

		return true;
		//PlayerPrefs pref = new PlayerPrefs ();
	
	}

	private bool isContain(string[] arr,string key){
		foreach(string s in arr){
			if(s==key){
				return true;
			}
		}

		return false;
	}

	private bool initByPlayerPrefs(){
		//Debug.Log ("JoystickUImanager init");
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			Transform t = gameObject.transform.GetChild (i);
			JoystickButton btn = t.GetComponent<JoystickButton> ();
			if (btn != null) {
				string value = PlayerPrefs.GetString (btn.keySrc);
//				if (string.IsNullOrEmpty(value) == false) {
//					btn.keyDefine = value;
//				}
				if (value != null) {
		//			Debug.Log ("src=" + btn.keySrc + " & defin=" + value);
					btn.keyDefine = value;
					btn.updateText ();
				}
			}

			//在manager初始化的时候，radio还没初始化，貌似radio.selected没事
			JoystickRadio radio = t.GetComponent<JoystickRadio> ();
			if (radio != null) {
				string value = PlayerPrefs.GetString (radio.keySrc);
				if (string.IsNullOrEmpty (value)) {
					if (value.ToLower () == "true" || value.ToLower () == "false") {
						if (bool.Parse (value) == true) {
							//radio.selected ();
							radio.isPressed = true;
							//only ui update, may not update the other ui,
							//but it's ok when this function call on the begining of class
						} else {
						}
					}

				}
			}
		}

		return true;
	}	
	private void initKeycodeMapping(){
		keycodeMapping = new Dictionary<string,string> ();
		keycodeMapping.Add ("+", "Plus");
		keycodeMapping.Add ("=", "Equals");//Equals and KeypadEquals
		keycodeMapping.Add("-","Minus");
		keycodeMapping.Add ("_", "Underscore");
		keycodeMapping.Add (".", "Period");
		keycodeMapping.Add (">", "Greater");
		keycodeMapping.Add (",", "Comma");
		keycodeMapping.Add ("<", "Less");
		keycodeMapping.Add ("/", "Slash");
		keycodeMapping.Add ("?", "Question");
		keycodeMapping.Add (";", "Semicolon");
		keycodeMapping.Add (":", "Colon");
		keycodeMapping.Add ("'", "Quote");
		keycodeMapping.Add ("\"", "DoubleQuote");
		keycodeMapping.Add ("[", "LeftBracket");
		//keycodeMapping.Add ("{", "");
		keycodeMapping.Add ("]", "RightBracket");
		//keycodeMapping.Add ("}", "");
		keycodeMapping.Add("\\","BackSlash");
		//keycodeMapping.Add ("|", "");
		keycodeMapping.Add("`","BackQuote");
		//keycodeMapping.Add ("~", "");

		keycodeMapping.Add ("1", "Alpha1");
		keycodeMapping.Add ("2", "Alpha2");
		keycodeMapping.Add ("3", "Alpha3");
		keycodeMapping.Add ("4", "Alpha4");
		keycodeMapping.Add ("5", "Alpha5");
		keycodeMapping.Add ("6", "Alpha6");
		keycodeMapping.Add ("7", "Alpha7");
		keycodeMapping.Add ("8", "Alpha8");
		keycodeMapping.Add ("9", "Alpha9");
		keycodeMapping.Add ("0", "Alpha0");
		keycodeMapping.Add ("!", "Exclaim");
		keycodeMapping.Add ("@", "At");
		keycodeMapping.Add ("#", "Hash");
		keycodeMapping.Add ("$", "Dollar");
		//I couldn't find out the percent symbol
		//keycodeMapping.Add ("%", "");
		keycodeMapping.Add ("^", "Caret");
		keycodeMapping.Add ("&", "Ampersand");
		keycodeMapping.Add ("*", "Asterisk");
		keycodeMapping.Add ("(", "LeftParen");
		keycodeMapping.Add (")", "RightParen");

	
	} 
}
