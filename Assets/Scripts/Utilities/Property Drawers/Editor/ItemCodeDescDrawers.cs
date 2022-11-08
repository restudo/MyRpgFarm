
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ItemCodeDescAttribute))]
public class ItemCodeDescDrawers : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Change the return property height to be double to cater for the additional item code description
        // that we will draw
        return EditorGUI.GetPropertyHeight(property) * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that prefabs override logic
        // works on the entire property
        EditorGUI.BeginProperty(position, label, property);


        if(property.propertyType == SerializedPropertyType.Integer){
            EditorGUI.BeginChangeCheck(); // start of check for changed values

            // Draw item code
            var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.intValue);

            // Draw item description
            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Description", GetItemDescription(property.intValue));

            // if item code value has change then set value to new value
            if(EditorGUI.EndChangeCheck()){
                property.intValue = newValue;
            }
        }


        EditorGUI.EndProperty();
    }

    string GetItemDescription(int itemCode){
        SO_ItemLists so_ItemLists;
        so_ItemLists = AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Assets/Item/so_ItemLists.asset", typeof(SO_ItemLists)) as SO_ItemLists;
    
        List<ItemDetails> itemDetailsList = so_ItemLists.itemDetails;
        ItemDetails itemDetails = itemDetailsList.Find(x => x.itemCode == itemCode);

        if(itemDetails != null){
            return itemDetails.itemDescription;
        }
        else{
            return "";
        }
    }
}

