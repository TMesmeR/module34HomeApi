﻿using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers;
[ApiController]
[Route("[controller]")]
public class DeviceController: ControllerBase
{
    private IDeviceRepository _devices;
    private IRoomRepository _rooms;
    private IMapper _mapper;

    public DeviceController(IDeviceRepository devices, IRoomRepository rooms, IMapper mapper)
    {
        _devices = devices;
        _rooms = rooms;
        _mapper = mapper;
    }
    /// <summary>
    /// Получение списка всех устройст
    /// </summary>
    /// <returns>Список устройств с общей информацией</returns>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetDevices()
    {
        var devices = await _devices.GetDevices();

        var resp = new GetDeviceResponse()
        {
            DeviceAmount = devices.Length,
            Devices = _mapper.Map<Device[], DeviceView[]>(devices)
        };
        return StatusCode(200, resp);
    }
    /// <summary>
    /// Добавлений нового устройства
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost] 
    [Route("")] 
    public async Task<IActionResult> Add( AddDeviceRequest request )
    {
        var room = await _rooms.GetRoomByName(request.RoomLocation);
        if(room == null)
            return StatusCode(400, $"Ошибка: Комната {request.RoomLocation} не подключена. Сначала подключите комнату!");
            
        var device = await _devices.GetDeviceByName(request.Name);
        if(device != null)
            return StatusCode(400, $"Ошибка: Устройство {request.Name} уже существует.");
            
        var newDevice = _mapper.Map<AddDeviceRequest, Device>(request);
        await _devices.SaveDevice(newDevice, room);
            
        return StatusCode(201, $"Устройство {request.Name} добавлено. Идентификатор: {newDevice.Id}");
    }
    /// <summary>
    /// Редактирование существующего устройства
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch] 
    [Route("{id}")] 
    public async Task<IActionResult> Edit(
        [FromRoute] Guid id,
        [FromBody]  EditDeviceRequest request)
    {
        var room = await _rooms.GetRoomByName(request.NewRoom);
        if(room == null)
            return StatusCode(400, $"Ошибка: Комната {request.NewRoom} не подключена. Сначала подключите комнату!");
            
        var device = await _devices.GetDeviceByID(id);
        if(device == null)
            return StatusCode(400, $"Ошибка: Устройство с идентификатором {id} не существует.");
            
        var withSameName = await _devices.GetDeviceByName(request.NewName);
        if(withSameName != null)
            return StatusCode(400, $"Ошибка: Устройство с именем {request.NewName} уже подключено. Выберите другое имя!");

        await _devices.UpdateDevice(
            device,
            room,
            new UpdateDeviceQuery(request.NewName, request.NewSerial)
        );

        return StatusCode(200, $"Устройство обновлено! Имя - {device.Name}, Серийный номер - {device.SerialNumber},  Комната подключения - {device.Room.Name}");
    }
    
    // TODO: Задание: напишите запрос на удаление устройства
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteDevice(
        [FromRoute] Guid id,
        [FromBody] DeleteDeviceRequest request)
    {
        var room = await _rooms.GetRoomByName(request.Name);
        if(room == null)
            return StatusCode(400, $"Ошибка: Комната {request.Room} не подключена. Сначала подключите комнату!");
        var device = await _devices.GetDeviceByID(id);
        if (device != null)
            return StatusCode(400, $"Ошибка: Устройство с идентификатором {id} не существует.");
        
        return Ok();
    }
}