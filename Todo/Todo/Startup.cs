using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Models;
using Todo.Services;
using Todo.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Todo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo", Version = "v1" });
            });


            //��s localDB 
            //Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\code\csharp\dotnet-api-example\localsql\Todo.mdf;Integrated Security=True;Connect Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force -CoNtext TodoContext
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TodoDatabase")));



            //service DI�`�J
            //���P�`�J��k
            //services.AddScoped<>() => �C��request���@�ӷs�����
            //services.AddSingleton<>() => �C���`�J���O�@�ӷs�����
            //services.AddTransient<>() => �{���B������u�|���@�ӹ��(���A�����Ҥ~�|���m)
            services.AddScoped<TodoServiceTestService>();
            services.AddScoped<TodoListAsyncService>();

            //service  Ioc DI�`�J <����,�n��@����k> 1:1
            //�ϥ�Ioc DI ,�w�藍�P���p�i�H�����󴫹�@
            //services.AddScoped<IIocExampleService, IocExampleService>();
            services.AddScoped<IIocExampleService, IocExampleService2>();

            //service Ioc DI�`�J <����,�n��@����k> 1:�h
            //�b�{�����P�_�έ��@�ӹ�@
            services.AddScoped<IIocMultipleExampleService, IocMultipleExampleService1>();
            services.AddScoped<IIocMultipleExampleService, IocMultipleExampleService2>();




            //JWT ���� (�M��w��:Microsoft.AspNetCore.Authentication.JwtBearer)
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //�O�_�n���ҵo��� (�n����JWT�n�g�J,�Ѧ�LoginController.cs =>JWTLoginExample)
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidateLifetime = true,  //�O�_�n���Ҩ���ɶ�
                        //ClockSkew = TimeSpan.Zero, //���ҹL��"���n"�w�Įɶ�
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:KEY"])) //�T�{KEY
                    };
                });

            //�O�_����api���n�n�J���� , �_�h��^401 (�ӧO���ݭn��api�n�[�W [AllowAnonymous] ����)
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter());
            //});







        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //�u���}�o�ɤ~�|��swagger
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/static-files?view=aspnetcore-6.0
            //�ϥ��R�A�ؿ�(��~�i����ؿ��U���)
            //�w�]�����V wwwroot
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
