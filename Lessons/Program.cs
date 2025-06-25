using Task1.HotelRooms;
using Task1.Simulation;
using Task1.Hotel;
using Timer = System.Timers.Timer;

namespace Task1;

public class Program
{
    public static void Main()
    {
        var hotel = new Task1.Hotel.Hotel();
        hotel.PrintInfo();
        hotel.MoveIntoBetterRoom(RoomCategory.Standard, 10000, TimeSpan.FromDays(23));
    }

    private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
