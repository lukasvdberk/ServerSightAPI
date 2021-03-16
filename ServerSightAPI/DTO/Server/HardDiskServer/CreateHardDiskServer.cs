namespace ServerSightAPI.DTO.Server.HardDiskServer
{
    public class CreateHardDiskServer
    {
        public string DiskName { get; set; }
        public float SpaceAvailable { get; set; }
        public float SpaceTotal { get; set; }
    }
}