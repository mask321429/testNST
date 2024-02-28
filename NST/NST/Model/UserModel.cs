using System;
using System.ComponentModel.DataAnnotations;

namespace NST.Model{

	public class PersonModel
	{
		[Key]
		public long Id { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public List<Skill> Skills { get; set; }
	}

	public class Skill
	{
		[Key]
		public long Id { get; set; }
		public string Name { get; set; }
		public byte Level { get; set; } 
	}

}


