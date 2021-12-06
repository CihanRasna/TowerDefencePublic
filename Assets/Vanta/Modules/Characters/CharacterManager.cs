using System;
using JetBrains.Annotations;
using UnityEngine;
using Vanta.Core;



namespace Vanta.Characters
{
    
    public class CharacterManager : Singleton<CharacterManager>
    {

        public CharacterInfo.Dictionary characterInfo => _characterInfo;
        [HideInInspector] [SerializeField] private CharacterInfo.Dictionary _characterInfo;



    #region Life Cycle

        protected override void Awake()
        {
            base.Awake();
            
            CharacterInfo.Dictionary safeCharacterInfo = new CharacterInfo.Dictionary();
            foreach (CharacterType type in Enum.GetValues(typeof(CharacterType)))
            {
                var info = InfoForCharacter(type);
                if (info != null)
                {
                    safeCharacterInfo.Add(type, info);
                }
            }

            _characterInfo = safeCharacterInfo;
        }

    #endregion



    #region Public Methods
        
        public CharacterType? TypeForCharacter(CharacterInfo characterInfo)
        {
            foreach (var item in _characterInfo)
            {
                if (item.Value.id == characterInfo.id)
                {
                    return item.Key;
                }
            }

            return null;
        }
        
        [CanBeNull]
        public CharacterInfo InfoForCharacter(CharacterType characterType)
        {
            if (_characterInfo == null)
            {
                return null;
            }
            
            if (!_characterInfo.ContainsKey(characterType))
            {
                Debug.LogWarning(
                    Enum.GetName(typeof(CharacterType), characterType) +
                    " character doesn't contain any info!"
                );
                
                return null;
            }

            return _characterInfo[characterType];
        }

        public CharacterInfo InfoForCharacter(string characterId)
        {
            if (_characterInfo == null)
            {
                return null;
            }

            foreach (var item in _characterInfo)
            {
                if (item.Value.id == characterId)
                {
                    return item.Value;
                }
            }
            
            Debug.LogWarning("CharacterManager doesn't contain any characters with id of " + characterId + "!");
            return null;
        }
        
    #endregion

    }
    
}
