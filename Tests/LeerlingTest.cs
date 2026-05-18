using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Controllers;
using Piano.Data;
using Piano.Models;
using System.Security.Claims;

namespace Piano.Tests;

[TestClass]
public class LeerlingToevoegenAanKlasTest
{
    private ApplicationDbContext _context = default!;
    private KlasController _controller = default!;
    private UserManager<IdentityUser> _userManager = default!;
    private string _docentUserId = default!;
    private string _leerlingUserId = default!;
    private int _klasId;

    [TestInitialize]
    public async Task Setup()
    {
        // In-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        _context.Roles.Add(new IdentityRole { Id = "1", Name = "Docent", NormalizedName = "DOCENT" });
        _context.Roles.Add(new IdentityRole { Id = "2", Name = "Leerling", NormalizedName = "LEERLING" });
        await _context.SaveChangesAsync();

        // Setup UserManager
        var store = new UserStore<IdentityUser>(_context);
        var options1 = Microsoft.Extensions.Options.Options.Create(new IdentityOptions());
        var passwordHasher = new PasswordHasher<IdentityUser>();
        var userValidators = new List<IUserValidator<IdentityUser>>();
        var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
        var keyNormalizer = new UpperInvariantLookupNormalizer();
        var errors = new IdentityErrorDescriber();
        var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<UserManager<IdentityUser>>.Instance;
        var servicesProvider = new Microsoft.Extensions.DependencyInjection.ServiceCollection().BuildServiceProvider();

        _userManager = new UserManager<IdentityUser>(store, options1, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, servicesProvider, logger);

        _docentUserId = Guid.NewGuid().ToString();
        var docentUser = new IdentityUser { Id = _docentUserId, UserName = "docent@test.nl", Email = "docent@test.nl" };
        await _userManager.CreateAsync(docentUser);
        await _userManager.AddToRoleAsync(docentUser, "Docent");

        var docent = new Docent { Naam = "Test Docent", IdentityUserId = _docentUserId };
        _context.Docenten.Add(docent);

        _leerlingUserId = Guid.NewGuid().ToString();
        var leerlingUser = new IdentityUser { Id = _leerlingUserId, UserName = "leerling@test.nl", Email = "leerling@test.nl" };
        await _userManager.CreateAsync(leerlingUser);
        await _userManager.AddToRoleAsync(leerlingUser, "Leerling");

        var leerling = new Leerling { Naam = "Test Leerling", IdentityUserId = _leerlingUserId };
        _context.Leerlingen.Add(leerling);

        var klas = new Klas { Naam = "Test Klas", DocentIdentityUserId = _docentUserId };
        _context.Klassen.Add(klas);
        await _context.SaveChangesAsync();

        _klasId = klas.Id;

        // Setup controller met ingelogde docent
        _controller = new KlasController(_context, _userManager);

        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, _docentUserId) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [TestMethod]
    public async Task AddLeerlingToKlas()
    {
        // Arrange
        var dto = new AddLeerlingDto { Email = "leerling@test.nl", Naam = "Test Leerling", Niveau = Niveau.Beginner };

        // Act
        var result = await _controller.AddLeerlingToKlas(_klasId, dto);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var leerlingInDb = await _context.Leerlingen.FirstOrDefaultAsync(l => l.IdentityUserId == _leerlingUserId);
        Assert.IsNotNull(leerlingInDb);
        Assert.AreEqual(_klasId, leerlingInDb.KlasId);
    }

    [TestMethod]
    public async Task AddLeerlingToKlasGeeftNotFound()
    {
        // Arrange
        var dto = new AddLeerlingDto { Email = "bestaatiniet@test.nl" };

        // Act
        var result = await _controller.AddLeerlingToKlas(_klasId, dto);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual("Leerling met dit e-mailadres niet gevonden", notFoundResult.Value);
    }

    [TestMethod]
    public async Task AddLeerlingToKlasDocentGeeftBadRequest()
    {
        // Arrange
        var dto = new AddLeerlingDto { Email = "docent@test.nl" };

        // Act
        var result = await _controller.AddLeerlingToKlas(_klasId, dto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.IsTrue(badRequestResult.Value?.ToString()?.Contains("docent"));
    }

    [TestMethod]
    public async Task AddLeerlingToKlasLeerlingAlInKlas()
    {
        // Arrange
        var dto = new AddLeerlingDto { Email = "leerling@test.nl", Naam = "Test Leerling" };
        await _controller.AddLeerlingToKlas(_klasId, dto);

        // Act
        var result = await _controller.AddLeerlingToKlas(_klasId, dto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.IsTrue(badRequestResult.Value?.ToString()?.Contains("zit al in"));
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}