using UnityEngine;

namespace ProjectFiles.Scripts.Characters.Player
{
    public interface IControlPlayer
    {
        // Properties
        public bool IsMoveCharacterActive { set; }
        public Vector3 MoveDirection { set; }
        public float MoveSpeedScale { set; }
    }
}