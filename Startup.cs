using System.Text;
using AutoMapper;
using DesafioBackEnd.Data;
using DesafioBackEnd.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DesafioBackEnd
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
            //obtem a configuração do banco no arquivo appsettings.json
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddCors();
            //registra o serviço e a instancia dura até o final da requisição
            services.AddScoped<IAuthRepository,AuthRepository>();
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfiles());
            });
            // configura o serviço do AutoMapper
             IMapper mapper = mapperConfig.CreateMapper();
             // o AutoMapper utiliza a mesma instancia para toda a aplicação
            services.AddSingleton(mapper);
            //obtem uma chave do token inicial pois em cima dela será baseado toda metodologia do hash
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value); 
            //configura o jwt
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            //configura o swagger
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new OpenApiInfo { Title ="API Desafio BackEnd", Version ="v1"});
            });
            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
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

            app.UseAuthorization();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
       

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //disponibiliza o swagger
            app.UseSwagger();
            app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "API Desafio BackEnd"));
        }
    }
}
