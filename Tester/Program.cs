// See https://aka.ms/new-console-template for more information
using Test.Api.Domain;
using Test.AppService.Infrastructure;
using Tester;
using TestWebApi.Models;

Console.WriteLine("Hello, World!");

var map = new DataMapper<IForkLift, ForkLiftModel>().Include(x=>x.ModifiedBy , x=>x.ModifiedBy.Name).Include(x=>x.ModifiedAt , x=>x.ModifiedAt.ToLocalTime()).Compile();
//map.Map(new ForkLiftModel(), new ForkLiftModel());
var obj = map.New(new ForkLiftModel());
Console.ReadLine();

