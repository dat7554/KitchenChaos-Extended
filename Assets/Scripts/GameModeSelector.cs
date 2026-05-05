public static class GameModeSelector
{
    private static GameModeSO _selectedGameModeSO;

    public static void SetGameModeSO(GameModeSO mode)
    {
        _selectedGameModeSO = mode;
    }

    public static GameModeSO GetGameModeSO()
    {
        return _selectedGameModeSO;
    }
}
