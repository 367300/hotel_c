namespace Task1.HotelRooms;

/// <summary>
/// Состояние комнаты отеля
/// </summary>
public enum HotelRoomState
{
    /// <summary>
    /// Неизвестно
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// Занято
    /// </summary>
    Busy = 1,
    /// <summary>
    /// Свободно
    /// </summary>
    Free = 2,
    /// <summary>
    /// Забронировано
    /// </summary>
    Booked = 3
}