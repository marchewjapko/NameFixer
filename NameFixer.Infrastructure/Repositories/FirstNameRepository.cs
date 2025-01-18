using NameFixer.Core.Entities;
using NameFixer.Core.Repositories;

namespace NameFixer.Infrastructure.Repositories;

public class FirstNameRepository : RepositoryBase<FirstNameEntity>, IFirstNameRepository;