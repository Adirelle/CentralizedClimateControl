namespace CentralizedClimateControl
{
    public enum AirTypePriority
    {
        Hot = 0,
        Cold = 1,
        Frozen = 2,
        Auto = 3
    }

    public static class AirTypePriorityExtensions
    {
        private const string SwitchPipeAutoKey = "CentralizedClimateControl.Command.SwitchPipe.Auto";
        private const string SwitchPipeRedKey = "CentralizedClimateControl.Command.SwitchPipe.Red";
        private const string SwitchPipeBlueKey = "CentralizedClimateControl.Command.SwitchPipe.Blue";
        private const string SwitchPipeCyanKey = "CentralizedClimateControl.Command.SwitchPipe.Cyan";

        public static string CommandLabelKey(this AirTypePriority priority)
        {
            return priority switch {
                AirTypePriority.Hot => SwitchPipeRedKey,
                AirTypePriority.Cold => SwitchPipeBlueKey,
                AirTypePriority.Frozen => SwitchPipeCyanKey,
                _ => SwitchPipeAutoKey,
            };
        }

        public static string CommandIconName(this AirTypePriority priority)
        {
            return priority switch {
                AirTypePriority.Hot => "UI/PipeSelect_Red",
                AirTypePriority.Cold => "UI/PipeSelect_Blue",
                AirTypePriority.Frozen => "UI/PipeSelect_Cyan",
                _ => "UI/PipeSelect_Auto",
            };
        }

        public static AirTypePriority Next(this AirTypePriority priority)
        {
            return priority switch {
                AirTypePriority.Hot => AirTypePriority.Cold,
                AirTypePriority.Cold => AirTypePriority.Frozen,
                AirTypePriority.Frozen => AirTypePriority.Auto,
                _ => AirTypePriority.Hot,
            };
        }
    }
}
