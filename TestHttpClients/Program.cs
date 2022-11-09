using Core;
using Core.Cryptography;
using CustomersService.TransportTypes.TransportServices;
using Newtonsoft.Json;
using OauthAuthorization.TransportTypes.TransportServices;
using System.Text;

Thread.Sleep(5000);

var oauthHttpService = new OauthHttpService("http://localhost:5233/api");

var issuer = Guid.Parse("A087A6B0-1544-4E1A-8BAF-44A21625A465");
var accepter = Guid.Parse("73AC39B1-C724-48B7-8FFB-40700D2EBCDE");

var codeResult = await oauthHttpService.Authorize(issuer, accepter, "123qwe123");
if (codeResult.Status == ServiceResult.Failure)
    Console.WriteLine(codeResult.Message);

var tokenResult = await oauthHttpService.Token(issuer, accepter, codeResult.Payload.Code);
if (tokenResult.Status == ServiceResult.Failure)
    Console.WriteLine(codeResult.Message);

var verifyResult = await oauthHttpService.Verify(issuer, accepter, tokenResult.Payload.Token);
if (tokenResult.Status == ServiceResult.Success)
    Console.WriteLine(JsonConvert.SerializeObject(verifyResult.Payload));