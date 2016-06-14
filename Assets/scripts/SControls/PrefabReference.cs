using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace SControls
{
	public class PrefabReference:MonoBehaviour{

	//	// Use this for initialization
	//	void Start () {
	//	
	//	}
	//	
	//	// Update is called once per frame
	//	void Update () {
	//	
	//	}
	#if UNITY_EDITOR
		[MenuItem("SControls/Check Prefab Use ?")]
		private static void OnSearchForReferences()
		{
			//确保鼠标右键选择的是一个Prefab
			if(Selection.gameObjects.Length != 1)
			{
				return;
			}
			
			//遍历所有游戏场景
			foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
			{
				if(scene.enabled)
				{
					//打开场景
					EditorApplication.OpenScene(scene.path);
					//获取场景中的所有游戏对象(FindObjectsoftype is on monobehavior)
					GameObject []gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
					foreach(GameObject go  in gos)
					{
						//判断GameObject是否为一个Prefab的引用
						if(PrefabUtility.GetPrefabType(go)  == PrefabType.PrefabInstance)
						{
							UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(go); 
							string path = AssetDatabase.GetAssetPath(parentObject);
							//判断GameObject的Prefab是否和右键选择的Prefab是同一路径。
							if(path == AssetDatabase.GetAssetPath(Selection.activeGameObject))
							{
								//输出场景名，以及Prefab引用的路径
								Debug.Log(scene.path  + "  " + GetGameObjectPath(go));
							}
						}
					}
				}
			}
		}

		public static string GetGameObjectPath(GameObject obj)
		{
			string path = "/" + obj.name;
			while (obj.transform.parent != null)
			{
				obj = obj.transform.parent.gameObject;
				path = "/" + obj.name + path;
			}
			return path;
		}

		[MenuItem ("SControls/Scane assets")]
		private static void ScaneAssetsSub(){
			//List<string> strs = GetObjectNameToArray<string>("Assets/_Prefabs","meta");
			List<string> strs = GetObjectNameToArray<string>("","meta");
			//int count =  0;
			string final = string.Empty;
			string output = Application.dataPath + "/../Temp/scaneasset.txt";
			foreach(string path in strs){
				WWW www = new WWW ("file://" + path);
				while(www.isDone==false){
					string bbc = "";
					string aac = bbc;
				}
				string[] lines = www.text.Split('\n');


				if(File.Exists(output)==true){
					File.Delete(output);
				}


				foreach(string line in lines){
					if(line.StartsWith("guid")){
						final+=line + "|" + path + "\n\r";//it is for windows only, in macos has different symbol from changing line.
						//FileStream fs1;
//						if( File.Exists(output)==true){
//							fs1	= new FileStream(output, FileMode.Append, FileAccess.Write);//创建写入文件 
//						}
//						else{
//							fs1 = new FileStream(output, FileMode.Create, FileAccess.Write);
//						}
						//System.Text.Encoding encoder = System.Text.Encoding.UTF8;  
						//byte[] bytes = encoder.GetBytes(line);  

						//StreamWriter sw = new StreamWriter(output,true);//not work, i don't know why
						//sw.WriteLine(line);//开始写入值
						//fs1 = File.OpenWrite(output);  
						//设定书写的開始位置为文件的末尾  
						//fs1.Position = fs1.Length;  
						//fs1.Position = count;
						//将待写入内容追加到文件末尾  
						//fs1.Write(bytes, 0, bytes.Length);  
						//count +=bytes.Length;
						//sw.Close();
						//fs1.Close();
					}
				}

				//www.text
				Debug.Log(path);
			}
			StreamWriter sw = new StreamWriter(output,true);//not work, i don't know why
			sw.WriteLine(final);//开始写入值
			sw.Close();
		}
		private static void WriteLine(string line){

		}

		private static List<string> nameArray=new List<string>();
		/// <summary>
		/// 根据指定的 Assets下的文件路径 返回这个路径下的所有文件名//
		/// </summary>
		/// <returns>文件名数组</returns>
		/// <param name="path">Assets下“一"级路径</param>
		/// <param name="pattern">筛选文件后缀名的条件.</param>
		/// <typeparam name="T">函数模板的类型名t</typeparam>
		private static List<string> GetObjectNameToArray<T>(string path, string pattern)
		{   
			string objPath = string.Empty;
			if (path == "") {
				objPath = Application.dataPath;
			} else{
				objPath = Application.dataPath + "/" + path;  
			}
			string[] directoryEntries;  
			try   
			{  
				//返回指定的目录中文件和子目录的名new WWW ("file://E:/项目/Assets/StreamingAssets/Actor.assetbundle");称的数组或空数组
				//directoryEntries = System.IO.Directory.GetFileSystemEntries(objPath);   
				
//				for(int i = 0; i < directoryEntries.Length ; i ++){  
//					string p = directoryEntries[i];  
//					//得到要求目录下的文件或者文件夹（一级的）//
//					//objPath = {$project} + "/Assets/" + path + "/" + {$fileName}
//					string[] tempPaths;
//					if(path == ""){
//						tempPaths = StringExtention.SplitWithString(p,"/Assets\\"); 
//					}
//					else
//					{
//						tempPaths = StringExtention.SplitWithString(p,"/Assets/"+path+"\\"); 
//					}
//					
//					//tempPaths 分割后的不可能为空,只要directoryEntries不为空//
//					//if(tempPaths[1].EndsWith(".meta"))
//					//	continue;
//					string[] pathSplit = StringExtention.SplitWithString(tempPaths[1],".");  
//
//					//文件
//					if(pathSplit.Length > 1)
//					{  
//						if(tempPaths[1].EndsWith(pattern)){
//
//							//nameArray.Add(pathSplit[0]); 
//							nameArray.Add(tempPaths[0]+"/Assets/"+path+"/" +tempPaths[1]);
//						}
//					}
//					//遍历子目录下 递归吧！
//					else
//					{
//						//subpath = path + "/" + {$foldername}
//						GetObjectNameToArray<T> (path+"/"+pathSplit[0], pattern);
//						continue;
//					}
//				} sds
			}  
			catch (System.IO.DirectoryNotFoundException)   
			{  
				Debug.Log("The path encapsulated in the " + objPath + "Directory object does not exist.");  
			} 

			return nameArray;
		}
	#endif
	}
}