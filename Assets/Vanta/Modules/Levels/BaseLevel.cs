using UnityEngine;
using Vanta.Characters;
using Vanta.Core;



namespace Vanta.Levels
{

    public partial interface ILevelDelegate
    {
        void Level_DidLoad(BaseLevel baseLevel);
        void Level_DidStart(BaseLevel baseLevel);
        void Level_DidSuccess(BaseLevel baseLevel, float score);
        void Level_DidFail(BaseLevel baseLevel);
    }

    public abstract class BaseLevel : VantaBehaviour
    {

        public enum Type
        {
            Normal
        }

        public Type _type = Type.Normal;
        [HideInInspector] private Type type => _type;

        
        public enum State
        {
            Unknown,
            Loading,
            Loaded,
            Started,
            Finishing,
            Succeed,
            Failed
        }

        protected State _state = State.Unknown;
        [HideInInspector] public State state => _state;

        protected LevelInfo _levelInfo;
        public LevelInfo levelInfo => _levelInfo;

        [HideInInspector] public virtual BaseCharacter player => _player;
        [SerializeField] protected BaseCharacter _player;

        public ILevelDelegate listener;



    #region Life Cycle

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            _state = State.Loading;
        }

    #endregion

    }

}
