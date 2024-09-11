using gp_backend.Core.Models;

namespace gp_backend.Api.Dtos
{
    public class GetWoundDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? file { get; set; }
        public string Type { get; set; }
        public string AddedDate { get; set; }
    }
}
