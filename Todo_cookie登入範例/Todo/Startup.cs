using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Todo.Interfaces;
using Todo.Models;
using Todo.Services;

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
            //Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Todo.mdf;Integrated Security=True;Connect Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force -CoNtext TodoContext
            services.AddControllers()
            .AddNewtonsoftJson();
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TodoDatabase")));
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<TodoListService>();
            services.AddScoped<TestDIService>();
            services.AddScoped<TodoListAsyncService>();
            services.AddScoped<AsyncService>();


            //�C���`�J�ɡA���O�@�ӷs����ҡC
            services.AddSingleton<SingletonService>();

            //�C��Request���P�@�ӷs�����
            services.AddScoped<ScopedService>();

            //�{���B������u�|���@�ӹ��
            services.AddTransient<TransientService>();


            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<ITodoListService, TodoLinqService>();
            services.AddScoped<ITodoListService, TodoAutomapperService>();
            services.AddScoped<ITodoListService, TodoListRService>(); 
            services.AddHttpContextAccessor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                //���n�J�ɷ|�۰ʾɨ�o�Ӻ��}
                option.LoginPath = new PathString("/api/Login/NoLogin");
                //�S���v���ɷ|�۰ʾɨ�o�Ӻ��}
                option.AccessDeniedPath = new PathString("/api/Login/NoAccess");
                //option.ExpireTimeSpan = TimeSpan.FromSeconds(2);
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //cookie 驗證
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
