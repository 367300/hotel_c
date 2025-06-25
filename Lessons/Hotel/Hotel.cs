using System.Diagnostics.Metrics;
using Task1.HotelRooms;

namespace Task1.Hotel;

public class Hotel
{
    private Dictionary<int, IHotelRoom> _rooms = new();

    public Hotel()
    {
        GenerateRooms();
    }

    public void PrintInfo()
    {
        Console.WriteLine("Приветсвую в отеле, DmitryRoom!\n" +
                          "В распоряжении есть 3 вида номеров:\n" +
                          "1. Стандарт от 1000 до 4000\n" +
                          "2. Лакшери ото 4000 до 7000\n" +
                          "3. VIP от 7000 до 10000");
    }

    public void MoveIntoBetterRoom(RoomCategory roomCategory, long heroBalance, TimeSpan liveDuration)
    {
        List<IHotelRoom> suitableNumbers = [];
        foreach (var hotelRoom in _rooms.Values)
        {
            if (hotelRoom.RoomCategory == roomCategory && 
                hotelRoom.Price <= heroBalance && 
                hotelRoom.State != HotelRoomState.Busy)
            {
                suitableNumbers.Add(hotelRoom);
            }
        }

        bool isSuccess = false;

        foreach (var suitableNumber in suitableNumbers)
        {
            if (suitableNumber.RentHotelRoom(liveDuration))
            {
                isSuccess = true;
                break;
            }
        }

        if (isSuccess)
        {
            Console.WriteLine($"[Hotel] Успешно заселились в комнату. Тип комнаты = {roomCategory} " +
                              $"Длительность проживания = {liveDuration}");
        }
        else
        {
            Console.WriteLine("[Hotel] Живу в подвале =(");
        }
    }

    public void BookRoom(RoomCategory roomCategory, long heroBalance, DateTime startLive, DateTime endLive)
    {
        List<IHotelRoom> suitableNumbers = [];
        foreach (var hotelRoom in _rooms.Values)
        {
            if (hotelRoom.RoomCategory == roomCategory &&
                hotelRoom.Price <= heroBalance)
            {
                suitableNumbers.Add(hotelRoom);
            }
        }

        bool isSuccess = false;

        foreach (var suitableNumber in suitableNumbers)
        {
            if (suitableNumber.Book(startLive, endLive))
            {
                isSuccess = true;
                break;
            }
        }

        if (isSuccess)
        {
            Console.WriteLine($"[Hotel] Успешно забронировали комнату. Тип комнаты = {roomCategory} " +
                              $"Время начала проживания = {startLive}" +
                              $"Время окончания проживания = {endLive}");
        }
        else
        {
            Console.WriteLine("[Hotel] Живу в подвале =(");
        }
    }

    private void GenerateRooms()
    {
        for (var i = 0; i < Random.Shared.Next(5, 20); i++)
        {
            var price = Random.Shared.Next(1000, 10_000);
            if (price < 4000)
            {
                _rooms.Add(i, new StandardRoom(i, price));
            }
            else if(price <= 7000)
            {
                _rooms.Add(i, new LuxuryRoom(i, price));
            }
            else
            {
                _rooms.Add(i, new VipRoom(i, price));
            }
        }
    }
}