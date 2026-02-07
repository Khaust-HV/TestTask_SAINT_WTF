using UnityEngine;

namespace ProjectFiles.Scripts.Characters
{
    public interface IContolPlayer
    {
        // Properties
        public bool IsMoveCharacterActive { set; }
        public Vector3 MoveDirection { set; }
        public float MoveSpeedScale { set; }
    }
}