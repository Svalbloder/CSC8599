using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Crop Data")]
public class CropData : ScriptableObject
{
    public int _index;//ÐòºÅ
    public string _name;//Ãû×Ö
    public Sprite _image;//Í¼Æ¬

    [TextArea] public string _info;//ÐÅÏ¢

    public Sprite _fruit;
    public Sprite _seedSprite;
    public Sprite _seedlingModel;
    public Sprite _plantingModel;
    public Sprite _halfGrownModel;
    public Sprite _matureModel;

    public float _fert;
    public float _organic;
    public float _condition;
    public float _water;
    public int _cost;
}