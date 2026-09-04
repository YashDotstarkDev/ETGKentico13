using ETG.Data.Models.Modules;
using System;

namespace ETG.Data.Repositories.Modules
{
    public interface IArticleAuthorRepository
    {
        AuthorModel GetAuthor(Guid guid);
    }
}