using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomsController:ControllerBase
{
    private IRoomRepository _repository;
    private IMapper _mapper;

    public RoomsController(IRoomRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    /// <summary>
    /// Добавляем новую комнату
    /// </summary>
    /// <param name="request">Данные для создания комнаты</param>
    /// <returns></returns>
    [HttpPost] 
    [Route("")] 
    public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
    {
        var existingRoom = await _repository.GetRoomByName(request.Name);
        if (existingRoom == null)
        {
            var newRoom = _mapper.Map<AddRoomRequest,Room>(request);
            await _repository.AddRoom(newRoom);
            return StatusCode(201, $"Комната {request.Name} добавлена!");
        }
            
        return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
    }
    /// <summary>
    /// Получение всех комнат
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _repository.GetAllRooms();
        
        return StatusCode(200, rooms);
    }

    /// <summary>
    /// Изменение комнаты
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Route("{name}")]
    public async Task<IActionResult> UpdateRoom(
        [FromRoute] string name,
        [FromBody] EditRoomRequest request)
    {
        var room = await _repository.GetRoomByName(name);
        if (room == null)
            return StatusCode(404, $"Комната {name} не найдена!");

        if (request.NewName != null && request.NewName != name)
        {
            var existingRoom = await _repository.GetRoomByName(request.NewName);
            if (existingRoom != null)
                return StatusCode(409, $"Комната {request.NewName} уже существует!");
        }
        
        if (request.NewName != null)
            room.Name = request.NewName;

        if (request.Area.HasValue)
            room.Area = request.Area.Value;

        if (request.GasConnected.HasValue)
            room.GasConnected = request.GasConnected.Value;

        if (request.Voltage.HasValue)
            room.Voltage = request.Voltage.Value;

        await _repository.UpdateRoom(room);
        
        return StatusCode(200, $"Комната {name} успешно обновлена!");
    }
}