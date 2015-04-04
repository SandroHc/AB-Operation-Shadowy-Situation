import System.IO;
import System.Xml;


#pragma strict
var animTest : GameObject;
var viewCam  : aoi_character_pack_v0111_viewscript;
var guiSkin  : GUISkin;
var boneName : String = "Hips";

var camGuiRootRect       : Rect = Rect (870,25,93,420);
var camGuiBodyRect       : Rect = Rect (870,25,93,420);
var animSpeedGuiBodyRect : Rect = Rect (770,520,170,150);
var textGuiBodyRect      : Rect = Rect (20,510,300,70);
var modelBodyRect        : Rect = Rect (20,40,300,500);

var FBXListFile       : String  = "fbx_list";
var AnimationListFile : String  = "animation_list";
var TitleTextFile     : String  = "title_text";
var guiOn             : boolean = true;

private var viewerResourcesPath : String  = "Aoi_v0111";
private var settingsPath        : String  = viewerResourcesPath + "/Viewer Settings";
private var materialsPath       : String  = viewerResourcesPath + "/Viewer Materials";

private var curBG        : int    = 1;
private var curAnim      : int    = 1;
private var curModel     : int    = 1;
private var curLOD       : float  = 0;
private var curModelName : String = "";
private var curAnimName  : String = "";
private var curBgName    : String = "";

private var animSpeed    : float = 1;

private var animationList  : String[];
private var modelList      : String[];
private var backGroundList : String[];
private var lodList        : String[] =["_h","_m","_l"];
private var lodTextList    : String[] =["Hi","Mid","Low"];
 
private var obj    : GameObject;
private var loaded : GameObject;
private var SM     : SkinnedMeshRenderer;

private var txt : TextAsset;

private var CamModeRote : boolean = true;
private var CamModeMove : boolean = false;
private var CamModeZoom : boolean = false;
private var CamModeFix  : boolean = true; 

private var CamMode   : int = 0;
private var titleText : String = "";
private var xDoc      : XmlDocument;
private var xNodeList : XmlNodeList;

private var faceMat_L : Material;
private var faceMat_M : Material;

function Start () {
	viewCam = GameObject.Find("Main Camera").GetComponent("aoi_character_pack_v0111_viewscript");

	faceMat_L = Resources.Load( materialsPath + "f02_face_l", Material);
	faceMat_M = Resources.Load( materialsPath + "f02_face_m", Material);
		
	txt = Resources.Load( settingsPath + "/background_list", TextAsset);
	backGroundList =txt.text.Split(["\r","\n"],1);
	

	txt = Resources.Load( settingsPath + "/" + FBXListFile, TextAsset);
	modelList =txt.text.Split(["\r","\n"],1);
	
	txt = Resources.Load( settingsPath + "/" + AnimationListFile, TextAsset);
	animationList =txt.text.Split(["\r","\n"],1);

	txt = Resources.Load( settingsPath + "/" + TitleTextFile, TextAsset);
	titleText =txt.text;

	
	txt = Resources.Load( settingsPath + "/fbx_ctrl", TextAsset);

	xDoc = new XmlDocument();
	xDoc.LoadXml( txt.text );
	
	SetInitBackGround();
    SetInitModel();
	SetInitMotion(); 
	SetAnimationSpeed(animSpeed);
}

function Update () {

	
if (Input.GetKeyDown("1"))SetNextModel(-1);
if (Input.GetKeyDown("2"))SetNextModel(-1);
			
if (Input.GetKeyDown("q"))SetNextMotion(-1);
if (Input.GetKeyDown("w"))SetNextMotion(1);

if (Input.GetKeyDown("a"))SetNextBackGround(-1);
if (Input.GetKeyDown("s"))SetNextBackGround(1);

if (Input.GetKeyDown("z"))SetNextLOD(-1);
if (Input.GetKeyDown("x"))SetNextLOD(1);

}
private var scale: Vector3;

