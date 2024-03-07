using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;


[CustomEditor(typeof(ScriptableWeapon))]
public class WeaponBarrelsInspector : Editor
{
    ScriptableWeapon targetScript;

    void OnEnable()
    {
        targetScript = target as ScriptableWeapon;
    }

    public override void OnInspectorGUI()
    {
        //     public EnumDamageType DamageType;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Itemname");
        targetScript.ItemName = EditorGUILayout.TextField(targetScript.ItemName);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        ;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Description");
        targetScript.Description = EditorGUILayout.TextArea(targetScript.Description, GUILayout.Height(50));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Price");
        targetScript.Price = EditorGUILayout.IntField(targetScript.Price);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Is item illigal?");
        targetScript.IsIllegal = EditorGUILayout.Toggle(targetScript.IsIllegal);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        //
        GUILayout.Label("Image");
        EditorGUILayout.BeginHorizontal();
        targetScript.Image = (Sprite)EditorGUI.ObjectField(new Rect(20, 210, 50, 50), targetScript.Image, typeof(Sprite), false);
        EditorGUILayout.EndHorizontal();
        //

        EditorGUILayout.Space(80);


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Damage");
        targetScript.Damage = EditorGUILayout.FloatField(targetScript.Damage);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Firerate");
        targetScript.FireRate = EditorGUILayout.FloatField(targetScript.FireRate);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        targetScript.DamageType = (EnumDamageType)EditorGUILayout.EnumPopup("Type of Damage", targetScript.DamageType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Bullet Prefab");
        targetScript.Bullet = (GameObject)EditorGUILayout.ObjectField(targetScript.Bullet, typeof(GameObject), true);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        

        GUILayout.Label("Barrels");
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.Barrels.Length; i += 3)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(10));
            for (int x = 0; x < 3; x++)
            {

                targetScript.Barrels[i+x] = EditorGUILayout.Toggle(targetScript.Barrels[i+x]);
            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndHorizontal();
    }
}
