
  / \
  |.|　　
  |.|
  |:|　　　__
,_|:|_,　 /　)
 (Oo　　/ _I_
  +\ \　||^ ^|
 　 \ \||_0→   
　　 \/.:.\-\
  　　|.:. /-----\
  　　|___|::oOo::|
  　　/　 |:<_T_>:|
      ""....""
Summary:

无尽版本，一个提供键盘和手柄支持的ACT游戏
Wush_endless project, it is an ACT game with keyboard and joystick support.
所有脚本专门设计用于整合项目，主要是以下2个文件夹
All scripts in this project is design to be easily ingegrated into your project, which is base on  2 packs：
"./Scripts/SControls/"
"./Scripts/SJoystick/"


Upgrade Log:

version 1.1, publish on 2016.06.01, 
1.Full Test in Android and PC OS, cause of iOS9 and iOS10 is not well prepared, iOS version is comming soon
2.A setting controls to set keyboard or joystick mapping，player could change whatever they want
3.A demo of ACT game with muliplate controls for Testing: screen touch, keyboard keys, joystick buttons and trigger axis are compatible.

1.多次测试通过Android和PC版本，iOS蓝牙机制原因导致凸锤，iOS9和iOS10修复中
2.提供配置界面，以备玩家修改（!- -某些按键不能用，例如F12）
3.多操作方式的ACT游戏原型，以供测试, 键盘，手触，手柄，已经进行兼容测试


Demo GuidLine: 

1.Check your InputManager Setting(UnityEidtor-->Edit-->Project Settings->Input), ensure to have keys "Horizontal" and "Vertical", it should be well prepared by unity3d 5.3.3. the params of "Horizontal" should be:
----------------------------
Gravity		:0
Dead		:.19
Sensitivity	:1
Type		:Joystick Axis
Axis		:X axis
Joy Num		:Get Motion from all Joysticks
----------------------------
more setting guidline refer to document "Design/setting.doc"
2.Open dojo.unity scene under Assets Root Folder
3.Run
4.1.Connect Joystick
Axis to control character's directions and attack keys could be set by menu scene on top_right
4.2.Keyboard on PC|Mac Platform
character's directions(default key is AWSD) and attack keys(default key is JKL) could be set by menu scene on top_right
5.The othe demo functiosn refer to document "Design/func.doc"


1.检查InputManager设置(UnityEidtor-->Edit-->Project Settings->Input),确保已经设置“Horizontal”和“Vertical”2个参数， unity3d 5.3.3版本已默认设置，具体参数如下：
----------------------------
Gravity		:0
Dead		:.19
Sensitivity	:1
Type		:Joystick Axis
Axis		:X axis
Joy Num		:Get Motion from all Joysticks
----------------------------
更多的设置指引请参考“Design/setting.doc”文档
2.打开在Assets根目录下的dojo.unity界面
3.Run the project
4.1连接蓝牙手柄
位置和攻击按键可以通过右上的设置按钮，进入界面设置
4.2用PC|Mac键盘
位置和攻击按键可以通过右上的设置按钮，进入界面设置


Contract & Support:

测试手柄型号：
viaplay F2
技术支持邮箱：
sidney9111@gmail.com
