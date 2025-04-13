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
}