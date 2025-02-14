using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Applications.Interfaces.Services;
using Decida.Sj.Applications.Interfaces.UseCases;
using Decida.Sj.Applications.Interfaces.Dto;
using Decida.Sj.Applications.UseCases;
using Decida.Sj.Infrastructure.Services;
using Decida.Sj.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<ConnectionStringsOptionsDTO>(
    builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddTransient<IPacientsServices, PacientsServices>();

builder.Services.AddSingleton<IMSpecialtyMysqlRepository, MSpecialtyMysqlRepository>();
builder.Services.AddTransient<IGetPacientDataUseCase, GetPacientDataUseCase>();
builder.Services.AddTransient<IGetMedicalSpecialtyDataUseCase, GetMedicalSpecialtyDataUseCase>();
 
builder.Services.AddTransient<IGetMedicDataUseCase, GetMedicDataUseCase>();
builder.Services.AddTransient<IGetRankAgendaByFilterUseCase, GetRankAgendaByFilterUseCase>();
builder.Services.AddSingleton<IAgendaOracleRepository, AgendaOracleRepository>();
builder.Services.AddTransient<IMedicService, MedicService>();

builder.Services.AddSingleton<IMedicMysqlRepository, MedicMysqlRepository>();
builder.Services.AddTransient<IMedicalSpecialtyServices, MedicalSpecialtyServices>();

builder.Services.AddTransient<IGetHealthPlanDataUseCase, GetHealthPlanDataUseCase>();
builder.Services.AddSingleton<IPacientOracleRepository, PacientOracleRepository>();
builder.Services.AddSingleton<IHealthPlanMysqlRepository, HealthPlanMysqlRepository>();
builder.Services.AddTransient<IHealthServices, HealthServices>();




builder.Logging.AddConsole();





var app = builder.Build();





// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
