namespace Task1.HotelRooms;

public class VipRoom : BaseHotelRoom
{
    public override RoomCategory RoomCategory => RoomCategory.Vip;
    public VipRoom(int roomId, long price) : base(roomId, price) { }
}