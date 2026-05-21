using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Controllers;
using Piano.Data;
using Piano.Models;

namespace Piano.Tests;

[TestClass]
public class AccountTest
{
    [TestMethod]
    public void WachtwoordMinimaal6TekensOngeldig()
    {
        string wachtwoord = "abcde";

        bool isGeldig = wachtwoord.Length >= 6;

        Assert.IsFalse(isGeldig);
    }

    [TestMethod]
    public void WachtwoordZonderHoofdletterOngeldig()
    {
        string wachtwoord = "test123";

        bool heeftHoofdletter = wachtwoord.Any(char.IsUpper);

        Assert.IsFalse(heeftHoofdletter);
    }

    [TestMethod]
    public void WachtwoordZonderCijferOngeldig()
    {
        string wachtwoord = "TestTest";

        bool heeftCijfer = wachtwoord.Any(char.IsDigit);

        Assert.IsFalse(heeftCijfer);
    }

    [TestMethod]
    public void WachtwoordGeldig()
    {
        string wachtwoord = "Test123";

        bool isMinimaal6Tekens = wachtwoord.Length >= 6;
        bool heeftHoofdletter = wachtwoord.Any(char.IsUpper);
        bool heeftCijfer = wachtwoord.Any(char.IsDigit);

        Assert.IsTrue(isMinimaal6Tekens && heeftHoofdletter && heeftCijfer);
    }
}

