#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Word))]
public class WordPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty hasCluesProp = property.FindPropertyRelative("hasClues");
        bool hasClues = hasCluesProp.boolValue;

        float line = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;

        float height = line * 2 + pad * 2;
        if (hasClues)
        {
            SerializedProperty cluesProp = property.FindPropertyRelative("clues");
            height += EditorGUI.GetPropertyHeight(cluesProp, true) + line + pad;
        }
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var wordProp = property.FindPropertyRelative("word");
        var hasDataProp = property.FindPropertyRelative("hasClues");
        var cluesProp = property.FindPropertyRelative("clues");
        var cluesUnlockedProp = property.FindPropertyRelative("cluesUnlocked");

        float line = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;
        var r = new Rect(position.x, position.y, position.width, line);

        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(r, wordProp, new GUIContent("Word"));

        r.y += line + pad;
        EditorGUI.PropertyField(r, hasDataProp, new GUIContent("Has Clues"));

        if (hasDataProp.boolValue)
        {
            EditorGUI.indentLevel++;

            r.y += line + pad;
            EditorGUI.PropertyField(r, cluesProp, new GUIContent("Clues"));

            r.y += EditorGUI.GetPropertyHeight(cluesProp, true) + pad;
            EditorGUI.PropertyField(r, cluesUnlockedProp, new GUIContent("Unlocked"));

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}

[CustomEditor(typeof(SentenceData))]
public class SentenceAssetEditor : Editor
{
    SerializedProperty sentenceProp;
    SerializedProperty wordsProp;
    SerializedProperty manaProp;

    void OnEnable()
    {
        sentenceProp = serializedObject.FindProperty("sentence");
        wordsProp = serializedObject.FindProperty("words");
        manaProp = serializedObject.FindProperty("sentenceMana");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(sentenceProp);

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Split Sentence"))
            {
                foreach (Object o in targets)
                {
                    var sa = (SentenceData)o;
                    Undo.RecordObject(sa, "Split Sentence");
                    sa.SentenceToWords();
                    EditorUtility.SetDirty(sa);
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(wordsProp, includeChildren: true);
        EditorGUILayout.PropertyField(manaProp);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
