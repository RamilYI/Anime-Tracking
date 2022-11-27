// See https://aka.ms/new-console-template for more information

using AnimeTrackingApi;
using AnimeTrackingApi.Dto;

var animeTracking = new AnimeTracking();
var result = animeTracking.GetTitleSchedule("Chainsaw Man").Result;

Console.WriteLine("Hello, World!");