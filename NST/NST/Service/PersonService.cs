
using NST.DbContext;
using NST.DTO;
using NST.Model;
using NST.Interfaces;
using NST.Middleware;
using Microsoft.EntityFrameworkCore;

namespace NST.Service
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Random _random;

        public PersonService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _random = new Random();
        }
        //-----------------------{Список пользователей}---------------------------
        public async Task<List<PersonDTO>> GetListPersons()
        {
            var persons = await _dbContext.Persons
                .Include(p => p.Skills)
                .ToListAsync();

            var personsDto = persons.Select(p => new PersonDTO
            {
                Name = p.Name,
                DisplayName = p.DisplayName,
                Skills = p.Skills.Select(s => new SkillDTO
                {
                    Name = s.Name,
                    Level = s.Level
                }).ToList()
            }).ToList();

            return personsDto;
        }
        //-----------------------{Удаление}---------------------------
        public async Task DeletePerson(long userId)
        {
            var user = await _dbContext.Persons.Include(u => u.Skills).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }
            _dbContext.Skills.RemoveRange(user.Skills);
            _dbContext.Persons.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
        //-----------------------{Обновление информации}---------------------------
        public async Task UpdatePerson(long id, PersonDTO personDto)
        {
            ValidatePerson(personDto);
            var existingPerson = await _dbContext.Persons
                .Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPerson == null)
            {
                throw new KeyNotFoundException($"Person with ID {id} not found");
            }

            existingPerson.Name = personDto.Name;
            existingPerson.DisplayName = personDto.DisplayName;

            _dbContext.Skills.RemoveRange(existingPerson.Skills);

            existingPerson.Skills = personDto.Skills.Select(skillDto => new Skill
            {
                Name = skillDto.Name,
                Level = skillDto.Level
            }).ToList();

            await _dbContext.SaveChangesAsync();
        }
        //-----------------------{Информация по Id}---------------------------
        public async Task<PersonDTO> GetInfoPersonById(long id)
        {
            var person = await _dbContext.Persons
                .Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                throw new KeyNotFoundException($"Person with ID {id} not found");
            }

            var personDto = new PersonDTO
            {
                Name = person.Name,
                DisplayName = person.DisplayName,
                Skills = person.Skills.Select(s => new SkillDTO
                {
                    Name = s.Name,
                    Level = s.Level
                }).ToList()
            };

            return personDto;
        }
        //-----------------------{Новый сотрудник}---------------------------
        public async Task CreateNewPerson(PersonDTO personDto)
        {
            ValidatePerson(personDto);

            long newPersonId = GenerateUniqueId();

            var skills = personDto.Skills.Select(skillDto => new Skill
            {
                Name = skillDto.Name,
                Level = skillDto.Level
            }).ToList();

            var newPerson = new PersonModel
            {
                Name = personDto.Name,
                DisplayName = personDto.DisplayName,
                Id = newPersonId,
                Skills = skills
            };

            _dbContext.Persons.Add(newPerson);
            await _dbContext.SaveChangesAsync();
        }
        
        private void ValidatePerson(PersonDTO personDto)
        {
            if (string.IsNullOrWhiteSpace(personDto.Name))
            {
                throw new BadDataException("The \"Name\" field cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(personDto.DisplayName))
            {
                throw new BadDataException("The \"DisplayName\" field cannot be empty");
            }

            if (personDto.Skills == null || !personDto.Skills.Any())
            {
                throw new BadDataException("The \"Skills\" field cannot be empty");
            }

            foreach (var skillDto in personDto.Skills)
            {
                if (skillDto.Level < 1 || skillDto.Level > 10)
                {
                    throw new BadDataException("Skill level must be between 1 and 10");
                }
            }
        }
        private long GenerateUniqueId()
        {
            long uniqueId = _random.Next(1, int.MaxValue) + ((long)_random.Next(int.MaxValue) << 32);
            return uniqueId;
        }
    }
}

