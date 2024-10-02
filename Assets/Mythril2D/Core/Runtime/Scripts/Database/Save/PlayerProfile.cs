using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Save + nameof(PlayerProfile))]
    public class PlayerProfile : DatabaseEntry
    {
        [Header("References")]
        public GameObject prefab;
    }
}
