using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardScriptableObject))]
public class CardScriptableObjectEditor : Editor
{
    private SerializedProperty additionalEffectProp;

    private void OnEnable()
    {
        // リスト型プロパティ（AdditionalEffect）を取得
        additionalEffectProp = serializedObject.FindProperty("additionalEffect");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 共通プロパティの描画
        // EditorGUILayout.PropertyField(serializedObject.FindProperty("playCostAffection"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playActionPoints"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardType"));

        var useReqProp = serializedObject.FindProperty("useRequirmentPlayerParameter");
        EditorGUILayout.PropertyField(useReqProp);

        if (useReqProp.boolValue)
        {
            var paramProp = serializedObject.FindProperty("requirmentPlayerParameter");
            if (paramProp != null)
            {
                EditorGUILayout.PropertyField(paramProp, true);
            }
            else
            {
                EditorGUILayout.HelpBox("requirmentPlayerParameter is null or could not be found.", MessageType.Warning);
            }
        }

        // AdditionalEffect の手動描画
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Additional Effects", EditorStyles.boldLabel);

        if (additionalEffectProp != null)
        {
            for (int i = 0; i < additionalEffectProp.arraySize; i++)
            {
                var element = additionalEffectProp.GetArrayElementAtIndex(i);
                if (element.managedReferenceValue == null) continue;

                var effectObj = element.managedReferenceValue as AdditionalEffect;
                if (effectObj != null)
                {
                    EditorGUILayout.BeginVertical("box");
                    effectObj.effectName = EditorGUILayout.TextField("Effect Name", effectObj.effectName);
                    EditorGUILayout.LabelField("Type", effectObj.GetType().Name);

                    if (GUILayout.Button("Remove"))
                    {
                        additionalEffectProp.DeleteArrayElementAtIndex(i);
                        break;
                    }

                    EditorGUILayout.EndVertical();
                }
            }

            if (GUILayout.Button("Add CostBypass"))
            {
                additionalEffectProp.arraySize++;
                var newElement = additionalEffectProp.GetArrayElementAtIndex(additionalEffectProp.arraySize - 1);
                newElement.managedReferenceValue = new CostBypassCard
                {
                    effectName = "Cost Bypass"
                };
            }
        }

        // カードタイプごとの個別フィールド描画
        var cardTypeProp = serializedObject.FindProperty("cardType");
        var cardType = (CardScriptableObject.cardTypes)cardTypeProp.enumValueIndex;

        switch (cardType)
        {
            case CardScriptableObject.cardTypes.Talk:
                break;
            case CardScriptableObject.cardTypes.Comment:
                break;
            case CardScriptableObject.cardTypes.Action:
                break;
            case CardScriptableObject.cardTypes.Psychic:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("addParameterNum"));
                break;
            case CardScriptableObject.cardTypes.Special:
                break;
            case CardScriptableObject.cardTypes.Confession:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}