function OnGUI () {
if(!guiOn)return;
    if (guiSkin) {
        GUI.skin = guiSkin;
    }
    
    //print(Screen.height+" "+Screen.width);
    scale.x = Screen.width / 960.0;
    scale.y = Screen.height / 600.0; 
    scale.x = 1;
    scale.y = 1;
    scale.z = 1.0;
    GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
     

	GUI.Label( Rect(20,20,500,50), titleText, "Title");
	
	GUILayout.BeginArea (modelBodyRect);
			GUILayout.BeginVertical();

				GUILayout.BeginHorizontal();
				if (GUILayout.Button ("","Left")) SetNextModel(-1);
				GUILayout.Label( "","Costume");
				if (GUILayout.Button ("","Right"))SetNextModel(1);
				GUILayout.EndHorizontal();
				GUILayout.Space(20);
			  
				GUILayout.BeginHorizontal();
				if (GUILayout.Button ("","Left")) SetNextMotion(-1);
				GUILayout.Label( "","Anim");
				if (GUILayout.Button ("","Right"))SetNextMotion(1);
				GUILayout.EndHorizontal();
				GUILayout.Space(20);
				 
				GUILayout.BeginHorizontal();
				if (GUILayout.Button ("","Left")) SetNextBackGround(-1);
				GUILayout.Label( "","BG");
				if (GUILayout.Button ("","Right"))SetNextBackGround(1);
				GUILayout.EndHorizontal();
				GUILayout.Space(20);
				  
				GUILayout.BeginHorizontal();
				if (GUILayout.Button ("","Left")) SetNextLOD(-1);
				GUILayout.Label( "","LOD");
				if (GUILayout.Button ("","Right"))SetNextLOD(1);
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			GUILayout.EndVertical(); 
	GUILayout.EndArea ();
	 
	

	GUILayout.BeginArea (animSpeedGuiBodyRect);
		GUILayout.FlexibleSpace();
		var val:float = GUILayout.HorizontalSlider(animSpeed, 0, 2);
		if(animSpeed != val){ 
			animSpeed = val;
			SetAnimationSpeed(animSpeed);
			viewCam.MouseLock(true);
		}else{
			viewCam.MouseLock(false);
		}
		GUILayout.FlexibleSpace();
		var animSpeedText = "Animation Speed : " + String.Format("{0:F1}", animSpeed);
		GUILayout.Box(animSpeedText);
		GUILayout.FlexibleSpace();
	GUILayout.EndArea ();
	
	
	GUI.Label( camGuiRootRect, "", "CamBG");
	GUILayout.BeginArea (camGuiBodyRect);
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical(); 
			//GUILayout.FlexibleSpace();
			var newVal:boolean;
			newVal = GUILayout.Toggle (CamMode==0,"","Rote"); 
			if( CamMode!=0 && newVal ){
				CamMode=0;
				viewCam.ModeRote();
			}
			GUILayout.FlexibleSpace();
			newVal = GUILayout.Toggle (CamMode==1,"","Move"); 
			if( CamMode!=1 && newVal ){
				CamMode=1;
				viewCam.ModeMove();
			}
			GUILayout.FlexibleSpace();
			newVal = GUILayout.Toggle (CamMode==2,"","Zoom"); 
			if( CamMode!=2 && newVal ){
				CamMode=2;
				viewCam.ModeZoom();
			}
			GUILayout.FlexibleSpace();
			newVal = GUILayout.Toggle (CamModeFix,"","Fix");
			if (CamModeFix != newVal) {
				CamModeFix = newVal;
				viewCam.FixTarget(CamModeFix);
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("","Reset")) {
				viewCam.Reset();
			}
			GUILayout.FlexibleSpace();
		GUILayout.EndVertical();  
		GUILayout.EndHorizontal();
	GUILayout.EndArea ();
	
	var text:String="";
	text += "Costume : " + (curModel+1) + " / " + modelList.length + " : " + curModelName + "\n";
	text += "Animation : " + (curAnim+1) + " / " +(animationList.length) +" : "+curAnimName+ "\n";
	text += "BackGround : " + (curBG+1)+" / " +backGroundList.length + " : " + curBgName + "\n";
	text += "Quality : " + lodTextList[curLOD]+ "\n";
	GUI.Box(textGuiBodyRect,text);
}

function SetInitModel() { 
	curModel =0;
	ModelChange(modelList[curModel]+lodList[curLOD]);
} 

function SetNextModel(_add:int) { 
	curModel +=_add;

	if( modelList.Length <= curModel  ) {  
		 curModel = 0;
	} else if(curModel<0){
		 curModel = modelList.Length-1;
	}
	ModelChange(modelList[curModel]+lodList[curLOD]);
	 
}
 
function SetNextLOD(_add:int) { 
	curLOD +=_add;
	
	if( lodList.Length <= curLOD  ) {  
		 curLOD = 0;
	}else if(curLOD<0){
		 curLOD = lodList.Length-1;
	}
	ModelChange(modelList[curModel]+lodList[curLOD]);
}

