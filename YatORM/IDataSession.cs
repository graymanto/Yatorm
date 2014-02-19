using System.Collections.Generic;

namespace YatORM
{
    public interface IDataSession
    {
        IEnumerable<TEntity> FindAll<TEntity>();
    }
}