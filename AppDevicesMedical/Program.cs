using AppDevicesMedical.Models;
using AppDevicesMedical.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using AppDevicesMedical.Authorization;
using AppDevicesMedical;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ✅ Configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://172.16.131.173:3000",
            "http://localhost:3000",
            "https://med-trasnfers-app-res.vercel.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // si usas Authorization header o cookies
    });
});

builder.Services.AddOpenApi();

// ✅ Configuración EF Core
builder.Services.AddDbContext<MedicalDevicesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Configuración JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)
            ),
            ValidateIssuerSigningKey = true
        };
    });

// ✅ Handler de autorización por permisos
builder.Services.AddScoped<IAuthorizationHandler, PermisoRequeridoHandler>();

// ✅ Políticas de permisos (completas)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Permiso:VER_ROLES", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_ROLES")));

    options.AddPolicy("Permiso:CREAR_ROLES", policy =>
        policy.Requirements.Add(new PermisoRequerido("CREAR_ROLES")));

    options.AddPolicy("Permiso:EDITAR_ROLES", policy =>
        policy.Requirements.Add(new PermisoRequerido("EDITAR_ROLES")));

    options.AddPolicy("Permiso:ELIMINAR_ROLES", policy =>
        policy.Requirements.Add(new PermisoRequerido("ELIMINAR_ROLES")));

    options.AddPolicy("Permiso:GESTIONAR_PERMISOS", policy =>
        policy.Requirements.Add(new PermisoRequerido("GESTIONAR_PERMISOS")));

    options.AddPolicy("Permiso:VER_PERMISOS", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_PERMISOS")));
    //Dispositivos
    options.AddPolicy("Permiso:VER_DISPOSITIVOS", policy =>
       policy.Requirements.Add(new PermisoRequerido("VER_DISPOSITIVOS")));

    options.AddPolicy("Permiso:CREAR_DISPOSITIVOS", policy =>
        policy.Requirements.Add(new PermisoRequerido("CREAR_DISPOSITIVOS")));
    // 🔐 Permisos para AuthController
    options.AddPolicy("Permiso:VER_USUARIO", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_USUARIO")));

    options.AddPolicy("Permiso:VER_TODOS_USUARIOS", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_TODOS_USUARIOS")));

    options.AddPolicy("Permiso:REGISTRAR_USUARIO", policy =>
        policy.Requirements.Add(new PermisoRequerido("REGISTRAR_USUARIO")));

    options.AddPolicy("Permiso:LOGIN_USUARIO", policy =>
        policy.Requirements.Add(new PermisoRequerido("LOGIN_USUARIO")));

    options.AddPolicy("Permiso:EDITAR_USUARIO", policy =>
        policy.Requirements.Add(new PermisoRequerido("EDITAR_USUARIO")));

    options.AddPolicy("Permiso:ELIMINAR_USUARIO", policy =>
        policy.Requirements.Add(new PermisoRequerido("ELIMINAR_USUARIO")));

    options.AddPolicy("Permiso:ENDPOINT_AUTENTICADO", policy =>
        policy.Requirements.Add(new PermisoRequerido("ENDPOINT_AUTENTICADO")));

    //CATEGORIA
    options.AddPolicy("Permiso:VER_CATEGORIAS", policy =>
       policy.Requirements.Add(new PermisoRequerido("VER_CATEGORIAS")));

    options.AddPolicy("Permiso:VER_CATEGORIA", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_CATEGORIA")));

    options.AddPolicy("Permiso:CREAR_CATEGORIA", policy =>
        policy.Requirements.Add(new PermisoRequerido("CREAR_CATEGORIA")));

    options.AddPolicy("Permiso:EDITAR_CATEGORIA", policy =>
        policy.Requirements.Add(new PermisoRequerido("EDITAR_CATEGORIA")));

    options.AddPolicy("Permiso:ELIMINAR_CATEGORIA", policy =>
        policy.Requirements.Add(new PermisoRequerido("ELIMINAR_CATEGORIA")));
    options.AddPolicy("Permiso:VER_CLASES_RIESGO", policy =>
       policy.Requirements.Add(new PermisoRequerido("VER_CLASES_RIESGO")));

    options.AddPolicy("Permiso:VER_CLASE_RIESGO", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_CLASE_RIESGO")));

    options.AddPolicy("Permiso:CREAR_CLASE_RIESGO", policy =>
        policy.Requirements.Add(new PermisoRequerido("CREAR_CLASE_RIESGO")));

    options.AddPolicy("Permiso:EDITAR_CLASE_RIESGO", policy =>
        policy.Requirements.Add(new PermisoRequerido("EDITAR_CLASE_RIESGO")));

    options.AddPolicy("Permiso:ELIMINAR_CLASE_RIESGO", policy =>
        policy.Requirements.Add(new PermisoRequerido("ELIMINAR_CLASE_RIESGO")));
    options.AddPolicy("Permiso:VER_CUARTOS", policy =>
       policy.Requirements.Add(new PermisoRequerido("VER_CUARTOS")));

    options.AddPolicy("Permiso:VER_CUARTO", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_CUARTO")));

    options.AddPolicy("Permiso:CREAR_CUARTO", policy =>
        policy.Requirements.Add(new PermisoRequerido("CREAR_CUARTO")));

    options.AddPolicy("Permiso:EDITAR_CUARTO", policy =>
        policy.Requirements.Add(new PermisoRequerido("EDITAR_CUARTO")));

    options.AddPolicy("Permiso:ELIMINAR_CUARTO", policy =>
        policy.Requirements.Add(new PermisoRequerido("ELIMINAR_CUARTO")));
    options.AddPolicy("Permiso:VER_ESPECIALIDADES", policy =>
       policy.Requirements.Add(new PermisoRequerido("VER_ESPECIALIDADES")));

    options.AddPolicy("Permiso:VER_ESPECIALIDAD", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_ESPECIALIDAD")));

    options.AddPolicy("Permiso:CREAR_ESPECIALIDAD", policy =>
        policy.Requirements.Add(new PermisoRequerido("CREAR_ESPECIALIDAD")));

    options.AddPolicy("Permiso:EDITAR_ESPECIALIDAD", policy =>
        policy.Requirements.Add(new PermisoRequerido("EDITAR_ESPECIALIDAD")));

    options.AddPolicy("Permiso:ELIMINAR_ESPECIALIDAD", policy =>
        policy.Requirements.Add(new PermisoRequerido("ELIMINAR_ESPECIALIDAD")));
    options.AddPolicy("Permiso:VER_EXAMENES", policy =>
       policy.Requirements.Add(new PermisoRequerido("VER_EXAMENES")));

    options.AddPolicy("Permiso:VER_EXAMEN", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_EXAMEN")));

    options.AddPolicy("Permiso:CREAR_EXAMEN", policy =>
        policy.Requirements.Add(new PermisoRequerido("CREAR_EXAMEN")));

    options.AddPolicy("Permiso:EDITAR_EXAMEN", policy =>
        policy.Requirements.Add(new PermisoRequerido("EDITAR_EXAMEN")));

    options.AddPolicy("Permiso:ELIMINAR_EXAMEN", policy =>
        policy.Requirements.Add(new PermisoRequerido("ELIMINAR_EXAMEN")));
    options.AddPolicy("Permiso:VER_STATUS", policy =>
       policy.Requirements.Add(new PermisoRequerido("VER_STATUS")));

    options.AddPolicy("Permiso:VER_STATUS_DETALLE", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_STATUS_DETALLE")));

    options.AddPolicy("Permiso:CREAR_STATUS", policy =>
        policy.Requirements.Add(new PermisoRequerido("CREAR_STATUS")));

    options.AddPolicy("Permiso:EDITAR_STATUS", policy =>
        policy.Requirements.Add(new PermisoRequerido("EDITAR_STATUS")));

    options.AddPolicy("Permiso:ELIMINAR_STATUS", policy =>
        policy.Requirements.Add(new PermisoRequerido("ELIMINAR_STATUS")));
    options.AddPolicy("Permiso:VER_TIPOS_DISPOSITIVO", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_TIPOS_DISPOSITIVO")));

    options.AddPolicy("Permiso:VER_TIPO_DISPOSITIVO", policy =>
        policy.Requirements.Add(new PermisoRequerido("VER_TIPO_DISPOSITIVO")));

    options.AddPolicy("Permiso:CREAR_TIPO_DISPOSITIVO", policy =>
        policy.Requirements.Add(new PermisoRequerido("CREAR_TIPO_DISPOSITIVO")));

    options.AddPolicy("Permiso:EDITAR_TIPO_DISPOSITIVO", policy =>
        policy.Requirements.Add(new PermisoRequerido("EDITAR_TIPO_DISPOSITIVO")));

    options.AddPolicy("Permiso:ELIMINAR_TIPO_DISPOSITIVO", policy =>
        policy.Requirements.Add(new PermisoRequerido("ELIMINAR_TIPO_DISPOSITIVO")));
});

// ✅ Servicios de Auth
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// ✅ Orden correcto de middlewares
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

// ✅ Respuesta explícita a OPTIONS (preflight)
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "https://med-trasnfers-app-res.vercel.app");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
    }
    else
    {
        await next();
    }
});

app.MapOpenApi();
app.MapScalarApiReference();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == StatusCodes.Status403Forbidden)
    {
        response.ContentType = "application/json";
        await response.WriteAsync("{\"error\":\"No tienes permisos suficientes para acceder a este recurso.\"}");
    }
});

app.MapControllers();
app.Run();