
using UnityEngine;

public class Item : MonoBehaviour
{
    [ItemCodeDesc]
    [SerializeField]
    private int _itemCode;
    private SpriteRenderer sr;
    public int itemCode {get {return _itemCode;} set {_itemCode = value;}}

    void Awake(){
        sr = GetComponent<SpriteRenderer>();
    }

    void Start(){
        if(itemCode != 0){
            Init(itemCode);
        }
    }

    public void Init(int itemCodeParam){

    }
}
