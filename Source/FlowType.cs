namespace CentralizedClimateControl
{
    public enum FlowType : byte
    {
        None = 0x00,
        Red = 0x01,
        Blue = 0x02,
        Cyan = 0x4,
        Any = Red | Blue | Cyan,
    }
}
