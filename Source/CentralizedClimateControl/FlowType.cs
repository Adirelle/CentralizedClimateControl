namespace CentralizedClimateControl
{
    public enum FlowType : byte
    {
        None = 0x00,
        Hot = 0x01,
        Cold = 0x02,
        Frozen = 0x4,
        Any = Hot | Cold | Frozen,
    }
}
