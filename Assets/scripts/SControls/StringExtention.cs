//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
namespace SControls
{
	public class StringExtention {  
		
		public static string[] SplitWithString(string sourceString, string splitString){
			string tempSourceString = sourceString;
			List<string> arrayList = new List<string>();  
			string s = string.Empty;  
			while (sourceString.IndexOf(splitString) > -1)  //分割
			{  
				s = sourceString.Substring(0, sourceString.IndexOf(splitString));  
				sourceString = sourceString.Substring(sourceString.IndexOf(splitString) + splitString.Length);  
				arrayList.Add(s);  
			} 
			arrayList.Add(sourceString); 
			return arrayList.ToArray();  
		}  
	}
}

