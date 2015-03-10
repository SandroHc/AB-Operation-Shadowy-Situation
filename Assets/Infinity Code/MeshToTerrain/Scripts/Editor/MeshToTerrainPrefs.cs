/*       INFINITY CODE 2013         */
/*   http://www.infinity-code.com   */

using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class MeshToTerrainPrefs
{
    private const string appKey = "MTT_";

    public int baseMapResolution = 1024;
    public GameObject boundsGameObject;
    public MeshToTerrainBounds boundsType = MeshToTerrainBounds.autoDetect;
    public MeshToTerrainDirection direction;
    public int detailResolution = 1024;
    public int heightmapResolution = 128;
    public List<GameObject> meshes = new List<GameObject>();
    public MeshToTerrainFindType meshFindType = MeshToTerrainFindType.gameObjects;
    public int meshLayer = 31;
    public int newTerrainCountX = 1;
    public int newTerrainCountY = 1;
    public int resolutionPerPatch = 16;
    public int smoothingFactor = 1;
    public List<Terrain> terrains = new List<Terrain>();
    public MeshToTerrainSelectTerrainType terrainType = MeshToTerrainSelectTerrainType.newTerrains;
    public MeshToTerrainTextureType textureType = MeshToTerrainTextureType.noTexture;
    public Color textureEmptyColor = Color.white;
    public int textureHeight = 1024;
    public int textureWidth = 1024;
    public bool useHeightmapSmoothing = true;

    private Vector2 scrollPos = Vector2.zero;
    private bool showMeshes = true;
    private bool showTerrains = true;
    private bool showTextures = true;

    public MeshToTerrainPrefs()
    {
        Load();
    }

    private static int LimitPowTwo(int val, int min = 32, int max = 4096)
    {
        return Mathf.Clamp(Mathf.ClosestPowerOfTwo(val), min, max);
    }

    private void Load()
    {
        baseMapResolution = LoadPref("BaseMapRes", 1024);
        boundsGameObject = LoadPrefGameObject("BoundsGameObject", null);
        boundsType = (MeshToTerrainBounds) LoadPref("BoundsType", (int) MeshToTerrainBounds.autoDetect);
        detailResolution = LoadPref("DetailMapRes", 1024);
        direction = (MeshToTerrainDirection)LoadPref("Direction", (int)MeshToTerrainDirection.normal);
        heightmapResolution = LoadPref("HeightMapRes", 128);
        meshes = LoadPref("Meshes", new List<GameObject>());
        meshFindType = (MeshToTerrainFindType)LoadPref("MeshFindType", (int)MeshToTerrainFindType.gameObjects);
        meshLayer = LoadPref("MeshLayer", 31);
        newTerrainCountX = LoadPref("CountX", 1);
        newTerrainCountY = LoadPref("CountY", 1);
        resolutionPerPatch = LoadPref("ResPerPath", 16);
        showMeshes = LoadPref("ShowMeshes", true);
        showTerrains = LoadPref("ShowTerrains", true);
        showTextures = LoadPref("ShowTextures", true);
        smoothingFactor = LoadPref("SmoothingFactor", 1);
        terrains = LoadPref("Terrains", new List<Terrain>());
        terrainType = (MeshToTerrainSelectTerrainType)LoadPref("TerrainType", (int)MeshToTerrainSelectTerrainType.newTerrains);
        textureType = (MeshToTerrainTextureType)LoadPref("TextureType", (int)MeshToTerrainTextureType.noTexture);
        textureEmptyColor = LoadPref("TextureEmptyColor", Color.white);
        textureHeight = LoadPref("TextureHeight", 1024);
        textureWidth = LoadPref("TextureWidth", 1024);
        useHeightmapSmoothing = LoadPref("UseHeightmapSmoothing", true);
    }

    private bool LoadPref(string id, bool defVal)
    {
        string key = appKey + id;
        return EditorPrefs.HasKey(key) ? EditorPrefs.GetBool(key) : defVal;
    }

    private Color LoadPref(string id, Color defVal)
    {
        return new Color(LoadPref(id + "_R", defVal.r), LoadPref(id + "_G", defVal.g), LoadPref(id + "_B", defVal.b), LoadPref(id + "_A", defVal.a));
    }

    private float LoadPref(string id, float defVal)
    {
        string key = appKey + id;
        return EditorPrefs.HasKey(key) ? EditorPrefs.GetFloat(key) : defVal;
    }

    private int LoadPref(string id, int defVal)
    {
        string key = appKey + id;
        return EditorPrefs.HasKey(key) ? EditorPrefs.GetInt(key) : defVal;
    }

    private List<GameObject> LoadPref(string id, List<GameObject> defVals)
    {
        string key = appKey + id + "_Count";
        if (EditorPrefs.HasKey(key))
        {
            int count = EditorPrefs.GetInt(appKey + id + "_Count");
            List<GameObject> retVal = new List<GameObject>();
            for (int i = 0; i < count; i++) retVal.Add(EditorUtility.InstanceIDToObject(EditorPrefs.GetInt(appKey + id + "_" + i)) as GameObject);
            return retVal;
        }
        return defVals;
    }

    private List<Terrain> LoadPref(string id, List<Terrain> defVals)
    {
        string key = appKey + id + "_Count";
        if (EditorPrefs.HasKey(key))
        {
            int count = EditorPrefs.GetInt(appKey + id + "_Count");
            List<Terrain> retVal = new List<Terrain>();
            for (int i = 0; i < count; i++) retVal.Add(EditorUtility.InstanceIDToObject(EditorPrefs.GetInt(appKey + id + "_" + i)) as Terrain);
            return retVal;
        }
        return defVals;
    }

    private GameObject LoadPrefGameObject(string id, GameObject defVal)
    {
        int goID = LoadPref(id, -1);
        if (goID == -1) return defVal;
        GameObject go = EditorUtility.InstanceIDToObject(goID) as GameObject;
        return go ?? defVal;
    }

    public void OnGUI()
    {
        OnToolbarGUI();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        OnMeshesGUI();
        OnTerrainsGUI();
        OnTexturesGUI();

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Start", GUILayout.Height(40))) MeshToTerrain.phase = MeshToTerrainPhase.prepare;
    }

    private void OnMeshesGUI()
    {
        showMeshes = EditorGUILayout.Foldout(showMeshes, "Meshes: ");
        if (showMeshes)
        {
            meshFindType = (MeshToTerrainFindType)EditorGUILayout.EnumPopup("Mesh select type: ", meshFindType);

            if (meshFindType == MeshToTerrainFindType.gameObjects) OnMeshesGUIGameObjects();
            else if (meshFindType == MeshToTerrainFindType.layers) meshLayer = EditorGUILayout.LayerField("Layer: ", meshLayer);

            direction = (MeshToTerrainDirection) EditorGUILayout.EnumPopup("Direction: ", direction);
            if (direction == MeshToTerrainDirection.reversed) GUILayout.Label("Use the reverse direction, if that model has inverted the normal.");
            EditorGUILayout.Space();
        }
    }

    private void OnMeshesGUIGameObjects()
    {
        meshes.RemoveAll(m => m == null);
        for (int i = 0; i < meshes.Count; i++) meshes[i] = (GameObject)EditorGUILayout.ObjectField(meshes[i], typeof(GameObject), true);
        meshes.RemoveAll(m => m == null);

        GameObject newMesh = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), true);
        if (newMesh != null)
        {
            if (!meshes.Contains(newMesh)) meshes.Add(newMesh);
            else EditorUtility.DisplayDialog("Warning", "GameObject already added", "OK");
        }
    }

    private static void OnProductPage()
    {
        Process.Start("http://infinity-code.com/products/mesh-to-terrain");
    }

    private static void OnSendMail()
    {
        Process.Start("mailto:support@infinity-code.com?subject=Mesh to Terrain");
    }

    private void OnTerrainsGUI()
    {
        showTerrains = EditorGUILayout.Foldout(showTerrains, "Terrains: ");
        if (showTerrains)
        {
            terrainType = (MeshToTerrainSelectTerrainType)EditorGUILayout.EnumPopup("Type: ", terrainType);
            if (terrainType == MeshToTerrainSelectTerrainType.existTerrains) OnTerrainsGUIExists();
            else OnTerrainsGUINew();
            useHeightmapSmoothing = GUILayout.Toggle(useHeightmapSmoothing, "Use smoothing of height maps.");

            if (useHeightmapSmoothing)
            {
                smoothingFactor = EditorGUILayout.IntField("Smoothing factor: ", smoothingFactor);
                if (smoothingFactor < 1) smoothingFactor = 1;
                else if (smoothingFactor > 128) smoothingFactor = 128;
            }

            EditorGUILayout.Space();
        }
    }

    private void OnTerrainsGUINew()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Count terrains. X: ", GUILayout.ExpandWidth(false));
        newTerrainCountX = Mathf.Max(EditorGUILayout.IntField(newTerrainCountX, GUILayout.ExpandWidth(false)), 1);
        GUILayout.Label("Y: ", GUILayout.ExpandWidth(false));
        newTerrainCountY = Mathf.Max(EditorGUILayout.IntField(newTerrainCountY, GUILayout.ExpandWidth(false)), 1);
        GUILayout.EndHorizontal();

        boundsType = (MeshToTerrainBounds)EditorGUILayout.EnumPopup("Bounds: ", boundsType);
        if (boundsType == MeshToTerrainBounds.fromGameobject) boundsGameObject = (GameObject)EditorGUILayout.ObjectField("Bounds GameObject: ", boundsGameObject, typeof(GameObject), true);

        detailResolution = LimitPowTwo(EditorGUILayout.IntField("Detail Resolution", detailResolution));
        resolutionPerPatch = LimitPowTwo(EditorGUILayout.IntField("Resolution Per Patch", resolutionPerPatch), 4);
        baseMapResolution = LimitPowTwo(EditorGUILayout.IntField("Base Map Resolution", baseMapResolution));
        heightmapResolution = LimitPowTwo(EditorGUILayout.IntField("Height Map Resolution", heightmapResolution));
    }

    private void OnTerrainsGUIExists()
    {
        for (int i = 0; i < terrains.Count; i++) terrains[i] = (Terrain)EditorGUILayout.ObjectField(terrains[i], typeof(Terrain), true);
        terrains.RemoveAll(t => t == null);

        Terrain newTerrain = (Terrain)EditorGUILayout.ObjectField(null, typeof(Terrain), true);
        if (newTerrain != null)
        {
            if (!terrains.Contains(newTerrain)) terrains.Add(newTerrain);
            else EditorUtility.DisplayDialog("Warning", "Terrain already added", "OK");
        }
    }

    private void OnTexturesGUI()
    {
        showTextures = EditorGUILayout.Foldout(showTextures, "Textures: ");
        if (showTextures)
        {
            textureType = (MeshToTerrainTextureType)EditorGUILayout.EnumPopup("Type: ", textureType);

            if (textureType == MeshToTerrainTextureType.bakeMainTextures)
            {
                textureWidth = LimitPowTwo(EditorGUILayout.IntField("Width: ", textureWidth));
                textureHeight = LimitPowTwo(EditorGUILayout.IntField("Height: ", textureHeight));
                textureEmptyColor = EditorGUILayout.ColorField("Empty color: ", textureEmptyColor);
            }

            EditorGUILayout.Space();
        }
    }

    private static void OnToolbarGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(EditorStyles.toolbarButton);

        GUILayout.BeginHorizontal();
        GUILayout.Label("", buttonStyle);

        if (GUILayout.Button("Support", buttonStyle, GUILayout.ExpandWidth(false)))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Send mail"), false, OnSendMail);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Open product page"), false, OnProductPage);
            menu.AddItem(new GUIContent("View online documentation"), false, OnViewDocs);
            menu.ShowAsContext();
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
    }

    private static void OnViewDocs()
    {
        Process.Start("http://infinity-code.com/docs/mesh-terrain");
    }

    public void Save()
    {
        SetPref("BaseMapRes", baseMapResolution);
        SetPref("BoundsGameObject", boundsGameObject);
        SetPref("BoundsType", (int)boundsType);
        SetPref("DetailMapRes", detailResolution);
        SetPref("HeightMapRes", heightmapResolution);
        SetPref("Meshes", meshes);
        SetPref("MeshFindType", (int)meshFindType);
        SetPref("MeshLayer", meshLayer);
        SetPref("CountX", newTerrainCountX);
        SetPref("CountY", newTerrainCountY);
        SetPref("ResPerPath", resolutionPerPatch);
        SetPref("ShowMeshes", showMeshes);
        SetPref("ShowTerrains", showTerrains);
        SetPref("ShowTextures", showTextures);
        SetPref("SmoothingFactor", smoothingFactor);
        SetPref("Terrains", terrains);
        SetPref("TerrainType", (int)terrainType);
        SetPref("TextureType", (int)textureType);
        SetPref("TextureEmptyColor", textureEmptyColor);
        SetPref("TextureHeight", textureHeight);
        SetPref("TextureWidth", textureWidth);
        SetPref("Direction", (int)direction);
        SetPref("UseHeightmapSmoothing", useHeightmapSmoothing);
    }

    private void SetPref(string id, bool val)
    {
        EditorPrefs.SetBool(appKey + id, val);
    }

    private void SetPref(string id, Color val)
    {
        SetPref(id + "_R", val.r);
        SetPref(id + "_G", val.g);
        SetPref(id + "_B", val.b);
        SetPref(id + "_A", val.a);
    }

    private void SetPref(string id, float val)
    {
        EditorPrefs.SetFloat(appKey + id, val);
    }

    private void SetPref(string id, int val)
    {
        EditorPrefs.SetInt(appKey + id, val);
    }

    private void SetPref(string id, List<GameObject> vals)
    {
        if (vals != null)
        {
            EditorPrefs.SetInt(appKey + id + "_Count", vals.Count);

            for (int i = 0; i < vals.Count; i++)
            {
                Object val = vals[i];
                if (val != null) EditorPrefs.SetInt(appKey + id + "_" + i, val.GetInstanceID());
            }
        }
        else EditorPrefs.SetInt(appKey + id + "_Count", 0);
    }

    private void SetPref(string id, List<Terrain> vals)
    {
        if (vals != null)
        {
            EditorPrefs.SetInt(appKey + id + "_Count", vals.Count);

            for (int i = 0; i < vals.Count; i++)
            {
                Object val = vals[i];
                if (val != null) EditorPrefs.SetInt(appKey + id + "_" + i, val.GetInstanceID());
            }
        }
        else EditorPrefs.SetInt(appKey + id + "_Count", 0);
    }

    private void SetPref(string id, Object val)
    {
        if (val != null) EditorPrefs.SetInt(appKey + id, val.GetInstanceID());
    }
}

public enum MeshToTerrainDirection
{
    normal,
    reversed
}