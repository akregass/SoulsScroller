public static class GameState{

    private static int _level;
    public static int Level
    {
        get { return _level; }
        set { _level = value; }
    }

    private static int _checkpoint;
    public static int Checkpoint
    {
        get { return _checkpoint; }
        set { _checkpoint = value; }
    }

}
