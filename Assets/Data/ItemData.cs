using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    //dinh nghia ra cac muc de dien thong tin
    [Header("Thong tin co ban")] 
    public string itemName;
    
    [TextArea(3, 10)]
    public string itemDescription;
    public Sprite itemSprite; //anh item

    [Min(0)] public int price;

    [Min(0)] public int damage;
    
    
}
