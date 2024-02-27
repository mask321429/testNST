using System.ComponentModel.DataAnnotations;
using NST.Model;
namespace NST.DTO;

public class PersonDTO
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public List<SkillDTO> Skills { get; set; }
}