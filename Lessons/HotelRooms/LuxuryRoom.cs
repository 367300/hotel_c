namespace Task1.HotelRooms;

public class LuxuryRoom : BaseHotelRoom
{
    public override RoomCategory RoomCategory => RoomCategory.Luxury;
    public LuxuryRoom(int roomId, long price) : base(roomId, price) { }
}