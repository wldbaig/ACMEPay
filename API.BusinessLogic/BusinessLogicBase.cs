using API.Common;
using API.Repository;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.BusinessLogic
{
    public partial class ServiceBusinessLogic : IServiceBusinessLogic
    {
        CorsOptions _options;
        protected readonly SettingConfig settingConfig;
        protected readonly AppDbContext db = null;
        protected readonly IStoredProcedures storedProcedures;

        protected readonly IServiceLogger<ServiceBusinessLogic> logger;
        public ServiceBusinessLogic(SettingConfig settingConfig, AppDbContext dbContext, IServiceLogger<ServiceBusinessLogic> logger, IStoredProcedures storedProcedures, IOptions<CorsOptions> options)
        {
            db = dbContext;
            db.Database.ExecuteSqlRaw("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            this.settingConfig = settingConfig;  
            this.logger = logger;
            this.storedProcedures = storedProcedures;
            _options = options.Value;
        }
    }
}
