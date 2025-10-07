using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolyhydraGames.Api.Steam.Models;
using PolyhydraGames.Api.Steam.Services;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Models;
using PolyhydraGames.Extensions;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
namespace Api.Steam.Test;

public enum TestType
{
    All,
    Platform,
    Title,
    Description,
    ImageUrl,
    BackgroundImageUrl
}