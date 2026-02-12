using ElementGame.Data;
using ElementGame.Save;

namespace ElementGame.Core
{
    public class GameContext
    {
        public Board Board;
        public int CurrentLevelIndex;

        public LevelDatabase LevelDatabase;
        public BoardGenerator BoardGenerator;
        public SaveSystem SaveSystem;
        public DestroySystem DestroySystem;
        public GravityAnimator GravityAnimator;
        public bool IsResolving;
    }
}