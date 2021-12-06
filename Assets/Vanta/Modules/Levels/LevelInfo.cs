using System;
using UnityEngine;



namespace Vanta.Levels
{

    [Serializable]
    public class LevelInfo
    {
        public string id = Guid.NewGuid().ToString();
        public string name = "Level";
        public string title = "Level Title";
        public string description = "Yet another level";
        public Sprite thumbnail;
        public int viewCount;
        public GameObject prefab = null;
            
        public LevelInfo Clone()
        {
            var clone = new LevelInfo();
            clone.id = id;
            clone.name = name;
            clone.title = title;
            clone.description = description;
            clone.thumbnail = thumbnail;
            clone.viewCount = viewCount;
            clone.prefab = prefab;
                
            return clone;
        }
            
    }

}
