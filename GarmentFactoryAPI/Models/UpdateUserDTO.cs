namespace GarmentFactoryAPI.Models
{
    public class UpdateUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public int RoleId { get; set; }
    }
}
