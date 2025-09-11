public static class ItemIDGenerator
{
    private static int _lastUsedID = 0;

    public static int GenerateID() => ++_lastUsedID;
}