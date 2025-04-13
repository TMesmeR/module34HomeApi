namespace HomeApi.Contracts.Models.Devices;

public class DeleteDeviceRequest
{
    public Guid Id { get; set; }    
    public string Name { get; set; }
    public string Room { get; set; }
    
    
}