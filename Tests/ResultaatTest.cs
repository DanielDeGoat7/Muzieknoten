using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Controllers;
using Piano.Data;
using Piano.Models;

namespace Piano.Tests;

[TestClass]
public class ResultaatTestScore
{
    [TestMethod]
    public void WeergaveScoreFormaat()
    {
        var dto = new ResultaatDto
        {
            GoedeAntwoorden = 8,
            AantalVragen = 10,
            Percentage = 80
        };

        var weergave = dto.WeergaveScore;

        Assert.AreEqual("8/10 (80%)", weergave);
    }

    [TestMethod]
    public void WeergaveScore0Goed()
    {
        var dto = new ResultaatDto
        {
            GoedeAntwoorden = 0,
            AantalVragen = 10,
            Percentage = 0
        };

        var weergave = dto.WeergaveScore;

        Assert.AreEqual("0/10 (0%)", weergave);
    }
}

[TestClass]
public class ResultaatTestDatum
{
    [TestMethod]
    public void SorteemOpDatum()
    {
        var resultaten = new List<ResultaatDto>
        {
            new ResultaatDto { Datum = new DateTime(2026, 05, 20, 14, 30, 00) },
            new ResultaatDto { Datum = new DateTime(2026, 05, 22, 10, 0, 0) },
            new ResultaatDto { Datum = new DateTime(2026, 05, 21, 9, 15, 0) }
        };

        var gesorteerd = resultaten.OrderByDescending(r => r.Datum).ToList();


        Assert.AreEqual(22, gesorteerd[0].Datum.Day);
        Assert.AreEqual(21, gesorteerd[1].Datum.Day);
        Assert.AreEqual(20, gesorteerd[2].Datum.Day);
    }
}