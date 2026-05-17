using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Controllers;
using Piano.Data;
using Piano.Models;
using System.Security.Claims;
using Moq;

namespace Piano.Tests;

[TestClass]
public class KlasTest
{
    private ApplicationDbContext? _context;
    private KlasController? _controller;
    private string? _testUserId;

    [TestInitialize]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        _testUserId = Guid.NewGuid().ToString();
        var docent = new Docent
        {
            Id = 1,
            Naam = "Test Docent",
            IdentityUserId = _testUserId
        };
        _context.Docenten.Add(docent);
        await _context.SaveChangesAsync();



        var mockUserManager = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object,
            null!, null!, null!, null!, null!, null!, null!, null!
        );

        var currentUser = new IdentityUser { Id = _testUserId };
        mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(currentUser);

        _controller = new KlasController(_context, mockUserManager.Object);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, _testUserId)
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [TestMethod]
    public async Task MaakKlasAanInDatabase()
    {
        // Arrange
        var dto = new CreateKlasDto { Naam = "Integratie Test Klas" };

        // Act
        var result = await _controller!.CreateKlas(dto);

        // Assert
        var klasInDb = await _context!.Klassen.FirstOrDefaultAsync(k => k.Naam == "Integratie Test Klas");

        Assert.IsNotNull(klasInDb);
        Assert.AreEqual("Integratie Test Klas", klasInDb.Naam);
        Assert.AreEqual(_testUserId, klasInDb.DocentIdentityUserId);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context?.Database.EnsureDeleted();
        _context?.Dispose();
    }
}