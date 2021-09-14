using DG.Tweening;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField]private ItemObject worldItemObject;
    public int ItemId;
    public int Amount;
    public string Name;

    private float shakeDuration = 0.3f;
    private float shakeStrength = 0.3f;
    
    //Количество ресурса за удар
    public int AmountMultiplayerByDurability;
    //Количестве ударов
    public int Durability;

    private void Start()
    {
        AmountMultiplayerByDurability = Random.Range(3,7);
        Durability = Random.Range(1,5);
        ItemId = worldItemObject.Data.ID;
        Name = worldItemObject.Data.Name;
        Amount = Durability * AmountMultiplayerByDurability;
    }

    private void StartShakeAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOShakePosition(shakeDuration,shakeStrength));
    }
    
    public int GiveResources()
    {
        int returnNumber = Amount / Durability;
        Amount -= returnNumber;
        Durability --;
        if (Durability == 0)
        {
            Destroy(gameObject);
            return returnNumber;
        }
        StartShakeAnimation();
        return returnNumber;
    }
}
