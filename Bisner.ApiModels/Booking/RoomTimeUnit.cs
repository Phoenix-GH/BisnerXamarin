namespace Bisner.ApiModels.Booking
{
    public enum RoomSquareType
    {
        /// <summary>
        /// Square meters
        /// </summary>
        Meters = 0,

        /// <summary>
        /// Square feet
        /// </summary>
        Feet = 1,
    }
    public enum RoomType
    {
        /// <summary>
        /// Meeting room = default
        /// </summary>
        MeetingRoom = 0 ,

        /// <summary>
        /// Training room
        /// </summary>
        TrainingRoom = 1,

        /// <summary>
        /// Workspace
        /// </summary>
        Workplace = 2,

        /// <summary>
        /// Office / spreekkamer
        /// </summary>
        Office = 3
    }

    public enum RoomTimeUnit
    {
        /// <summary>
        /// Default 1 hour
        /// </summary>
        Hour = 0,

        /// <summary>
        /// 15 minute interval
        /// </summary>
        Minutes15 = 1,

        /// <summary>
        /// 30 minute interval
        /// </summary>
        Minutes30 = 2,

        /// <summary>
        /// 2 hour interval
        /// </summary>
        Hour2 = 3,

        /// <summary>
        /// 4 hour interval
        /// </summary>
        Hour4 = 4
        
    }
}