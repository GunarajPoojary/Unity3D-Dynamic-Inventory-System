public struct ItemStack
{

}
public class InventoryItem
{
    private SOItemConfig _itemConfig;
    private int _id;

    public SOItemConfig ItemConfig => _itemConfig;
    public int ID => _id;

    public InventoryItem(SOItemConfig config, int id)
    {
        Set(config, id);
    }

    public void Set(SOItemConfig config, int id)
    {
        _id = id;
        _itemConfig = config;
    }
}