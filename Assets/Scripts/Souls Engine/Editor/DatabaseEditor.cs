using System;
using UnityEngine;
using UnityEditor;
using SoulsEngine.Core;
using SoulsEngine.Utility;
using SoulsEngine.Utility.Combat;

public class DatabaseEditor : EditorWindow
{
    public ItemDatabase itemDB;
    private int _selectedItem = -1;
    private int selectedButton;

    private Vector2 scrollPos;

    public int idField;
    public string nameField;
    public string descriptionField;
    public ItemType typeField;
    public Sprite iconField;
    public int stackSizeField;

    public WeaponType weaponTypeField;
    public float damageField;
    public int durabilityField;
    
    private string itemSearchString = "";

    public int SelectedItem
    {
        get { return _selectedItem; }
        set { _selectedItem = Mathf.Clamp(value, -1, Int32.MaxValue); }
    }

    [MenuItem("Souls Engine/Database Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DatabaseEditor));
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal("box", GUILayout.Height(50f));
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Label("Item Database Editor", EditorStyles.largeLabel);

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal("box", GUILayout.Height(30f));
        DrawDatabaseArea();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal("box");
        DrawItemArea();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private void UpdateFieldData(int __id)
    {
        if (__id == -1)
        {
            nameField = null;
            descriptionField = null;
            typeField = ItemType.WEAPON;
            iconField = null;
            stackSizeField = 1;
        }
        else
        {
            var _item = itemDB.Database[__id];
            nameField = _item.ItemName;
            descriptionField = _item.ItemDescription;
            typeField = _item.ItemType;
            iconField = _item.ItemIcon;
            stackSizeField = _item.ItemStackSize;
        }
    }

    private void UpdateItemData()
    {
        if (SelectedItem >= 0)
        {
            var i = itemDB.Database[SelectedItem];
            i.ItemName = nameField;
            i.ItemDescription = descriptionField;
            i.ItemType = typeField;
            i.ItemIcon = iconField;
            i.ItemStackSize = stackSizeField;

            if (typeField == ItemType.WEAPON)
            {
                //update weapon type fields here
            }

            if (typeField == ItemType.SHIELD)
            {
                //update shield type fields here
            }

            if (typeField == ItemType.ARMOR)
            {
                //update armor type fields here
            }

            if (typeField == ItemType.CONSUMABLE)
            {
                //update consumable type fields here
            }
        }
    }

    private void CreateItemEntry()
    {
        var item = new Item()
        {
            ItemName = "NEW ITEM"
        };

        itemDB.Database.Add(item);

        UpdateFieldData(itemDB.Database.Count - 1);
        SelectedItem = itemDB.Database.Count - 1;

        GUI.FocusControl(null);
        EditorUtility.SetDirty(itemDB);
    }

    private void DrawItemSearchPanel()
    {
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        itemSearchString = EditorGUILayout.TextField(itemSearchString, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        GUI.color = Color.green;
        if (GUILayout.Button("Create"))
            CreateItemEntry();
        GUI.color = Color.white;

        EditorGUILayout.EndHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, "box", GUILayout.ExpandHeight(true));

        if (itemDB != null)
        {
            for (int i = 0; i < itemDB.Database.Count; i++)
            {
                if (itemDB.Database[i].ItemName.ToLower().Contains(""))
                {
                    EditorGUILayout.BeginHorizontal();

                    if (i == SelectedItem)
                        GUI.color = Color.cyan;

                    if (GUILayout.Button(itemDB.Database[i].ItemName, "box", GUILayout.ExpandWidth(true)))
                    {
                        GUI.FocusControl(null);
                        UpdateFieldData(i);
                        SelectedItem = i;
                    }

                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.Width(25f)))
                    {
                        itemDB.Database.RemoveAt(i);
                        GUI.FocusControl(null);

                        if (i == SelectedItem)
                        {
                            SelectedItem--;
                            UpdateFieldData(SelectedItem);
                        }
                        
                        EditorUtility.SetDirty(itemDB);
                    }
                    GUI.color = Color.white;

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawItemEditorPanel()
    {
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));

        EditorGUILayout.LabelField("General", EditorStyles.largeLabel);

        if (itemDB != null && SelectedItem >= 0)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));

            nameField = EditorGUILayout.TextField("Name", nameField, GUILayout.ExpandWidth(true));
            descriptionField = EditorGUILayout.TextField("Description", descriptionField, GUILayout.ExpandWidth(true));
            typeField = (ItemType) EditorGUILayout.EnumPopup("Type", typeField, GUILayout.ExpandWidth(true));
            iconField = (Sprite) EditorGUILayout.ObjectField("Icon", iconField, typeof(Sprite), false, GUILayout.ExpandWidth(true));
            stackSizeField = EditorGUILayout.IntField("Stack size", stackSizeField, GUILayout.ExpandWidth(true));

            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                UpdateItemData();
                EditorUtility.SetDirty(itemDB);
            }
            
            if (itemDB.Database[SelectedItem] != null)
            {
                if (itemDB.Database[SelectedItem].ItemType == ItemType.WEAPON)
                {
                    EditorGUILayout.LabelField("Weapon Data");
                    
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));

                    weaponTypeField = (WeaponType) EditorGUILayout.EnumPopup("Weapon Type", weaponTypeField, GUILayout.ExpandWidth(true));
                    damageField = EditorGUILayout.FloatField("Damage", damageField, GUILayout.ExpandWidth(true));
                    durabilityField = EditorGUILayout.IntField("Durability", durabilityField, GUILayout.ExpandWidth(true));

                    EditorGUILayout.EndVertical();

                    if (EditorGUI.EndChangeCheck())
                    {
                        UpdateItemData();
                        EditorUtility.SetDirty(itemDB);
                    }
                }
            }
        }
        
        EditorGUILayout.EndHorizontal();
    }

    private void DrawItemArea()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        EditorGUILayout.BeginVertical(GUILayout.Width(250f));

        DrawItemSearchPanel();
        
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));

        DrawItemEditorPanel();
        
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawDatabaseArea()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        itemDB = (ItemDatabase)EditorGUILayout.ObjectField("Database", itemDB, typeof(ItemDatabase), true);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        EditorGUILayout.LabelField("Item count: " + (itemDB == null ? 0.ToString() : itemDB.Database.Count.ToString()));
        
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}