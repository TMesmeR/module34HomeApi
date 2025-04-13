using HomeApi.Data.Models;

namespace HomeApi.Data.Repos;

public interface IRoomRepository
{
    Task<Room> GetRoomByName(string name);
    Task AddRoom(Room room);
    Task<Room[]> GetAllRooms();
}