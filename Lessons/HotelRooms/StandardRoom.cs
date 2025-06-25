namespace Task1.HotelRooms;

public class StandardRoom : BaseHotelRoom
{
    public override RoomCategory RoomCategory => RoomCategory.Standard;
    public StandardRoom(int roomId, long price) : base(roomId, price) { }
}