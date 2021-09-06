using Compentio.Example.App.Entities;
using Compentio.Example.App.Mappers;
using System;

namespace Compentio.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ISampleObjectMapper mapper = SampleObjectMapper.Create();

            var dao = new NoteDao
            {
                Id = 5,
                Created = DateTime.UtcNow,
                CreatedBy = "Admin",
                Description = "Description",
                Modified = DateTime.Now,
                PageTitle = "This is a title of the page",
                ValidFrom = DateTime.MinValue,
                ValidTo = DateTime.MaxValue
            };

            var dto = mapper.MapToRest(dao);

            Console.WriteLine($"Description: '{dto.Description}', Title '{dto.Title}'");
            Console.ReadKey();
        }
    }
}
