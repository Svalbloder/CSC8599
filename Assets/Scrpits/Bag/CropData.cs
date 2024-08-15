using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Crop Data")]
public class CropData : ScriptableObject
{
    public int _index;//���
    public string _name;//����
    public Sprite _image;//ͼƬ

    [TextArea] public string _info;//��Ϣ

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