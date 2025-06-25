using Task1.HotelRooms;

namespace Task1.Simulation;

public class Person
{
    public Person(long balance, RoomCategory roomCategory, BookRoomInfo bookRoomInfo)
    {
        
    }

    public Person(long balance, RoomCategory roomCategory, TimeSpan liveDuration)
    {
        
    }

    public void Print()
    {

    }

    public void Print(string message)
    {

    }
    public long Balance { get; set; }
    public RoomCategory DesiredRoomType { get; set; }
    public PersonWish PersonWish { get; set; }
    /// <summary>
    /// Заполняется при PersonWish == BookRoom
    /// </summary>
    public BookRoomInfo BookRoomInfo { get; set; }
    /// <summary>
    /// Заполняется при PersonWish == EnterRoom
    /// </summary>
    public TimeSpan LiveDuration { get; set; }
}