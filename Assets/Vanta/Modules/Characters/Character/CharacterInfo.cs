using System;
using UnityEngine;
using Vanta.Core;



namespace Vanta.Characters
{
        
    [Serializable]
    public class CharacterInfo
    {
        public enum StoreCategory
        {
            Rare,
            Epic,
            Legend
        }

        public string id = Guid.NewGuid().ToString();
        public string name = null;
        public string title = null;
        public StoreCategory storeCategory = StoreCategory.Rare;
        public Sprite storeThumbnail;
        public int subscriberCount = 0;
        public GameObject prefab = null;

        public CharacterInfo Clone()
        {
            var clone = new CharacterInfo();
            clone.id = id;
            clone.name = name;
            clone.title = title;
            clone.storeCategory = storeCategory;
            clone.storeThumbnail = storeThumbnail;
            clone.subscriberCount = subscriberCount;
            clone.prefab = prefab;

            return clone;
        }
        
        [Serializable] public class Dictionary : SerializableDictionary<CharacterType, CharacterInfo> { }

    }
        
}