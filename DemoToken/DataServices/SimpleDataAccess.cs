namespace DemoToken.DataServices
{
    using Models;
    public class SimpleDataAccess
    {
        public List<Program> Program { get; set; }
        public void Load()
        {
            Program = new List<Program>
            {
                new Program {Id = 1, NameProgram = "Developing young talent", DesciptionProgram = "Nothing" },
                new Program {Id = 2, NameProgram = "Support for people with disabilities", DesciptionProgram = "Nothing" },
                new Program {Id = 3, NameProgram = "Development of a football center", DesciptionProgram = "Nothing" },
                new Program {Id = 4, NameProgram = "Charity fund", DesciptionProgram = "Nothing" },
                new Program {Id = 5, NameProgram = "Road Infrastructure Development Fund", DesciptionProgram = "Nothing" },
            };
        }
        public void SaveChanges() { }
    }
}
