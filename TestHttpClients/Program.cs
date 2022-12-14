using Core.Cryptography;

var randomizer = new Randomizer();
var token = randomizer.RandomString(100, true);

Console.Write(token);