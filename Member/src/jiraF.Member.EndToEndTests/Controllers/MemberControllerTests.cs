﻿using jiraF.Member.API.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace jiraF.Member.EndToEndTests.Controllers;

public class MemberControllerTests
{
    private readonly HttpClient _client;

    public MemberControllerTests()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    });
                });
            });

        _client = application.CreateClient();
    }

    [Theory]
    [InlineData("/Member/2f857708-6e97-413b-b495-f2161135616a")]
    [InlineData("/Member/2f857708-6e97-413b-b495-f2161135616b")]
    [InlineData("/Member/2f857708-6e97-413b-b495-f2161135616c")]
    public async Task CheckAllGETApiMethodsIsValid_StatusCode200(string url)
    {
        HttpResponseMessage response = await _client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
    }
}
