using UnityEngine;

[CreateAssetMenu(menuName = "Decorate/DecorateBlock")]
public class DecorateBlock : ScriptableObject
{
    [SerializeField] EDecorateBlockSize decorateBlockSize;
    [SerializeField] Sprite[] sprites;

    public EDecorateBlockSize Size => decorateBlockSize;
    public Sprite[] Sprites => sprites;
}
public enum EDecorateBlockSize
{None ,Size_1x1, Size_1x2, Size_2x1, Size_2x2}