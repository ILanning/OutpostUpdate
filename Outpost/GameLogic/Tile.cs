namespace Outpost.GameLogic
{
    public struct Tile
    {
        public int ID;
        public bool Revealed;

        public Tile(int id)
        {
            ID = id;
            Revealed = true;
        }

        public Tile(int id, bool revealed)
        {
            ID = id;
            Revealed = revealed;
        }
    }
}
