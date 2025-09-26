using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class DialogueDataCreator : EditorWindow
{
    private string fileName = "NewDialogueData";
    private List<DialogueNode> nodes = new List<DialogueNode>();
    private ReorderableList nodeList;
    private string searchFilter = "";

    private DialogueData loadedData; 
    private Vector2 nodeListScroll, detailScroll;
    private int selectedNodeIndex = -1;

    [MenuItem("Tools/Dialogue/Dialogue Data Creator")]
    public static void ShowWindow()
    {
        GetWindow<DialogueDataCreator>("Dialogue Data Creator");
    }

    private void OnEnable()
    {
        SetupNodeList();
    }

    private void SetupNodeList()
    {
        nodeList = new ReorderableList(nodes, typeof(DialogueNode), true, true, true, true);

        nodeList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Dialogue Nodes");
        };

        nodeList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (index >= nodes.Count) return;
            var node = nodes[index];

            rect.y += 2;
            string previewText = string.IsNullOrEmpty(node.text) ? "" : node.text;
            if (previewText.Length > 15) previewText = previewText.Substring(0, 15) + "...";

            string choicePreview = (node.choices != null && node.choices.Length > 0)
                ? $" (선택지 {node.choices.Length})" : "";

            string displayText = $"[{index}] {node.speaker}: {previewText}{choicePreview}";

            if (!string.IsNullOrEmpty(searchFilter) && !displayText.Contains(searchFilter))
            {
                GUI.color = new Color(1f, 1f, 1f, 0.3f);
            }

            if (GUI.Button(rect, displayText, EditorStyles.miniButtonLeft))
            {
                selectedNodeIndex = index;
            }

            GUI.color = Color.white;
        };

        nodeList.onAddCallback = (ReorderableList list) =>
        {
            Undo.RecordObject(this, "Add Dialogue Node");
            nodes.Add(new DialogueNode
            {
                speaker = "NPC",
                text = "대사를 입력하세요",
                portrait = null,
                background = null,
                choices = new DialogueChoice[0],
                lineEvent = new DialogueEvent { eventType = DialogueEventType.None }
            });
        };

        nodeList.onRemoveCallback = (ReorderableList list) =>
        {
            if (list.index >= 0 && list.index < nodes.Count)
            {
                Undo.RecordObject(this, "Remove Dialogue Node");
                nodes.RemoveAt(list.index);
                if (selectedNodeIndex == list.index) selectedNodeIndex = -1;
            }
        };
    }

    void OnGUI()
    {
        DrawToolbar();
        GUILayout.Space(5);
        DrawSearchBar();
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        // 좌측: 노드 리스트
        EditorGUILayout.BeginVertical(GUILayout.Width(250));
        nodeListScroll = EditorGUILayout.BeginScrollView(nodeListScroll);
        nodeList.DoLayoutList();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        // 우측: 상세 정보
        EditorGUILayout.BeginVertical("box");
        detailScroll = EditorGUILayout.BeginScrollView(detailScroll);

        if (selectedNodeIndex >= 0 && selectedNodeIndex < nodes.Count)
        {
            DrawNodeDetail(nodes[selectedNodeIndex], selectedNodeIndex);
        }
        else
        {
            EditorGUILayout.HelpBox("노드를 선택하면 상세 정보가 표시됩니다.", MessageType.Info);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("New", EditorStyles.toolbarButton)) CreateNewData();
        if (GUILayout.Button("Load", EditorStyles.toolbarButton)) LoadDialogueData();
        if (GUILayout.Button("Save", EditorStyles.toolbarButton)) SaveDialogueData();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Export JSON", EditorStyles.toolbarButton)) ExportJson();
        if (GUILayout.Button("Import JSON", EditorStyles.toolbarButton)) ImportJson();

        EditorGUILayout.EndHorizontal();

        if (loadedData != null)
        {
            EditorGUILayout.HelpBox($"현재 불러온 데이터: {loadedData.name}", MessageType.Info);
        }
    }

    private void DrawSearchBar()
    {
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Search", GUILayout.Width(50));
        searchFilter = EditorGUILayout.TextField(searchFilter);
        if (GUILayout.Button("Clear", GUILayout.Width(60))) searchFilter = "";
        GUILayout.EndHorizontal();
    }

    private void DrawNodeDetail(DialogueNode node, int index)
    {
        EditorGUILayout.LabelField($"Node {index}", EditorStyles.boldLabel);
        GUILayout.Space(5);

        node.speaker = EditorGUILayout.TextField("Speaker", node.speaker);
        node.text = EditorGUILayout.TextArea(node.text, GUILayout.Height(60));

        node.portrait = (Sprite)EditorGUILayout.ObjectField("Portrait", node.portrait, typeof(Sprite), false);
        node.background = (Sprite)EditorGUILayout.ObjectField("Background", node.background, typeof(Sprite), false);

        GUILayout.Space(10);

        EditorGUILayout.LabelField("Line Event", EditorStyles.boldLabel);
        node.lineEvent.eventType = (DialogueEventType)EditorGUILayout.EnumPopup("Event Type", node.lineEvent.eventType);
        node.lineEvent.parameter = EditorGUILayout.TextField("Parameter", node.lineEvent.parameter);

        GUILayout.Space(10);

        EditorGUILayout.LabelField("Choices", EditorStyles.boldLabel);

        if (node.choices == null) node.choices = new DialogueChoice[0];

        for (int j = 0; j < node.choices.Length; j++)
        {
            EditorGUILayout.BeginVertical("box");
            node.choices[j].choiceText = EditorGUILayout.TextField("Choice Text", node.choices[j].choiceText);
            node.choices[j].nextDialogueIndex = EditorGUILayout.IntField("Next Dialogue Index", node.choices[j].nextDialogueIndex);

            node.choices[j].choiceEvent.eventType =
                (DialogueEventType)EditorGUILayout.EnumPopup("Choice Event", node.choices[j].choiceEvent.eventType);
            node.choices[j].choiceEvent.parameter =
                EditorGUILayout.TextField("Parameter", node.choices[j].choiceEvent.parameter);

            if (GUILayout.Button("Remove Choice", EditorStyles.miniButton))
            {
                var choiceList = new List<DialogueChoice>(node.choices);
                choiceList.RemoveAt(j);
                node.choices = choiceList.ToArray();
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Choice", EditorStyles.miniButton))
        {
            var choiceList = new List<DialogueChoice>(node.choices);
            choiceList.Add(new DialogueChoice
            {
                choiceText = "선택지",
                nextDialogueIndex = 0,
                choiceEvent = new DialogueEvent { eventType = DialogueEventType.None }
            });
            node.choices = choiceList.ToArray();
        }
    }

    // --- 데이터 관리 ---
    void CreateNewData()
    {
        loadedData = null;
        nodes.Clear();
        fileName = "NewDialogueData";
        SetupNodeList();
        EditorUtility.DisplayDialog("완료", "새 DialogueData 생성 준비 완료!", "확인");
    }

    void SaveDialogueData()
    {
        if (loadedData == null)
        {
            DialogueData data = ScriptableObject.CreateInstance<DialogueData>();
            data.lines = nodes.ToArray();

            string folderPath = "Assets/DialogueData";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string path = $"{folderPath}/{fileName}.asset";
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();

            loadedData = data;
            EditorUtility.DisplayDialog("완료", $"DialogueData '{fileName}' 생성 완료!", "확인");
        }
        else
        {
            Undo.RecordObject(loadedData, "Update DialogueData");
            loadedData.lines = nodes.ToArray();
            EditorUtility.SetDirty(loadedData);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("완료", $"'{loadedData.name}' 업데이트 완료!", "확인");
        }
    }

    void LoadDialogueData()
    {
        string path = EditorUtility.OpenFilePanel("Load DialogueData", "Assets/Dialogues", "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);
        DialogueData data = AssetDatabase.LoadAssetAtPath<DialogueData>(path);

        if (data != null)
        {
            loadedData = data;
            fileName = data.name;
            nodes = new List<DialogueNode>(data.lines);
            SetupNodeList();
            EditorUtility.DisplayDialog("로드 성공", $"DialogueData '{fileName}' 불러오기 완료!", "확인");
        }
        else
        {
            EditorUtility.DisplayDialog("로드 실패", "선택한 파일이 DialogueData가 아닙니다!", "확인");
        }
    }

    void ExportJson()
    {
        string json = JsonUtility.ToJson(new DialogueDataWrapper { lines = nodes.ToArray() }, true);
        string path = EditorUtility.SaveFilePanel("Export DialogueData as JSON", "", fileName, "json");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, json);
            EditorUtility.DisplayDialog("Export", "JSON 파일 저장 완료!", "확인");
        }
    }

    void ImportJson()
    {
        string path = EditorUtility.OpenFilePanel("Import DialogueData from JSON", "", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            DialogueDataWrapper wrapper = JsonUtility.FromJson<DialogueDataWrapper>(json);
            nodes = new List<DialogueNode>(wrapper.lines);
            SetupNodeList();
            EditorUtility.DisplayDialog("Import", "JSON 파일 불러오기 완료!", "확인");
        }
    }

    [System.Serializable]
    private class DialogueDataWrapper
    {
        public DialogueNode[] lines;
    }
}