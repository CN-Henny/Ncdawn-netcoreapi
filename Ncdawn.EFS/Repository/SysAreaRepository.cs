using Ncdawn.EFS.DBContext;
using Ncdawn.EFS.Models;

namespace Ncdawn.EFS.Response
{
    public partial interface ISysAreaRepository : IBaseRepository<SysArea>
    {
    }
    public partial class SysAreaRepository : BaseRepository<SysArea>, ISysAreaRepository
    {

        public SysAreaRepository(MSSqlDBContext db) : base(db)
        {

        }

    }

}
