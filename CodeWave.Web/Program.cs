using CodeWave.Application.Interfaces;
using CodeWave.Application.Services;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using CodeWave.Infrastructure.Data.Seed;
using CodeWave.Infrastructure.Services;
using CodeWave.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using AutoMapper;
using CodeWave.Web.Hubs;
using CodeWave.Web.Services;

// ======================================================
// SERILOG CONFIGURATION
// ======================================================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/codewave-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Use Serilog instead of default logging
builder.Host.UseSerilog();

// ======================================================
// DATABASE (SQL SERVER ONLY)
// ======================================================
var sqlServerConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(sqlServerConnectionString,
        sql => sql.MigrationsAssembly("CodeWave.Infrastructure"));
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpClient();

// ======================================================
// CUSTOM SERVICES
// ======================================================
builder.Services.AddSingleton<CodeRunnerService>();
builder.Services.AddSingleton<CodeExecutionService>();
builder.Services.AddScoped<ICodeExecutionService, JavaExecutionService>();

// ======================================================
// IDENTITY CONFIGURATION
// ======================================================
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager<SignInManager<ApplicationUser>>()
.AddDefaultTokenProviders()
.AddClaimsPrincipalFactory<CodeWave.Web.Claims.UserClaimsPrincipalFactory>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/User/AccessDenied";
});

// ======================================================
// EXTERNAL AUTHENTICATION (GOOGLE + GITHUB)
// ======================================================
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

var githubClientId = builder.Configuration["Authentication:GitHub:ClientId"];
var githubClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/User/AccessDenied";
})
.AddCookie(IdentityConstants.ExternalScheme)
.AddCookie(IdentityConstants.TwoFactorUserIdScheme)
.AddCookie(IdentityConstants.TwoFactorRememberMeScheme)
.AddGoogle(options =>
{
    options.ClientId = googleClientId!;
    options.ClientSecret = googleClientSecret!;
    options.SaveTokens = true;
})
.AddGitHub(options =>
{
    options.ClientId = githubClientId!;
    options.ClientSecret = githubClientSecret!;
    options.Scope.Add("user:email");
    options.SaveTokens = true;
});

// ======================================================
// SESSION
// ======================================================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// ======================================================
// AUTHORIZATION POLICIES
// ======================================================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.Identity!.IsAuthenticated &&
            context.User.HasClaim("IsAdmin", "true")));
});

// ======================================================
// AUTOMAPPER
// ======================================================
builder.Services.AddAutoMapper(typeof(CodeWave.Application.Mappings.MappingProfile));

// ======================================================
// SIGNALR
// ======================================================
builder.Services.AddSignalR();

// ======================================================
// BACKGROUND SERVICES
// ======================================================
builder.Services.AddHostedService<CleanupBackgroundService>();

// ======================================================
// MVC + APPLICATION SERVICES
// ======================================================
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseSubmissionRepository, ExerciseSubmissionRepository>();
builder.Services.AddScoped<ILessonCompletionRepository, LessonCompletionRepository>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILearningPathService, LearningPathService>();
builder.Services.AddScoped<ICodeService, CodeService>();
builder.Services.AddScoped<CodeWave.Infrastructure.Services.ICVPDFService, CodeWave.Infrastructure.Services.CVPDFService>();
builder.Services.AddScoped<IUserService, CodeWave.Infrastructure.Services.UserService>();
builder.Services.AddScoped<IJobOfferService, CodeWave.Infrastructure.Services.JobOfferService>();
builder.Services.AddScoped<IJobApplicationService, CodeWave.Infrastructure.Services.JobApplicationService>();
builder.Services.AddScoped<ICVService, CodeWave.Infrastructure.Services.CVService>();
builder.Services.AddScoped<IProjectRepository, CodeWave.Infrastructure.Repositories.ProjectRepository>();
builder.Services.AddScoped<IQuizRepository, CodeWave.Infrastructure.Repositories.QuizRepository>();
builder.Services.AddScoped<IUserCourseRepository, CodeWave.Infrastructure.Repositories.UserCourseRepository>();

var app = builder.Build();

// ======================================================
// DATABASE SEEDING
// ======================================================
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await SeedData.SeedAsync(context);
        await JavaSeed.SeedAsync(context);
        await PythonSeed.SeedAsync(context);
        await JobOfferSeed.SeedAsync(context);
    }

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await AdminSeed.SeedAsync(context, userManager);
    }

    Log.Information("Database seeding completed successfully.");
}
catch (Exception ex)
{
    Log.Error(ex, "Error during database seeding: {Message}", ex.Message);
}

// ======================================================
// PIPELINE
// ======================================================
if (app.Environment.IsDevelopment())
{
    // Optional: app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Welcome}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");

// Ensure log directory exists
var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

Log.Information("CodeWave application started successfully");

app.Run();