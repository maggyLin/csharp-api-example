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


            //更新 localDB 
            //Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\code\csharp\dotnet-api-example\localsql\Todo.mdf;Integrated Security=True;Connect Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force -CoNtext TodoContext
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TodoDatabase")));


            //service DI注入
            //不同注入方法
            //services.AddScoped<>() => 每個request為一個新的實例
            //services.AddSingleton<>() => 每次注入都是一個新的實例
            //services.AddTransient<>() => 程式運行期間只會有一個實例(伺服器重啟才會重置)
            services.AddScoped<TodoServiceTestService>();

            //service  Ioc DI注入 <介面,要實作的方法> 1:1
            //使用Ioc DI ,針對不同狀況可以直接更換實作
            //services.AddScoped<IIocExampleService, IocExampleService>();
            services.AddScoped<IIocExampleService, IocExampleService2>();

            //service Ioc DI注入 <介面,要實作的方法> 1:多
            //在程式中判斷用哪一個實作
            services.AddScoped<IIocMultipleExampleService, IocMultipleExampleService1>();
            services.AddScoped<IIocMultipleExampleService, IocMultipleExampleService2>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //只有開發時才會有swagger
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
