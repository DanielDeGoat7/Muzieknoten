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
public class DashboardIntegratieTesten
{
    private ApplicationDbContext _context = default!;
    private ResultaatController _resultaatController = default!;
    private UserManager<IdentityUser> _userManager = default!;
    private string _docentUserId = default!;
    private string _leerlingUserId = default!;
    private int _klasId;
    private int _leerlingId;

    [TestInitialize]
    public async Task Setup()
    {
        // In-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        // Rollen aanmaken
        _context.Roles.Add(new IdentityRole { Id = "1", Name = "Docent", NormalizedName = "DOCENT" });
        _context.Roles.Add(new IdentityRole { Id = "2", Name = "Leerling", NormalizedName = "LEERLING" });
        await _context.SaveChangesAsync();

        // Setup UserManager
        var store = new UserStore<IdentityUser>(_context);
        var identityOptions = Microsoft.Extensions.Options.Options.Create(new IdentityOptions());
        var passwordHasher = new PasswordHasher<IdentityUser>();
        var userValidators = new List<IUserValidator<IdentityUser>>();
        var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
        var keyNormalizer = new UpperInvariantLookupNormalizer();
        var errors = new IdentityErrorDescriber();
        var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<UserManager<IdentityUser>>.Instance;
        var servicesProvider = new Microsoft.Extensions.DependencyInjection.ServiceCollection().BuildServiceProvider();

        _userManager = new UserManager<IdentityUser>(store, identityOptions, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, servicesProvider, logger);

        // Docent aanmaken
        _docentUserId = Guid.NewGuid().ToString();
        var docentUser = new IdentityUser { Id = _docentUserId, UserName = "docent@test.nl", Email = "docent@test.nl" };
        await _userManager.CreateAsync(docentUser, "Test123!");
        await _userManager.AddToRoleAsync(docentUser, "Docent");

        var docent = new Docent { Naam = "Test Docent", IdentityUserId = _docentUserId };
        _context.Docenten.Add(docent);

        // Leerling aanmaken
        _leerlingUserId = Guid.NewGuid().ToString();
        var leerlingUser = new IdentityUser { Id = _leerlingUserId, UserName = "leerling@test.nl", Email = "leerling@test.nl" };
        await _userManager.CreateAsync(leerlingUser, "Test123!");
        await _userManager.AddToRoleAsync(leerlingUser, "Leerling");

        var leerling = new Leerling { Naam = "Test Leerling", IdentityUserId = _leerlingUserId };
        _context.Leerlingen.Add(leerling);
        await _context.SaveChangesAsync();
        _leerlingId = leerling.Id;

        // Klas aanmaken
        var klas = new Klas { Naam = "Test Klas", DocentIdentityUserId = _docentUserId };
        _context.Klassen.Add(klas);
        await _context.SaveChangesAsync();
        _klasId = klas.Id;

        // Leerling koppelen aan klas
        leerling.KlasId = _klasId;
        await _context.SaveChangesAsync();

        // Resultaten aanmaken voor de leerling
        var oefening = new Oefening { Id = 1, Naam = "Treble Clef", Niveau = Niveau.Beginner };
        _context.Oefeningen.Add(oefening);

        var resultaat1 = new Resultaat
        {
            Score = 80,
            GoedeAntwoorden = 4,
            AantalVragen = 5,
            UserId = _leerlingUserId,
            OefeningId = 1,
            datetime = DateTime.Now.AddDays(-1)
        };
        var resultaat2 = new Resultaat
        {
            Score = 60,
            GoedeAntwoorden = 3,
            AantalVragen = 5,
            UserId = _leerlingUserId,
            OefeningId = 1,
            datetime = DateTime.Now
        };
        _context.Resultaten.AddRange(resultaat1, resultaat2);
        await _context.SaveChangesAsync();

        // Setup ResultaatController met ingelogde gebruiker
        _resultaatController = new ResultaatController(_context, _userManager);
    }

    [TestMethod]
    public async Task GetMijnResultatenLeerling()
    {
        // Arrange - setup leerling context
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, _leerlingUserId) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _resultaatController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        // Act
        var result = await _resultaatController.GetMijnResultaten();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var resultaten = okResult.Value as List<ResultaatDto>;
        Assert.IsNotNull(resultaten);
        Assert.AreEqual(2, resultaten.Count, "Leerling zou 2 resultaten moeten hebben");

        // Controleer of de resultaten van de juiste leerling zijn
        foreach (var r in resultaten)
        {
            Assert.AreEqual("Treble Clef", r.OefeningNaam);
        }
    }

    [TestMethod]
    public async Task GetLeerlingVoortgangDocent()
    {
        // Arrange - setup docent context
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, _docentUserId) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _resultaatController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        // Act
        var result = await _resultaatController.GetLeerlingVoortgang(_leerlingId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var voortgang = okResult.Value as LeerlingVoortgangDto;
        Assert.IsNotNull(voortgang);
        Assert.AreEqual("leerling@test.nl", voortgang.LeerlingEmail);
        Assert.AreEqual("Test Klas", voortgang.KlasNaam);
        Assert.AreEqual(2, voortgang.Resultaten.Count, "Leerling zou 2 resultaten moeten hebben");

        // Controleer of resultaten gesorteerd zijn
        Assert.IsTrue(voortgang.Resultaten[0].Datum > voortgang.Resultaten[1].Datum,
            "Resultaten moeten gesorteerd zijn op datum (nieuwste eerst)");
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context?.Database.EnsureDeleted();
        _context?.Dispose();
    }
}