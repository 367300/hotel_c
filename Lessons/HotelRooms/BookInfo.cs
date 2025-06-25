using Timer = System.Timers.Timer;

namespace Task1.HotelRooms;

public struct BookInfo
{
    public Timer BookTimer;
    public DateTime LiveStartTime;
    public DateTime LiveEndTime;
}