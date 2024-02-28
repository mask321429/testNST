using System;
using NST.DTO;
using NST.Model;

namespace NST.Interfaces
{
	public interface IPersonService 
	{
        Task CreateNewPerson(PersonDTO personDto);
        Task<List<PersonDTO>> GetListPersons();
        Task<PersonDTO> GetInfoPersonById(long Id);
        Task UpdatePerson(long id, PersonDTO personDto);
        Task DeletePerson(long userId);
	}
}

