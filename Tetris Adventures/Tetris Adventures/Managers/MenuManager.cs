

namespace Tetris_Adventures.Managers
{
    public class MenuManager
    {
        public enum GameStates
        {
            Menu,
            HowToPlay,
            Game,
            Pause,
            Exit
        }

        public enum MenuOptions
        {
            Game = 0,
            HowToPlay = 1,
            Exit = 2,
        }

        public GameStates GameState { get; set; } 
        public GameStates LastGameState { get; set; }
        public double JumpTimeCheck { get; set; }
    }
}
