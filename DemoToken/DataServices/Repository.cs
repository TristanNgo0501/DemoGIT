namespace DemoToken.DataServices
{
    using Models;
    public class Repository
    {
        protected readonly SimpleDataAccess _context;
        public Repository(SimpleDataAccess context)
        {
            _context = context;
            _context.Load();
        }
        public void SaveChanges() => _context.SaveChanges();
        public List<Program> Programs => _context.Program;
        public Program[] Select() => _context.Program.ToArray();
        public Program Select(int id)
        {
            foreach (var p in _context.Program)
            {
                if (p.Id == id) return p;
            }
            return null;
        }

        public Program[] Select(string key)
        {
            var temp = new List<Program>();
            var k = key.ToLower();
            foreach (var p in _context.Program)
            {
                var logic =
                    p.NameProgram.ToLower().Contains(k) ||
                    p.DesciptionProgram.ToLower().Contains(k)
                    ;
                if (logic) temp.Add(p);
            }
            return temp.ToArray();
        }
        public void Insert(Program program)
        {
            var lastIndex = _context.Program.Count - 1;
            var id = lastIndex < 0 ? 1 : _context.Program[lastIndex].Id + 1;
            program.Id = id;
            _context.Program.Add(program);
        }
    }
}
