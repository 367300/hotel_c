namespace Task1.HotelRooms;

public interface IHotelRoom
{
    public int RoomId { get; }
    public RoomCategory RoomCategory { get; }
    public long Price { get; }
    public HotelRoomState State { get; }
    /// <summary>
    /// Забронировать номер
    /// </summary>
    /// <returns></returns>
    public void Book(DateTime startLive, DateTime endLive);
    /// <summary>
    /// Заселение в отель
    /// </summary>
    /// <returns></returns>
    public void RentHotelRoom(TimeSpan liveDuration, int userId = -1);
    /// <summary>
    /// Выселение из комнаты отеля
    /// </summary>
    /// <returns></returns>
    public void UnRentHotelRoom();

}