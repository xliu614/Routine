using Routine.Api.Data;
using Routine.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Routine.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            //builder.Services.AddRazorPages();
            builder.Services.AddControllers(
                setup => { setup.ReturnHttpNotAcceptable = true;
                    //suppport XML output
                    //OutputFormatters is a collection, and by default it supports json, so one can add for XML, now we have both json and xml
                    //setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    
                    //if want to setup the default formater as for xml
                    //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter());
                }).AddXmlDataContractSerializerFormatters(); // this is to add xml formatter for both input and output formatters
            builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
            builder.Services.AddDbContext<RoutineDbContext>(option =>
            {
                option.UseSqlite("Data Source=routine.db");
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetService<RoutineDbContext>();
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.Migrate();
                }
                catch (Exception e)
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                    logger.LogError(e, "Database Migration Error");
                }
            }



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                {
                    //app.UseSwagger();
                    //app.UseSwaggerUI();
                }


            app.UseAuthorization();
            app.MapControllers();

            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.MapRazorPages(); //Routes for pages
            //    endpoints.MapControllers(); //Routes for my API controllers
            //});

            //app.MapRazorPages();

            app.Run();
        }
    }
}