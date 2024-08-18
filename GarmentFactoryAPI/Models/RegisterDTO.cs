namespace GarmentFactoryAPI.Models
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int roleId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
