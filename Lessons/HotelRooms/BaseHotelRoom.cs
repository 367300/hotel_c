using System.Security.Policy;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Task1.HotelRooms;

public abstract class BaseHotelRoom : IHotelRoom
{
    public int RoomId { get; }
    public abstract RoomCategory RoomCategory { get; }
    public long Price { get; }
    public HotelRoomState State { get; private set; }

    private int _currentLiveUserId;
    private Timer _liveTimer;
    private Dictionary<int ,BookInfo> _bookInfo = new();
    private const int MaxLiveDays = 24;

    protected BaseHotelRoom(int roomId, long price)
    {
        RoomId = roomId;
        Price = price;
        State = HotelRoomState.Free;
    }

    public void Book(DateTime startLive, DateTime endLive)
    {
        try
        {
            if (startLive <= DateTime.Now)
            {
                throw new ArgumentException("Дата начала проживания должна быть больше текущей даты.");
            }

            if (endLive <= startLive)
            {
                throw new ArgumentException("Дата окончания проживания должна быть больше даты начала.");
            }

            switch (State)
            {
                case HotelRoomState.Free:
                {
                    SetBookRecord(startLive, endLive);
                    return;
                }
                case HotelRoomState.Busy:
                case HotelRoomState.Booked:
                {
                    BookForBookedState(startLive, endLive);
                    return;
                }
            }
            throw new InvalidOperationException("Некорректное состояние комнаты для бронирования.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BaseHotelRoom] Ошибка при бронировании номера: {ex.Message}");
        }
    }

    private void BookForBookedState(DateTime startLive, DateTime endLive)
    {
        if (CheckResidenceInterval(startLive, endLive))
        {
            SetBookRecord(startLive, endLive);
        }
        else
        {
            Console.WriteLine($"[BaseHotelRoom] Данное окошко занято! Не удалось забронировать комнату! " +
                              $"RoomId = {RoomId}" +
                              $"Время начала жизни = {startLive}" +
                              $"Время окончания жизни в номере = {endLive}");
            throw new InvalidOperationException("Данное окошко занято! Не удалось забронировать комнату!");
        }
    }

    private bool CheckResidenceInterval(DateTime startLive, DateTime endLive)
    {
        foreach (var bookInfo in _bookInfo.Values)
        {
            //Проверяем входит ли наш промежуток времени в чью-то бронь
            if (bookInfo.LiveStartTime <= startLive && bookInfo.LiveEndTime >= startLive ||
                bookInfo.LiveStartTime <= endLive && bookInfo.LiveEndTime >= endLive)
            {
                return false;
            }

            //Проверяем входит ли в наш промежуток чья-то бронь
            if (startLive <= bookInfo.LiveStartTime && endLive >= bookInfo.LiveStartTime &&
                startLive <= bookInfo.LiveEndTime && endLive >= bookInfo.LiveEndTime)
            {
                return false;
            }
        }

        return true;
    }

    private void SetBookRecord(DateTime startLive, DateTime endLive)
    {
        var toStartTimeDuration = startLive - DateTime.Now;
        State = HotelRoomState.Booked;

        var userId = GenerateUserId();

        var bookTimer = new Timer(toStartTimeDuration);
        bookTimer.Elapsed += BookTimerOnElapsed;

        var bookInfo = new BookInfo
        {
            BookTimer = bookTimer,
            LiveStartTime = startLive,
            LiveEndTime = endLive
        };
        _bookInfo.Add(userId, bookInfo);
        Console.WriteLine($"[BaseHotelRoom] Успешно Забронировали номер для человека. " +
                          $"RoomId = {RoomId}" +
                          $"Время начала жизни = {startLive}" +
                          $"Время окончания жизни в номере = {endLive}");
        
        void BookTimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            RentHotelRoom(endLive - startLive, userId);
            bookTimer.Elapsed -= BookTimerOnElapsed;
        }
    }

    private int GenerateUserId()
    {
        var userId = 0;
        do
        {
            userId = Random.Shared.Next(50000);
        } while (_bookInfo.ContainsKey(userId));

        return userId;
    }

    public void RentHotelRoom(TimeSpan liveDuration, int userId = -1)
    {
        if (userId == -1) { userId = GenerateUserId(); }

        if (State == HotelRoomState.Free)
        {
            SettleHotelRoom(liveDuration, userId);
            return;
        }
        else if (State == HotelRoomState.Booked)
        {
            if (CheckResidenceInterval(DateTime.Now, DateTime.Now + liveDuration))
            {
                SettleHotelRoom(liveDuration, userId);
                return;
            }
            Console.WriteLine($"[BaseHotelRoom] Данное окошко занято! Не удалось заселить в комнату! " +
                              $"RoomId = {RoomId}" +
                              $"Длительность проживания = {liveDuration}");
            throw new InvalidOperationException("Данное окошко занято! Не удалось заселить в комнату!");
        }
        throw new InvalidOperationException("Некорректное состояние комнаты для заселения.");
    }

    /// <summary>
    /// Произвести заселение людей
    /// </summary>
    /// <returns></returns>
    private void SettleHotelRoom(TimeSpan liveDuration, int userId)
    {
        if (liveDuration.TotalDays > MaxLiveDays)
        {
            throw new ArgumentException
            (
                $"Максимальный срок заселения: {MaxLiveDays}, переданный аргумент: {liveDuration.TotalDays}"
            );
            // Конкретно здесь гораздо чище смотрится вывод в консоль
            // Console.WriteLine($"Мы не можем вас заселить на столь длительный срок, максимальное количество дней {MaxLiveDays}");
            // return false;
        }
        _bookInfo.Add(userId, new BookInfo
        {
            LiveStartTime = DateTime.Now,
            LiveEndTime = DateTime.Now + liveDuration
        });
        _currentLiveUserId = userId;

        // Максимальное время в милисекундах 2 147 483 647 (24.8 дня)
        _liveTimer = new Timer(liveDuration.TotalMilliseconds);
        _liveTimer.Start();
        _liveTimer.Elapsed += EndLiveTimeToRoom;
        State = HotelRoomState.Busy;
        Console.WriteLine($"[BaseHotelRoom] Успешно заселили человека. Его длительность проживания = {liveDuration} " +
                          $"RoomId = {RoomId}");
    }

    private void EndLiveTimeToRoom(object? sender, System.Timers.ElapsedEventArgs e)
    {
        Console.WriteLine($"[BaseHotelRoom] Начинаю выселять жителя. RoomId = {RoomId}");
        UnRentHotelRoom();
    }

    public void UnRentHotelRoom()
    {
        if (State == HotelRoomState.Free)
        {
            Console.WriteLine($"[BaseHotelRoom] Пытались выселиться из номера в отеле, но он и так уже пуст! RoomId = {RoomId}");
            throw new InvalidOperationException("Пытались выселиться из номера в отеле, но он и так уже пуст!");
        }

        _liveTimer.Stop();
        _liveTimer.Elapsed -= EndLiveTimeToRoom;
        _liveTimer = null;

        _bookInfo.Remove(_currentLiveUserId);
        State = _bookInfo.Count > 0 ? HotelRoomState.Booked : HotelRoomState.Free;

        _currentLiveUserId = -1;
        Console.WriteLine($"[BaseHotelRoom] Успешно выселили человека. RoomId = {RoomId}");
    }
}