function ModelChange(_name:String){
	if(_name){
		print("ModelChange : "+_name);
		curModelName = Path.GetFileNameWithoutExtension(_name);
		var loaded = Resources.Load(_name ,GameObject);
		Destroy(obj); 
    	Debug.Log(_name);
    	
		obj = Instantiate(loaded) as GameObject;
    	
    	
    	
		SM = obj.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
		SM.quality = SkinQuality.Bone4;
		SM.updateWhenOffscreen = true;

		var i = 0;
		for each( var mat:Material in SM.GetComponent.<Renderer>().sharedMaterials){
			if(mat.name == "face02_M"){
				SM.GetComponent.<Renderer>().materials[i] = faceMat_M;
			}else if(mat.name == "face02_L"){
				SM.GetComponent.<Renderer>().materials[i] = faceMat_L;
			}
			i++;
		}
		for each ( var anim:AnimationState in animTest.GetComponent.<Animation>()) {
			obj.GetComponent.<Animation>().AddClip(anim.clip,anim.name);
		}
		viewCam.ModelTarget(GetBone(obj,boneName));
  		SetAnimation(""+animationList[curAnim]);
  		SetAnimationSpeed( animSpeed );
  	}
}

function SetAnimationSpeed(_speed:float) {
	for (var state : AnimationState in obj.GetComponent.<Animation>())
	{
	     state.speed = _speed;
	}
}

function SetInitMotion() { 
	curAnim =0;
 	SetAnimation(animationList[curAnim]);
  	SetAnimationSpeed( animSpeed );
 }

function SetNextMotion(_add:int) {
	curAnim +=_add;
	if( animationList.length <= curAnim  ) {  
		 curAnim = 0;
	} else if(curAnim < 0){
		 curAnim = animationList.length-1;
	}
	SetAnimation(animationList[curAnim]);
  	SetAnimationSpeed( animSpeed );
	
}

function SetAnimation(_name:String){ 
	if(_name){
		print("SetAnimation : "+_name);
		curAnimName = ""+_name;
		obj.GetComponent.<Animation>().Play(curAnimName); 
		
		SetFixedFbx( xDoc, obj, curModelName, curAnimName, curLOD ) ;
	}
}

 
 function SetInitBackGround() { 
	curBG =0;
	SetBackGround(backGroundList[curBG]);
 }

function SetNextBackGround(_add:int) {
	curBG +=_add;
	if( backGroundList.length <= curBG  ) {  
		 curBG = 0;
	}  else if(curBG < 0){
		 curBG = backGroundList.length-1;
	}
	SetBackGround(backGroundList[curBG]);
}

function SetBackGround(_name:String) {
	if(_name){
		print("SetBackGround : "+_name);
		curBgName = Path.GetFileNameWithoutExtension(_name);
		var loaded = Resources.Load(_name ,Texture2D);
		var obj = GameObject.Find("BillBoard") as GameObject;
		obj.GetComponent.<Renderer>().material.mainTexture =  loaded;
	}
}

function GetBone(_obj:GameObject,_bone:String){
	var SM:SkinnedMeshRenderer = _obj.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
	if (SM){
		for each( var t:Transform in  SM.bones ){
			if (t.name == _bone ) {
				return t;
			}
		}
	}
}


function SetFixedFbx( _xDoc:XmlDocument, _obj:GameObject, _model:String, _anim:String, _lod:int ){
	if(_xDoc==null)return;
	if(_obj==null)return; 
	
	
	var xNode:XmlNode;
	var xNodeTex:XmlNode;
	var xNodeAni:XmlNode;
	var t:String;
	
    t = "Root/Texture[@Lod=''or@Lod='" + _lod + "'][Info[@Model=''or@Model='" + _model + "'][@Ani=''or@Ani='" + _anim + "']]" ;
	xNodeTex = _xDoc.SelectSingleNode(t);
 
	if(xNodeTex){ 
		var matname:String = xNodeTex.Attributes["Material"].InnerText;
		var property:String = xNodeTex.Attributes["Property"].InnerText;
		var file:String = xNodeTex.Attributes["File"].InnerText;
		print("Change Texture To "+matname+" : " + property +" : "+file);
		for each( var mat:Material in SM.GetComponent.<Renderer>().sharedMaterials){
			if(mat){
			if(mat.name ==  matname){
				var tex:Texture2D = Resources.Load(file ,Texture2D);
				mat.SetTexture( property, tex);
			}
			}
		}
	} 
	
    t = "Root/Animation[@Lod=''or@Lod='" + _lod + "'][Info[@Model=''or@Model='" + _model + "'][@Ani=''or@Ani='" + _anim + "']]" ;
	xNodeAni = _xDoc.SelectSingleNode(t);
	 
	if(xNodeAni){ 
		var ani:String = xNodeAni.Attributes["File"].InnerText;
		curAnimName = ani;
		print("Change Animation To "+curAnimName);
		_obj.GetComponent.<Animation>().Play(curAnimName);
	}
}