
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_ItemLists", menuName = "Scriptable Objects/Item/Item List")]
public class SO_ItemLists : ScriptableObject
{
    [SerializeField]
    public List<ItemDetails> itemDetails;
}
